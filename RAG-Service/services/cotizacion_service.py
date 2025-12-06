"""
Servicio para generar cotizaciones en formato colombiano usando RAG y Gemini.
Adaptado para el Sistema General de Regalías (SGR) y entidades públicas colombianas.
"""

import os
import re
import pandas as pd
import tempfile
from typing import Dict, Any, List, Optional
from datetime import datetime
import logging

from .llm_service import LLMService

logger = logging.getLogger(__name__)


class CotizacionService:
    """
    Servicio para generar cotizaciones formales en formato colombiano.
    Lee archivos Excel, valida ítems y genera cotizaciones usando Gemini.
    """

    def __init__(self):
        # Inicializar servicio LLM
        try:
            self.llm_service = LLMService()
            self.use_llm = True
        except Exception as e:
            self.llm_service = None
            self.use_llm = False
            logger.warning(
                f"LLM no disponible para generación de cotizaciones. Error: {str(e)}")

        # Palabras clave para identificar columnas en español colombiano
        self.column_keywords = {
            "actividad": ["actividad", "descripción", "descripcion", "concepto", "item", "ítem", "producto", "servicio"],
            "cantidad": ["cantidad", "cant", "unidades", "número", "numero", "qty", "qty.", "unid"],
            "valor_unitario": [
                "valor unitario", "v. unitario", "costo unitario", "c. unitario",
                "precio unitario", "p. unitario", "valor/u", "precio/u"
            ],
            "valor_total": [
                "valor total", "v. total", "costo total", "c. total",
                "total", "subtotal", "monto", "valor"
            ],
            "justificacion": [
                "justificación", "justificacion", "descripción técnica",
                "especificaciones", "observaciones", "notas"
            ]
        }

        # Palabras a ignorar en filas (no son ítems cotizables)
        self.ignore_keywords = [
            "total", "subtotal", "suma", "gran total", "total general",
            "nota", "notas", "observación", "observaciones", "pie de página",
            "encabezado", "título", "resumen", "conclusión"
        ]

    async def generar_cotizacion_desde_excel(
        self,
        file_path: str,
        incluir_iva: bool = False,
        tasa_iva: float = 0.19
    ) -> Dict[str, Any]:
        """
        Generar cotización desde archivo Excel.

        Args:
            file_path: Ruta al archivo Excel
            incluir_iva: Si True, incluye IVA (19%) en la cotización
            tasa_iva: Tasa de IVA (por defecto 0.19 = 19%)

        Returns:
            Diccionario con la cotización en formato markdown y datos estructurados
        """
        try:
            # Paso 1: Leer y validar Excel
            items_validos = await self._leer_y_validar_excel(file_path)

            if not items_validos:
                raise ValueError(
                    "No se encontraron ítems válidos para cotizar en el archivo.")

            # Paso 2: Agrupar por actividad
            items_por_actividad = self._agrupar_por_actividad(items_validos)

            # Paso 3: Generar cotización con Gemini
            if self.use_llm and self.llm_service:
                cotizacion_markdown = await self._generar_cotizacion_con_gemini(
                    items_por_actividad,
                    incluir_iva=incluir_iva,
                    tasa_iva=tasa_iva
                )
            else:
                # Fallback: generar cotización básica sin LLM
                cotizacion_markdown = self._generar_cotizacion_basica(
                    items_por_actividad,
                    incluir_iva=incluir_iva,
                    tasa_iva=tasa_iva
                )

            # Paso 4: Calcular totales
            totales = self._calcular_totales(
                items_validos, incluir_iva, tasa_iva)

            return {
                "cotizacion_markdown": cotizacion_markdown,
                "items": items_validos,
                "items_por_actividad": items_por_actividad,
                "totales": totales,
                "incluye_iva": incluir_iva,
                "tasa_iva": tasa_iva,
                "fecha_generacion": datetime.now().isoformat(),
                "total_items": len(items_validos)
            }

        except Exception as e:
            logger.error(f"Error generando cotización: {str(e)}")
            raise

    async def _leer_y_validar_excel(self, file_path: str) -> List[Dict[str, Any]]:
        """
        Lee solo la hoja '02. Equipos y Software' y salta encabezados hasta encontrar ACTIVIDAD.
        """
        try:
            # 1. Leer solo la hoja que SÍ tiene los datos
            df_raw = pd.read_excel(
                file_path, sheet_name="02. Equipos y Software", header=None)

            # 2. Buscar fila que contenga "ACTIVIDAD" y "CANTIDAD" como encabezado
            header_idx = None
            for idx, row in df_raw.iterrows():
                valores = [str(c).strip().lower()
                           for c in row.values if pd.notna(c)]
                if "actividad" in valores and "cantidad" in valores:
                    header_idx = idx
                    break

            if header_idx is None:
                raise ValueError(
                    "No se encontró la fila de encabezados con ACTIVIDAD y CANTIDAD.")

            # 3. Re-leer con el encabezado correcto
            df = pd.read_excel(
                file_path, sheet_name="02. Equipos y Software", header=header_idx)

            # 4. Normalizar nombres de columnas
            df.columns = df.columns.str.strip().str.upper()

            # 5. Validar columnas mínimas
            req = ["ACTIVIDAD", "CANTIDAD", "VALOR UNITARIO"]
            faltan = [c for c in req if c not in df.columns]
            if faltan:
                raise ValueError(f"Faltan columnas: {', '.join(faltan)}")

             # 6. Filtrar solo filas con cantidad y valor válidos
            df = df.dropna(subset=["CANTIDAD", "VALOR UNITARIO"])
            df = df[pd.to_numeric(df["CANTIDAD"], errors="coerce") > 0]
            df = df[pd.to_numeric(df["VALOR UNITARIO"], errors="coerce") > 0]

            # 7. Mapear a formato estándar
            df = df.rename(columns={
                "ACTIVIDAD": "actividad",
                "EQUIPOS Y SOFTWARE (DESCRIPCIÓN)": "item",
                "CANTIDAD": "cantidad",
                "VALOR UNITARIO": "valor_unitario"
            })
            df["valor_total"] = df["cantidad"] * df["valor_unitario"]

            return df[["actividad", "item", "cantidad", "valor_unitario", "valor_total"]].to_dict(orient="records")

        except ValueError:
            raise
        except Exception as e:
            logger.error(f"Error leyendo Excel: {str(e)}")
        raise ValueError(f"Error procesando el archivo Excel: {str(e)}")

    async def _encontrar_fila_inicio_datos(self, excel_file: pd.ExcelFile, sheet_name: str) -> Optional[int]:
        """
        Encontrar la fila donde empiezan los datos reales (donde aparece "ACTIVIDAD" como encabezado).

        Busca en las primeras 50 filas del Excel la palabra "ACTIVIDAD" (o variantes).

        Returns:
            Índice de la fila (0-based) donde empiezan los datos, o None si no se encuentra
        """
        try:
            # Leer las primeras 50 filas sin encabezado para buscar la fila de inicio
            df_temp = pd.read_excel(
                excel_file, sheet_name=sheet_name, header=None, nrows=50)

            # Palabras clave para identificar la fila de encabezados
            keywords_actividad = ["actividad", "descripción",
                                  "descripcion", "concepto", "item", "ítem"]
            keywords_cantidad = ["cantidad", "cant",
                                 "unidades", "número", "numero"]
            keywords_valor = ["valor unitario", "v. unitario",
                              "costo unitario", "precio unitario"]

            # Buscar fila que contenga al menos "ACTIVIDAD" y "CANTIDAD" y "VALOR UNITARIO"
            for idx, row in df_temp.iterrows():
                # Convertir toda la fila a string y buscar palabras clave
                fila_texto = " ".join([str(cell).lower().strip()
                                      for cell in row.values if pd.notna(cell)])

                # Verificar si esta fila contiene las palabras clave necesarias
                tiene_actividad = any(
                    keyword in fila_texto for keyword in keywords_actividad)
                tiene_cantidad = any(
                    keyword in fila_texto for keyword in keywords_cantidad)
                tiene_valor = any(
                    keyword in fila_texto for keyword in keywords_valor)

                # Si tiene al menos ACTIVIDAD y CANTIDAD, probablemente es la fila de encabezados
                if tiene_actividad and tiene_cantidad and tiene_valor:
                    logger.info(
                        f"Fila de encabezados encontrada en la fila {idx + 1} (índice {idx})")
                    return idx

            # Si no se encontró, intentar buscar solo "ACTIVIDAD"
            for idx, row in df_temp.iterrows():
                fila_texto = " ".join([str(cell).lower().strip()
                                      for cell in row.values if pd.notna(cell)])
                if any(keyword in fila_texto for keyword in keywords_actividad):
                    logger.info(
                        f"Fila con 'ACTIVIDAD' encontrada en la fila {idx + 1} (índice {idx})")
                    return idx

            return None

        except Exception as e:
            logger.warning(
                f"Error buscando fila de inicio en hoja '{sheet_name}': {str(e)}")
            return None

    def _validar_columnas_requeridas(self, df: pd.DataFrame) -> Dict[str, Any]:
        """
        Validar que el DataFrame tenga las columnas requeridas.

        Returns:
            Dict con 'valido', 'faltantes', y 'mapeo' de columnas
        """
        columnas_df = [str(col).strip() for col in df.columns]
        columnas_lower = [col.lower() for col in columnas_df]

        mapeo = {}
        faltantes = []

        # Buscar columna de ACTIVIDAD (requerida)
        actividad_encontrada = False
        for col_type, keywords in self.column_keywords.items():
            if col_type == "actividad":
                for keyword in keywords:
                    for i, col_lower in enumerate(columnas_lower):
                        if keyword in col_lower:
                            mapeo["actividad"] = columnas_df[i]
                            actividad_encontrada = True
                            break
                    if actividad_encontrada:
                        break
                break

        if not actividad_encontrada:
            faltantes.append("ACTIVIDAD")

        # Buscar columna de CANTIDAD (requerida)
        cantidad_encontrada = False
        for keyword in self.column_keywords["cantidad"]:
            for i, col_lower in enumerate(columnas_lower):
                if keyword in col_lower:
                    mapeo["cantidad"] = columnas_df[i]
                    cantidad_encontrada = True
                    break
            if cantidad_encontrada:
                break

        if not cantidad_encontrada:
            faltantes.append("CANTIDAD")

        # Buscar columna de VALOR UNITARIO (requerida)
        valor_unitario_encontrado = False
        for keyword in self.column_keywords["valor_unitario"]:
            for i, col_lower in enumerate(columnas_lower):
                if keyword in col_lower:
                    mapeo["valor_unitario"] = columnas_df[i]
                    valor_unitario_encontrado = True
                    break
            if valor_unitario_encontrado:
                break

        if not valor_unitario_encontrado:
            faltantes.append("VALOR UNITARIO")

        # Buscar columna de VALOR TOTAL (opcional, se puede calcular)
        valor_total_encontrado = False
        for keyword in self.column_keywords["valor_total"]:
            for i, col_lower in enumerate(columnas_lower):
                if keyword in col_lower:
                    mapeo["valor_total"] = columnas_df[i]
                    valor_total_encontrado = True
                    break
            if valor_total_encontrado:
                break

        # Buscar columna de JUSTIFICACIÓN (opcional)
        justificacion_encontrada = False
        for keyword in self.column_keywords["justificacion"]:
            for i, col_lower in enumerate(columnas_lower):
                if keyword in col_lower:
                    mapeo["justificacion"] = columnas_df[i]
                    justificacion_encontrada = True
                    break
            if justificacion_encontrada:
                break

        valido = len(faltantes) == 0

        if not valido:
            raise ValueError(
                f"El archivo no contiene las columnas requeridas: {', '.join(faltantes)}. "
                f"Columnas encontradas: {', '.join(columnas_df)}"
            )

        return {
            "valido": valido,
            "faltantes": faltantes,
            "mapeo": mapeo
        }

    async def _extraer_items_validos(
        self,
        df: pd.DataFrame,
        sheet_name: str,
        mapeo_columnas: Dict[str, str]
    ) -> List[Dict[str, Any]]:
        """
        Extraer ítems válidos del DataFrame.

        Valida:
        - CANTIDAD > 0
        - VALOR UNITARIO > 0
        - Ignora filas con palabras clave (TOTAL, SUBTOTAL, etc.)
        """
        items_validos = []

        for idx, row in df.iterrows():
            # Obtener valor de actividad
            actividad_col = mapeo_columnas["actividad"]
            actividad_valor = row[actividad_col]

            # Validar que no sea vacío
            if pd.isna(actividad_valor) or str(actividad_valor).strip() == "":
                continue

            actividad_str = str(actividad_valor).strip()

            # Ignorar filas con palabras clave (TOTAL, SUBTOTAL, NOTA, etc.)
            actividad_lower = actividad_str.lower()
            if any(keyword in actividad_lower for keyword in self.ignore_keywords):
                logger.debug(
                    f"Ignorando fila con palabra clave: {actividad_str}")
                continue

            # Validar CANTIDAD
            cantidad_col = mapeo_columnas["cantidad"]
            cantidad = self._safe_numeric(row[cantidad_col])

            if cantidad is None or cantidad <= 0:
                logger.debug(
                    f"Ignorando fila con cantidad inválida: {actividad_str}")
                continue

            # Validar VALOR UNITARIO
            valor_unitario_col = mapeo_columnas["valor_unitario"]
            valor_unitario = self._safe_numeric(row[valor_unitario_col])

            if valor_unitario is None or valor_unitario <= 0:
                logger.debug(
                    f"Ignorando fila con valor unitario inválido: {actividad_str}")
                continue

            # Calcular o obtener VALOR TOTAL
            if "valor_total" in mapeo_columnas:
                valor_total = self._safe_numeric(
                    row[mapeo_columnas["valor_total"]])
                if valor_total is None or valor_total <= 0:
                    # Calcular si no está presente o es inválido
                    valor_total = cantidad * valor_unitario
            else:
                # Calcular valor total
                valor_total = cantidad * valor_unitario

            # Obtener justificación si existe
            justificacion = ""
            if "justificacion" in mapeo_columnas:
                justif_valor = row[mapeo_columnas["justificacion"]]
                if pd.notna(justif_valor):
                    justificacion = str(justif_valor).strip()

            # Crear ítem
            item = {
                "actividad": actividad_str,
                "cantidad": cantidad,
                "valor_unitario": valor_unitario,
                "valor_total": valor_total,
                "justificacion": justificacion,
                "hoja_origen": sheet_name,
                "fila_origen": idx + 2  # +2 porque Excel empieza en 1 y tiene header
            }

            items_validos.append(item)

        return items_validos

    def _agrupar_por_actividad(self, items: List[Dict[str, Any]]) -> Dict[str, List[Dict[str, Any]]]:
        """
        Agrupar ítems por actividad.

        Si hay una columna "ACTIVIDAD" explícita, agrupa por su valor.
        Si no, agrupa todos los ítems en una sola actividad "General".
        """
        agrupados = {}

        for item in items:
            actividad = item.get("actividad", "General")

            # Normalizar nombre de actividad (usar como clave de agrupación)
            # Si la actividad es muy específica (parece un ítem), agrupar en "General"
            if len(actividad) < 10 or any(char.isdigit() for char in actividad[:5]):
                # Parece ser un ítem específico, no una actividad
                actividad_grupo = "General"
            else:
                actividad_grupo = actividad

            if actividad_grupo not in agrupados:
                agrupados[actividad_grupo] = []

            agrupados[actividad_grupo].append(item)

        return agrupados

    async def _generar_cotizacion_con_gemini(
        self,
        items_por_actividad: Dict[str, List[Dict[str, Any]]],
        incluir_iva: bool = False,
        tasa_iva: float = 0.19
    ) -> str:
        """
        Generar cotización en formato markdown usando Gemini.
        """
        # Preparar datos para el prompt
        datos_actividades = []
        for actividad, items in items_por_actividad.items():
            items_texto = []
            for item in items:
                items_texto.append({
                    "descripcion": item["actividad"],
                    "cantidad": item["cantidad"],
                    "valor_unitario": item["valor_unitario"],
                    "valor_total": item["valor_total"],
                    "justificacion": item.get("justificacion", "")
                })

            datos_actividades.append({
                "actividad": actividad,
                "items": items_texto
            })

        # Calcular totales
        total_sin_iva = sum(
            sum(item["valor_total"] for item in items)
            for items in items_por_actividad.values()
        )

        iva = total_sin_iva * tasa_iva if incluir_iva else 0
        total_con_iva = total_sin_iva + iva

        system_prompt = """Eres un experto en generar cotizaciones formales para el Sistema General de Regalías (SGR) y entidades públicas colombianas.

Tu tarea es generar una cotización profesional en formato de tabla markdown, usando terminología colombiana formal.

REGLAS ESTRICTAS:
1. Debes responder ÚNICAMENTE con una tabla markdown, sin texto adicional antes o después
2. Usa terminología colombiana: "Ítem", "Valor unitario", "Valor total", "Subtotal por actividad", "Total general"
3. Formato de moneda: $1.234.567 COP (con puntos como separadores de miles)
4. Agrupa los ítems por actividad
5. Incluye subtotal por cada actividad
6. Al final, incluye Total general
7. Si se solicita IVA, incluye fila de IVA (19%) y Total con IVA

FORMATO DE LA TABLA MARKDOWN:
- Primera columna: Número de ítem (1, 2, 3...)
- Segunda columna: Descripción del ítem
- Tercera columna: Cantidad
- Cuarta columna: Valor unitario ($ COP)
- Quinta columna: Valor total ($ COP)
- Usa encabezados: | Ítem | Descripción | Cantidad | Valor unitario | Valor total |

EJEMPLO DE ESTRUCTURA:
| Ítem | Descripción | Cantidad | Valor unitario | Valor total |
|------|-------------|----------|----------------|-------------|
| **ACTIVIDAD: [Nombre de la actividad]** | | | | |
| 1 | Descripción del ítem 1 | 10 | $150.000 COP | $1.500.000 COP |
| 2 | Descripción del ítem 2 | 5 | $200.000 COP | $1.000.000 COP |
| | **Subtotal por actividad** | | | **$2.500.000 COP** |
| | | | | |
| **ACTIVIDAD: [Otra actividad]** | | | | |
| 3 | Descripción del ítem 3 | 2 | $500.000 COP | $1.000.000 COP |
| | **Subtotal por actividad** | | | **$1.000.000 COP** |
| | | | | |
| | **TOTAL GENERAL** | | | **$3.500.000 COP** |
| | IVA (19%) | | | **$665.000 COP** |
| | **TOTAL CON IVA** | | | | **$4.165.000 COP** |

IMPORTANTE:
- NO incluyas texto antes o después de la tabla
- NO uses código markdown adicional (```)
- Formatea todos los valores con puntos como separadores de miles
- Mantén el formato consistente y profesional"""

        user_prompt = f"""Genera una cotización en formato de tabla markdown con los siguientes datos:

DATOS DE LAS ACTIVIDADES:
{self._formatear_datos_para_prompt(datos_actividades)}

TOTALES:
- Total sin IVA: ${total_sin_iva:,.0f} COP
{"- IVA (19%): $" + f"{iva:,.0f} COP" if incluir_iva else ""}
{"- Total con IVA: $" + f"{total_con_iva:,.0f} COP" if incluir_iva else ""}

INSTRUCCIONES:
{"- INCLUYE IVA (19%) y Total con IVA al final de la tabla" if incluir_iva else "- NO incluyas IVA"}
- Agrupa los ítems por actividad
- Incluye subtotal por cada actividad
- Usa formato de moneda colombiana ($1.234.567 COP)
- Responde SOLO con la tabla markdown, sin texto adicional"""

        try:
            respuesta = await self.llm_service.generate_answer(
                question="Genera la cotización",
                context=user_prompt,
                system_prompt=system_prompt
            )

            # Limpiar respuesta (remover markdown code blocks si existen)
            respuesta_limpia = respuesta.strip()
            if respuesta_limpia.startswith("```"):
                # Remover code blocks
                respuesta_limpia = re.sub(r'^```[\w]*\n', '', respuesta_limpia)
                respuesta_limpia = re.sub(r'\n```$', '', respuesta_limpia)

            return respuesta_limpia.strip()

        except Exception as e:
            logger.error(f"Error generando cotización con Gemini: {str(e)}")
            # Fallback a generación básica
            return self._generar_cotizacion_basica(items_por_actividad, incluir_iva, tasa_iva)

    def _formatear_datos_para_prompt(self, datos_actividades: List[Dict[str, Any]]) -> str:
        """Formatear datos de actividades para el prompt de Gemini"""
        texto = ""
        item_num = 1

        for actividad_data in datos_actividades:
            actividad = actividad_data["actividad"]
            items = actividad_data["items"]

            texto += f"\nACTIVIDAD: {actividad}\n"
            texto += "Ítems:\n"

            for item in items:
                texto += f"  {item_num}. {item['descripcion']}\n"
                texto += f"     Cantidad: {item['cantidad']}\n"
                texto += f"     Valor unitario: ${item['valor_unitario']:,.0f} COP\n"
                texto += f"     Valor total: ${item['valor_total']:,.0f} COP\n"
                if item.get('justificacion'):
                    texto += f"     Justificación: {item['justificacion']}\n"
                texto += "\n"
                item_num += 1

        return texto

    def _generar_cotizacion_basica(
        self,
        items_por_actividad: Dict[str, List[Dict[str, Any]]],
        incluir_iva: bool = False,
        tasa_iva: float = 0.19
    ) -> str:
        """
        Generar cotización básica sin LLM (fallback).
        """
        markdown = "| Ítem | Descripción | Cantidad | Valor unitario | Valor total |\n"
        markdown += "|------|-------------|----------|----------------|------------|\n"

        item_num = 1
        total_general = 0.0

        for actividad, items in items_por_actividad.items():
            # Encabezado de actividad
            markdown += f"| **ACTIVIDAD: {actividad}** | | | | |\n"

            subtotal_actividad = 0.0

            # Ítems de la actividad
            for item in items:
                descripcion = item["actividad"]
                cantidad = item["cantidad"]
                valor_unitario = item["valor_unitario"]
                valor_total = item["valor_total"]

                markdown += f"| {item_num} | {descripcion} | {cantidad} | ${valor_unitario:,.0f} COP | ${valor_total:,.0f} COP |\n"

                subtotal_actividad += valor_total
                total_general += valor_total
                item_num += 1

            # Subtotal por actividad
            markdown += f"| | **Subtotal por actividad** | | | **${subtotal_actividad:,.0f} COP** |\n"
            markdown += "| | | | | |\n"

        # Total general
        markdown += f"| | **TOTAL GENERAL** | | | **${total_general:,.0f} COP** |\n"

        # IVA si aplica
        if incluir_iva:
            iva = total_general * tasa_iva
            total_con_iva = total_general + iva
            markdown += f"| | IVA (19%) | | | **${iva:,.0f} COP** |\n"
            markdown += f"| | **TOTAL CON IVA** | | | **${total_con_iva:,.0f} COP** |\n"

        return markdown

    def _calcular_totales(
        self,
        items: List[Dict[str, Any]],
        incluir_iva: bool = False,
        tasa_iva: float = 0.19
    ) -> Dict[str, float]:
        """Calcular totales de la cotización"""
        subtotal = sum(item["valor_total"] for item in items)
        iva = subtotal * tasa_iva if incluir_iva else 0
        total = subtotal + iva

        return {
            "subtotal": subtotal,
            "iva": iva,
            "total": total,
            "tasa_iva": tasa_iva if incluir_iva else 0
        }

    def _safe_numeric(self, value: Any) -> Optional[float]:
        """Convertir valor a numérico de forma segura"""
        if pd.isna(value):
            return None

        try:
            if isinstance(value, str):
                # Limpiar formato (comas, símbolos de moneda, etc.)
                cleaned = re.sub(r'[^\d.,\-]', '', value)
                # Manejar separadores
                if ',' in cleaned and '.' not in cleaned:
                    cleaned = cleaned.replace(',', '.')
                elif ',' in cleaned and '.' in cleaned:
                    # Asumir que coma es separador de miles
                    cleaned = cleaned.replace(',', '')

                return float(cleaned) if cleaned else None

            return float(value)
        except (ValueError, TypeError):
            return None
