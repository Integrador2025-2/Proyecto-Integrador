"""
Servicio para generar cotizaciones en formato colombiano usando RAG y Gemini.
Adaptado para el Sistema General de Regalías (SGR) y entidades públicas colombianas.
"""

import os
import re
import json
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
        tasa_iva: float = 0.19,
        project_description: Optional[str] = None,
        project_context: Optional[str] = None
    ) -> Dict[str, Any]:
        """
        Generar cotización desde archivo Excel.

        Args:
            file_path: Ruta al archivo Excel
            incluir_iva: Si True, incluye IVA (19%) en la cotización
            tasa_iva: Tasa de IVA (por defecto 0.19 = 19%)
            project_description: Descripción del proyecto (opcional, para estimación de valores)
            project_context: Contexto adicional del proyecto (opcional, para estimación de valores)

        Returns:
            Diccionario con la cotización en formato markdown y datos estructurados
        """
        try:
            # Paso 1: Leer y validar Excel
            items_validos = await self._leer_y_validar_excel(file_path)

            if not items_validos:
                raise ValueError(
                    "No se encontraron ítems válidos para cotizar en el archivo.")

            # Paso 1.5: Estimar valores faltantes si hay ítems que lo necesitan
            items_con_valores_estimados = await self._estimar_valores_faltantes(
                items_validos,
                project_description=project_description,
                project_context=project_context
            )

            # Paso 2: Agrupar por actividad
            items_por_actividad = self._agrupar_por_actividad(items_con_valores_estimados)

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
                items_con_valores_estimados, incluir_iva, tasa_iva)

            # Contar ítems estimados
            items_estimados = sum(1 for item in items_con_valores_estimados if item.get("necesita_estimacion", False) and item.get("valor_estimado", False))

            return {
                "cotizacion_markdown": cotizacion_markdown,
                "items": items_con_valores_estimados,
                "items_por_actividad": items_por_actividad,
                "totales": totales,
                "incluye_iva": incluir_iva,
                "tasa_iva": tasa_iva,
                "fecha_generacion": datetime.now().isoformat(),
                "total_items": len(items_con_valores_estimados),
                "items_estimados": items_estimados
            }

        except Exception as e:
            logger.error(f"Error generando cotización: {str(e)}")
            raise

    async def _leer_y_validar_excel(self, file_path: str) -> List[Dict[str, Any]]:
        """
        Lee todas las hojas del Excel que contengan las columnas mínimas:
        ACTIVIDAD, CANTIDAD, VALOR UNITARIO.
        
        Ignora hojas que no tengan esas columnas (como "RESUMEN", "Portada", etc.).
        Devuelve una lista plana de ítems con el campo 'hoja_origen'.
        """
        excel_file = None
        try:
            # 1. Obtener todas las hojas del Excel
            excel_file = pd.ExcelFile(file_path)
            sheet_names = excel_file.sheet_names
            
            logger.info(f"Hojas encontradas en el Excel: {', '.join(sheet_names)}")
            
            todos_los_items = []
            
            # 2. Iterar sobre cada hoja
            for sheet_name in sheet_names:
                try:
                    # 2.1. Leer la hoja sin encabezado para buscar la fila de inicio
                    df_raw = pd.read_excel(excel_file, sheet_name=sheet_name, header=None)
                    
                    # 2.2. Buscar fila que contenga "ACTIVIDAD", "CANTIDAD" y "VALOR UNITARIO"
                    header_idx = None
                    for idx, row in df_raw.iterrows():
                        valores = [str(c).strip().lower() for c in row.values if pd.notna(c)]
                        # Verificar que tenga las tres columnas requeridas
                        tiene_actividad = any("actividad" in v for v in valores)
                        tiene_cantidad = any("cantidad" in v for v in valores)
                        tiene_valor_unitario = any("valor unitario" in v or "v. unitario" in v or "costo unitario" in v or "precio unitario" in v for v in valores)
                        
                        if tiene_actividad and tiene_cantidad and tiene_valor_unitario:
                            header_idx = idx
                            break
                    
                    # 2.3. Si no se encontró la fila de encabezados, ignorar esta hoja
                    if header_idx is None:
                        logger.info(f"Hoja '{sheet_name}' ignorada: no tiene columnas válidas.")
                        continue
                    
                    # 2.4. Re-leer con el encabezado correcto
                    df = pd.read_excel(excel_file, sheet_name=sheet_name, header=header_idx)
                    
                    # 2.5. Normalizar nombres de columnas
                    df.columns = df.columns.str.strip().str.upper()
                    
                    # 2.6. Validar columnas mínimas requeridas
                    req = ["ACTIVIDAD", "CANTIDAD", "VALOR UNITARIO"]
                    faltan = [c for c in req if c not in df.columns]
                    if faltan:
                        logger.info(f"Hoja '{sheet_name}' ignorada: faltan columnas {', '.join(faltan)}.")
                        continue
                    
                    # 2.7. Buscar columna de descripción/ítem (puede tener diferentes nombres)
                    # ---------- DESCRIPCIÓN POR HOJA SGR (OFICIAL) ----------
                    hoja_a_desc = {
                        "01. Talento Humano": "CARGO ESPECÍFICO",
                        "02. Equipos y Software": "EQUIPOS Y SOFTWARE (DESCRIPCIÓN)",
                        "03. Capacitación y Eventos": "CAPACITACIÓN Y EVENTOS (DESCRIPCIÓN)",
                        "04. Servicios Tecnológicos": "SERVICIOS TECNOLÓGICOS (DESCRIPCIÓN)",
                        "05. Materiales, insumos y Doc": "MATERIALES E INSUMOS (DESCRIPCIÓN)",
                        "06. Protección conocimiento y Di": "PROTECCIÓN CONOCIMIENTO Y DIFUSIÓN (DESCRIPCIÓN)",
                        "07. Gastos de viaje": "GASTOS DE VIAJE (DESCRIPCIÓN)",
                        "11. Otros": "OTROS (DESCRIPCIÓN)"
                    }
                    
                    item_col = hoja_a_desc.get(sheet_name)
                    if item_col is None or item_col not in df.columns:
                        # Fallback por palabras clave
                        for col in df.columns:
                            if any(k in col.lower() for k in ["descripción", "descripcion", "concepto", "item", "servicio"]):
                                item_col = col
                                break
                    if item_col is None:
                        item_col = "ACTIVIDAD"
                    
                    logger.debug(f"Hoja '{sheet_name}': usando columna '{item_col}' para descripción de ítems.")
                    
                    # 2.8. Filtrar filas que no sean ítems válidos (ignorar totales, subtotales, etc.)
                    df = df[~df["ACTIVIDAD"].astype(str).str.lower().str.contains("|".join(self.ignore_keywords), na=False, regex=True)]
                    
                    # 2.9. NO filtrar por valores faltantes - capturar todos los ítems con descripción
                    # Los ítems sin valor serán estimados después
                    
                    if df.empty:
                        logger.info(f"Hoja '{sheet_name}': no se encontraron ítems válidos después del filtrado.")
                        continue
                    
                    # 2.10. Mapear a formato estándar y agregar hoja_origen
                    items_hoja = []
                    for idx, row in df.iterrows():
                        actividad = str(row["ACTIVIDAD"]).strip() if pd.notna(row["ACTIVIDAD"]) else ""
                        
                        # Extraer descripción del ítem de la columna correspondiente
                        item_desc = ""
                        if item_col in row.index:
                            item_valor = row[item_col]
                            if pd.notna(item_valor):
                                item_desc = str(item_valor).strip()
                        
                        # Si no hay descripción o es solo un número, buscar en otras columnas
                        if not item_desc or (item_desc.isdigit() and len(item_desc) < 5):
                            # Buscar en otras columnas que no sean numéricas ni requeridas
                            columnas_alternativas = [
                                c for c in df.columns 
                                if c not in req + ["VALOR TOTAL", "TOTAL", "SUBTOTAL", "JUSTIFICACIÓN", "JUSTIFICACION"]
                                and c != item_col
                                and not str(c).strip().isdigit()
                                and not str(c).lower().startswith("unnamed")
                            ]
                            
                            for col_alt in columnas_alternativas:
                                if col_alt in row.index:
                                    valor_alt = row[col_alt]
                                    if pd.notna(valor_alt):
                                        valor_alt_str = str(valor_alt).strip()
                                        # Si tiene texto significativo (más de 3 caracteres y no es solo número)
                                        if len(valor_alt_str) > 3 and not (valor_alt_str.replace(".", "").replace(",", "").isdigit()):
                                            item_desc = valor_alt_str
                                            break
                        
                        # Si aún no hay descripción o es solo un número, usar la actividad
                        if not item_desc or (item_desc.isdigit() and len(item_desc) < 5):
                            item_desc = actividad
                        
                        # Si aún no hay descripción válida, saltar esta fila
                        if not item_desc:
                            continue
                        
                        cantidad = pd.to_numeric(row["CANTIDAD"], errors="coerce")
                        valor_unitario = pd.to_numeric(row["VALOR UNITARIO"], errors="coerce")
                        
                        # Determinar si necesita estimación
                        necesita_estimacion = False
                        if pd.isna(cantidad) or cantidad <= 0:
                            cantidad = 1.0  # Default a 1 si no hay cantidad
                            necesita_estimacion = True
                        
                        if pd.isna(valor_unitario) or valor_unitario <= 0:
                            valor_unitario = 0.0  # Se estimará después
                            necesita_estimacion = True
                        
                        valor_total = cantidad * valor_unitario if valor_unitario > 0 else 0.0
                        
                        item = {
                            "actividad": actividad,
                            "item": item_desc,
                            "cantidad": float(cantidad),
                            "valor_unitario": float(valor_unitario),
                            "valor_total": float(valor_total),
                            "hoja_origen": sheet_name,
                            "necesita_estimacion": necesita_estimacion
                        }
                        
                        items_hoja.append(item)
                    
                    if items_hoja:
                        todos_los_items.extend(items_hoja)
                        logger.info(f"Hoja '{sheet_name}': {len(items_hoja)} ítems válidos extraídos.")
                    else:
                        logger.info(f"Hoja '{sheet_name}': no se encontraron ítems válidos.")
                        
                except Exception as e:
                    logger.warning(f"Error procesando hoja '{sheet_name}': {str(e)}. Continuando con la siguiente hoja.")
                    continue
            
            # 3. Validar que se encontraron ítems
            if not todos_los_items:
                raise ValueError(
                    "No se encontraron ítems válidos en ninguna hoja del archivo Excel. "
                    "Asegúrate de que al menos una hoja contenga las columnas: ACTIVIDAD, CANTIDAD, VALOR UNITARIO."
                )
            
            logger.info(f"Total de ítems extraídos de todas las hojas: {len(todos_los_items)}")
            return todos_los_items

        except ValueError:
            raise
        except Exception as e:
            logger.error(f"Error leyendo Excel: {str(e)}")
            raise ValueError(f"Error procesando el archivo Excel: {str(e)}")
        finally:
            # Cerrar el archivo Excel explícitamente para liberar el recurso
            if excel_file is not None:
                try:
                    excel_file.close()
                except Exception as e:
                    logger.warning(f"Error cerrando archivo Excel: {str(e)}")

    async def _estimar_valores_faltantes(
        self,
        items: List[Dict[str, Any]],
        project_description: Optional[str] = None,
        project_context: Optional[str] = None
    ) -> List[Dict[str, Any]]:
        """
        Estimar valores faltantes para ítems que no tienen valor unitario.
        Usa el LLM para generar estimaciones basadas en descripción, actividad y contexto del proyecto.
        
        Args:
            items: Lista de ítems, algunos pueden necesitar estimación
            project_description: Descripción del proyecto (opcional)
            project_context: Contexto adicional del proyecto (opcional)
        
        Returns:
            Lista de ítems con valores estimados cuando corresponda
        """
        # Separar ítems que necesitan estimación de los que no
        items_sin_estimacion = [item for item in items if not item.get("necesita_estimacion", False)]
        items_para_estimar = [item for item in items if item.get("necesita_estimacion", False)]
        
        if not items_para_estimar:
            logger.info("No hay ítems que necesiten estimación de valores.")
            return items
        
        logger.info(f"Estimando valores para {len(items_para_estimar)} ítems sin valor.")
        
        # Si no hay LLM disponible, usar valores por defecto
        if not self.use_llm or not self.llm_service:
            logger.warning("LLM no disponible. Usando valores por defecto para ítems sin valor.")
            for item in items_para_estimar:
                # Valores por defecto según la hoja de origen
                valor_default = self._obtener_valor_default_por_hoja(item.get("hoja_origen", ""))
                item["valor_unitario"] = valor_default
                item["valor_total"] = item["cantidad"] * valor_default
                item["valor_estimado"] = True
                item["necesita_estimacion"] = False
            return items_sin_estimacion + items_para_estimar
        
        # Agrupar ítems por hoja para estimar en batch
        items_por_hoja = {}
        for item in items_para_estimar:
            hoja = item.get("hoja_origen", "General")
            if hoja not in items_por_hoja:
                items_por_hoja[hoja] = []
            items_por_hoja[hoja].append(item)
        
        # Estimar valores para cada grupo
        items_estimados = []
        for hoja, items_hoja in items_por_hoja.items():
            try:
                valores_estimados = await self._estimar_valores_con_llm(
                    items_hoja,
                    hoja,
                    project_description=project_description,
                    project_context=project_context
                )
                
                # Actualizar ítems con valores estimados
                for i, item in enumerate(items_hoja):
                    if i < len(valores_estimados):
                        valor_estimado = valores_estimados[i]
                        item["valor_unitario"] = valor_estimado
                        item["valor_total"] = item["cantidad"] * valor_estimado
                        item["valor_estimado"] = True
                        item["necesita_estimacion"] = False
                    items_estimados.append(item)
                    
            except Exception as e:
                logger.error(f"Error estimando valores para hoja '{hoja}': {str(e)}")
                # En caso de error, usar valores por defecto
                for item in items_hoja:
                    valor_default = self._obtener_valor_default_por_hoja(hoja)
                    item["valor_unitario"] = valor_default
                    item["valor_total"] = item["cantidad"] * valor_default
                    item["valor_estimado"] = True
                    item["necesita_estimacion"] = False
                items_estimados.extend(items_hoja)
        
        logger.info(f"Valores estimados para {len(items_estimados)} ítems.")
        return items_sin_estimacion + items_estimados
    
    async def _estimar_valores_con_llm(
        self,
        items: List[Dict[str, Any]],
        hoja_origen: str,
        project_description: Optional[str] = None,
        project_context: Optional[str] = None
    ) -> List[float]:
        """
        Usar LLM para estimar valores unitarios de ítems.
        
        Returns:
            Lista de valores unitarios estimados en COP
        """
        # Preparar información de ítems para el prompt
        items_texto = []
        for i, item in enumerate(items, 1):
            items_texto.append(
                f"Ítem {i}:\n"
                f"- Descripción: {item.get('item', 'N/A')}\n"
                f"- Actividad: {item.get('actividad', 'N/A')}\n"
                f"- Cantidad: {item.get('cantidad', 1)}\n"
                f"- Hoja: {hoja_origen}\n"
            )
        
        items_info = "\n".join(items_texto)
        
        # Construir contexto del proyecto
        contexto_proyecto = ""
        if project_description:
            contexto_proyecto += f"\n\nDESCRIPCIÓN DEL PROYECTO:\n{project_description}"
        if project_context:
            contexto_proyecto += f"\n\nCONTEXTO ADICIONAL:\n{project_context[:2000]}"  # Limitar longitud
        
        system_prompt = """Eres un experto en estimación de costos para proyectos de investigación e innovación en Colombia.
Tu tarea es estimar valores unitarios realistas en PESOS COLOMBIANOS (COP) para ítems de presupuesto.

IMPORTANTE:
- Todos los valores deben estar en PESOS COLOMBIANOS (COP)
- Los valores deben ser realistas para el mercado colombiano (2024-2025)
- Considera el tipo de ítem, la actividad y el contexto del proyecto
- Los valores deben ser razonables según la categoría (Talento Humano, Equipos, Servicios, etc.)

FORMATO DE RESPUESTA:
Debes responder ÚNICAMENTE con un objeto JSON válido con un array de valores numéricos.
NO incluyas texto adicional antes o después del JSON.

Ejemplo de formato:
{
    "valores_estimados": [5000000, 1500000, 2000000, 800000]
}

Donde cada número es el valor unitario estimado en COP para cada ítem en el mismo orden que se presentaron."""
        
        user_prompt = f"""Estima valores unitarios realistas (en COP) para los siguientes ítems:

HOJA DE ORIGEN: {hoja_origen}

ÍTEMS A ESTIMAR:
{items_info}
{contexto_proyecto}

Considera:
1. El tipo de ítem según su descripción
2. La actividad a la que pertenece
3. La hoja de origen (categoría SGR)
4. El contexto del proyecto si está disponible
5. Precios de mercado colombiano actuales

Responde SOLO con el JSON en el formato especificado, sin texto adicional."""
        
        try:
            respuesta = await self.llm_service.generate_answer(
                question="Estima los valores unitarios",
                context=user_prompt,
                system_prompt=system_prompt
            )
            
            # Parsear respuesta JSON
            # Limpiar respuesta si viene con markdown
            respuesta_limpia = respuesta.strip()
            if "```json" in respuesta_limpia:
                respuesta_limpia = re.sub(r'```json\s*', '', respuesta_limpia)
                respuesta_limpia = re.sub(r'\s*```', '', respuesta_limpia)
            elif "```" in respuesta_limpia:
                respuesta_limpia = re.sub(r'```\s*', '', respuesta_limpia)
                respuesta_limpia = re.sub(r'\s*```', '', respuesta_limpia)
            
            datos = json.loads(respuesta_limpia)
            valores = datos.get("valores_estimados", [])
            
            # Validar que tengamos el mismo número de valores que ítems
            if len(valores) != len(items):
                logger.warning(f"Se estimaron {len(valores)} valores pero hay {len(items)} ítems. Completando con valores por defecto.")
                # Completar con valores por defecto
                while len(valores) < len(items):
                    valor_default = self._obtener_valor_default_por_hoja(hoja_origen)
                    valores.append(valor_default)
            
            # Asegurar que todos los valores sean positivos
            valores = [max(0, float(v)) for v in valores[:len(items)]]
            
            return valores
            
        except Exception as e:
            logger.error(f"Error estimando valores con LLM: {str(e)}")
            # Retornar valores por defecto
            return [self._obtener_valor_default_por_hoja(hoja_origen) for _ in items]
    
    def _obtener_valor_default_por_hoja(self, hoja: str) -> float:
        """
        Obtener valor por defecto según la hoja de origen.
        Valores en COP típicos para cada categoría SGR.
        """
        valores_default = {
            "01. Talento Humano": 5000000.0,  # 5M COP/mes típico
            "02. Equipos y Software": 2000000.0,  # 2M COP por equipo
            "03. Capacitación y Eventos": 500000.0,  # 500K COP por evento
            "04. Servicios Tecnológicos": 1500000.0,  # 1.5M COP por servicio
            "05. Materiales, insumos y Doc": 300000.0,  # 300K COP por material
            "06. Protección conocimiento y Di": 800000.0,  # 800K COP
            "07. Gastos de viaje": 500000.0,  # 500K COP por viaje
            "11. Otros": 1000000.0  # 1M COP por defecto
        }
        
        # Buscar coincidencia parcial
        for hoja_key, valor in valores_default.items():
            if hoja_key in hoja or hoja in hoja_key:
                return valor
        
        # Valor por defecto genérico
        return 1000000.0

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
