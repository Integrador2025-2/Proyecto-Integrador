import os
import re
import pandas as pd
import tempfile
from typing import Dict, Any, List, Optional, Tuple
from datetime import datetime
import logging

logger = logging.getLogger(__name__)

class BudgetExtractor:
    """
    Servicio inteligente para extraer actividades y presupuestos desde documentos Excel/DOCX/PDF.
    Mapea automáticamente las columnas a los rubros del sistema.
    """
    
    def __init__(self):
        # Mapeo de palabras clave a rubros del sistema
        self.rubro_keywords = {
            "TalentoHumano": [
                "talento humano", "recurso humano", "personal", "salario", "honorario",
                "nómina", "empleado", "trabajador", "profesional", "investigador",
                "coordinador", "asistente", "cargo", "perfil", "contratación"
            ],
            "ServiciosTecnologicos": [
                "servicio", "servicios tecnológicos", "consultoría", "asesoría",
                "desarrollo", "implementación", "soporte técnico", "mantenimiento",
                "outsourcing", "tercerización", "contrato de servicios"
            ],
            "EquiposSoftware": [
                "equipo", "equipos", "software", "licencia", "hardware", "computador",
                "computadora", "servidor", "dispositivo", "herramienta", "tecnología",
                "aplicación", "sistema", "plataforma"
            ],
            "MaterialesInsumos": [
                "material", "materiales", "insumo", "insumos", "suministro", "consumible",
                "reactivo", "material de laboratorio", "fungible", "papelería"
            ],
            "CapacitacionEventos": [
                "capacitación", "capacitacion", "evento", "taller", "curso", "formación",
                "entrenamiento", "seminario", "congreso", "conferencia", "workshop"
            ],
            "GastosViaje": [
                "viaje", "viajes", "transporte", "desplazamiento", "movilización",
                "hospedaje", "alojamiento", "viático", "pasaje", "tiquete", "ticket",
                "hotel", "alimentación durante viaje"
            ]
        }
        
        # Palabras clave comunes para identificar columnas
        self.column_keywords = {
            "actividad": ["actividad", "tarea", "item", "descripción", "descripcion", "nombre", "concepto"],
            "cantidad": ["cantidad", "cant", "unidades", "número", "numero", "qty"],
            "valor_unitario": ["valor unitario", "costo unitario", "precio unitario", "v. unitario", "c. unitario"],
            "total": ["total", "valor total", "costo total", "subtotal", "monto"],
            "justificacion": ["justificación", "justificacion", "descripción técnica", "especificaciones"],
            "especificaciones": ["especificaciones", "especificaciones técnicas", "specs", "detalles técnicos"],
            "periodo": ["periodo", "período", "año", "anio", "mes", "trimestre", "semestre"],
            "rubro": ["rubro", "categoría", "categoria", "tipo", "clasificación", "clasificacion"]
        }
    
    def identify_rubro_from_text(self, text: str) -> Optional[str]:
        """Identificar el rubro al que pertenece un texto basándose en palabras clave"""
        if not text:
            return None
        
        text_lower = text.lower()
        
        # Calcular relevancia de cada rubro
        rubro_scores = {}
        for rubro, keywords in self.rubro_keywords.items():
            score = sum(1 for keyword in keywords if keyword in text_lower)
            if score > 0:
                rubro_scores[rubro] = score
        
        # Retornar el rubro con mayor puntuación
        if rubro_scores:
            return max(rubro_scores.items(), key=lambda x: x[1])[0]
        
        return None
    
    def identify_column_type(self, column_name: str) -> Optional[str]:
        """Identificar el tipo de columna basándose en su nombre"""
        if not column_name:
            return None
        
        col_lower = column_name.lower().strip()
        
        for col_type, keywords in self.column_keywords.items():
            if any(keyword in col_lower for keyword in keywords):
                return col_type
        
        return None
    
    async def extract_from_excel(self, file_path: str) -> Dict[str, Any]:
        """
        Extraer actividades y presupuesto de un archivo Excel.
        Detecta automáticamente las columnas y mapea a la estructura del sistema.
        """
        try:
            # Leer todas las hojas del Excel
            excel_file = pd.ExcelFile(file_path)
            all_activities = []
            
            for sheet_name in excel_file.sheet_names:
                logger.info(f"Procesando hoja: {sheet_name}")
                
                # Leer la hoja
                df = pd.read_excel(excel_file, sheet_name=sheet_name)
                
                # Detectar si esta hoja contiene presupuesto/actividades
                if self._is_budget_sheet(df):
                    activities = await self._extract_activities_from_dataframe(df, sheet_name)
                    all_activities.extend(activities)
            
            excel_file.close()
            
            # Agrupar actividades por rubro
            grouped_by_rubro = self._group_activities_by_rubro(all_activities)
            
            return {
                "activities": all_activities,
                "grouped_by_rubro": grouped_by_rubro,
                "total_activities": len(all_activities),
                "rubros_found": list(grouped_by_rubro.keys()),
                "extraction_method": "excel_intelligent",
                "confidence": 0.85 if all_activities else 0.0
            }
            
        except Exception as e:
            logger.error(f"Error extrayendo desde Excel: {str(e)}")
            raise Exception(f"Error procesando Excel: {str(e)}")
    
    def _is_budget_sheet(self, df: pd.DataFrame) -> bool:
        """Detectar si una hoja contiene información de presupuesto"""
        # Verificar si tiene al menos 3 columnas
        if len(df.columns) < 3:
            return False
        
        # Buscar palabras clave en los nombres de columnas
        columns_str = " ".join([str(col).lower() for col in df.columns])
        budget_indicators = [
            "actividad", "valor", "costo", "total", "presupuesto", 
            "rubro", "cantidad", "precio"
        ]
        
        return any(indicator in columns_str for indicator in budget_indicators)
    
    async def _extract_activities_from_dataframe(
        self, 
        df: pd.DataFrame, 
        sheet_name: str
    ) -> List[Dict[str, Any]]:
        """Extraer actividades de un DataFrame de pandas"""
        activities = []
        
        # Identificar columnas
        column_mapping = self._map_columns(df)
        
        logger.info(f"Mapeo de columnas en '{sheet_name}': {column_mapping}")
        
        # Iterar por filas
        for idx, row in df.iterrows():
            # Saltar filas vacías o de encabezado adicional
            if self._is_empty_or_header_row(row, column_mapping):
                continue
            
            # Extraer datos de la fila
            activity = await self._extract_activity_from_row(row, column_mapping, sheet_name)
            
            if activity:
                activities.append(activity)
        
        return activities
    
    def _map_columns(self, df: pd.DataFrame) -> Dict[str, str]:
        """Mapear columnas del DataFrame a tipos conocidos"""
        column_mapping = {}
        
        for col in df.columns:
            col_str = str(col)
            col_type = self.identify_column_type(col_str)
            if col_type:
                column_mapping[col_type] = col_str
        
        # Si no se encontró columna de actividad, usar la primera columna de texto
        if "actividad" not in column_mapping:
            for col in df.columns:
                if df[col].dtype == 'object':  # Columna de texto
                    column_mapping["actividad"] = col
                    break
        
        # Si no se encontró columna de total, usar la última columna numérica
        if "total" not in column_mapping:
            numeric_cols = df.select_dtypes(include=['number']).columns
            if len(numeric_cols) > 0:
                column_mapping["total"] = numeric_cols[-1]
        
        return column_mapping
    
    def _is_empty_or_header_row(self, row: pd.Series, column_mapping: Dict[str, str]) -> bool:
        """Verificar si una fila está vacía o es un encabezado adicional"""
        # Si la columna de actividad está vacía, saltar
        if "actividad" in column_mapping:
            actividad_col = column_mapping["actividad"]
            value = row[actividad_col]
            
            if pd.isna(value) or str(value).strip() == "":
                return True
            
            # Si parece ser un encabezado (palabras clave de encabezado)
            value_lower = str(value).lower()
            if any(header in value_lower for header in ["actividad", "descripción", "total", "subtotal"]):
                return True
        
        return False
    
    async def _extract_activity_from_row(
        self, 
        row: pd.Series, 
        column_mapping: Dict[str, str],
        sheet_name: str
    ) -> Optional[Dict[str, Any]]:
        """Extraer información de actividad desde una fila"""
        try:
            activity = {}
            
            # Nombre/Descripción de la actividad
            if "actividad" in column_mapping:
                nombre = str(row[column_mapping["actividad"]])
                if pd.notna(nombre) and nombre.strip():
                    activity["nombre"] = nombre.strip()
                else:
                    return None
            else:
                return None
            
            # Identificar rubro basándose en el nombre y/o sheet
            rubro_from_name = self.identify_rubro_from_text(activity["nombre"])
            rubro_from_sheet = self.identify_rubro_from_text(sheet_name)
            
            # Verificar si hay columna de rubro explícita
            rubro_from_column = None
            if "rubro" in column_mapping:
                rubro_value = row[column_mapping["rubro"]]
                if pd.notna(rubro_value):
                    rubro_from_column = self.identify_rubro_from_text(str(rubro_value))
            
            # Prioridad: columna > nombre > sheet
            activity["rubro"] = rubro_from_column or rubro_from_name or rubro_from_sheet or "Otros"
            
            # Extraer valores numéricos
            if "cantidad" in column_mapping:
                cantidad = row[column_mapping["cantidad"]]
                activity["cantidad"] = self._safe_numeric(cantidad, 1)
            else:
                activity["cantidad"] = 1
            
            if "valor_unitario" in column_mapping:
                valor_unitario = row[column_mapping["valor_unitario"]]
                activity["valor_unitario"] = self._safe_numeric(valor_unitario, 0)
            else:
                activity["valor_unitario"] = None
            
            if "total" in column_mapping:
                total = row[column_mapping["total"]]
                activity["total"] = self._safe_numeric(total, 0)
            else:
                # Calcular total si no está presente
                if activity["valor_unitario"] is not None:
                    activity["total"] = activity["valor_unitario"] * activity["cantidad"]
                else:
                    activity["total"] = None
            
            # Justificación/Especificaciones
            if "justificacion" in column_mapping:
                justif = row[column_mapping["justificacion"]]
                activity["justificacion"] = str(justif) if pd.notna(justif) else ""
            else:
                activity["justificacion"] = ""
            
            if "especificaciones" in column_mapping:
                specs = row[column_mapping["especificaciones"]]
                activity["especificaciones_tecnicas"] = str(specs) if pd.notna(specs) else ""
            else:
                activity["especificaciones_tecnicas"] = ""
            
            # Periodo
            if "periodo" in column_mapping:
                periodo = row[column_mapping["periodo"]]
                activity["periodo"] = self._safe_numeric(periodo, 1)
            else:
                activity["periodo"] = 1
            
            # Metadatos
            activity["source_sheet"] = sheet_name
            activity["has_budget_values"] = activity["total"] is not None and activity["total"] > 0
            
            return activity
            
        except Exception as e:
            logger.warning(f"Error extrayendo actividad de fila: {str(e)}")
            return None
    
    def _safe_numeric(self, value: Any, default: float = 0.0) -> Optional[float]:
        """Convertir un valor a numérico de forma segura"""
        if pd.isna(value):
            return default if default is not None else None
        
        try:
            # Si es string, limpiar formato (comas, símbolos de moneda, etc.)
            if isinstance(value, str):
                # Remover símbolos de moneda y separadores de miles
                cleaned = re.sub(r'[^\d.,\-]', '', value)
                # Reemplazar coma por punto si es separador decimal
                if ',' in cleaned and '.' not in cleaned:
                    cleaned = cleaned.replace(',', '.')
                elif ',' in cleaned and '.' in cleaned:
                    # Asumimos que coma es separador de miles
                    cleaned = cleaned.replace(',', '')
                
                return float(cleaned) if cleaned else default
            
            return float(value)
        except (ValueError, TypeError):
            return default
    
    def _group_activities_by_rubro(self, activities: List[Dict[str, Any]]) -> Dict[str, List[Dict[str, Any]]]:
        """Agrupar actividades por rubro"""
        grouped = {}
        
        for activity in activities:
            rubro = activity.get("rubro", "Otros")
            if rubro not in grouped:
                grouped[rubro] = []
            grouped[rubro].append(activity)
        
        return grouped
    
    async def extract_from_docx(self, file_path: str) -> Dict[str, Any]:
        """
        Extraer actividades desde un documento Word.
        Busca tablas y texto estructurado.
        """
        try:
            from docx import Document
            
            doc = Document(file_path)
            all_activities = []
            
            # Buscar en tablas
            for table_idx, table in enumerate(doc.tables):
                activities = await self._extract_from_docx_table(table, table_idx)
                all_activities.extend(activities)
            
            # Si no se encontraron tablas, buscar en texto
            if not all_activities:
                activities = await self._extract_from_docx_text(doc)
                all_activities.extend(activities)
            
            grouped_by_rubro = self._group_activities_by_rubro(all_activities)
            
            return {
                "activities": all_activities,
                "grouped_by_rubro": grouped_by_rubro,
                "total_activities": len(all_activities),
                "rubros_found": list(grouped_by_rubro.keys()),
                "extraction_method": "docx_intelligent",
                "confidence": 0.75 if all_activities else 0.0
            }
            
        except Exception as e:
            logger.error(f"Error extrayendo desde DOCX: {str(e)}")
            raise Exception(f"Error procesando DOCX: {str(e)}")
    
    async def _extract_from_docx_table(self, table, table_idx: int) -> List[Dict[str, Any]]:
        """Extraer actividades de una tabla en Word"""
        activities = []
        
        try:
            # Convertir tabla a DataFrame
            data = []
            for row in table.rows:
                row_data = [cell.text.strip() for cell in row.cells]
                data.append(row_data)
            
            if not data or len(data) < 2:
                return activities
            
            # Usar primera fila como headers
            df = pd.DataFrame(data[1:], columns=data[0])
            
            # Procesar como DataFrame
            if self._is_budget_sheet(df):
                activities = await self._extract_activities_from_dataframe(
                    df, 
                    f"Tabla_{table_idx + 1}"
                )
        
        except Exception as e:
            logger.warning(f"Error procesando tabla {table_idx}: {str(e)}")
        
        return activities
    
    async def _extract_from_docx_text(self, doc) -> List[Dict[str, Any]]:
        """Extraer actividades del texto plano del documento"""
        # Esta es una implementación básica
        # En producción, podrías usar NLP más avanzado o LLM para extraer
        activities = []
        
        # Por ahora, retornamos vacío (se puede mejorar con LLM)
        logger.info("Extracción desde texto plano de DOCX no implementada completamente")
        
        return activities
    
    def calculate_budget_summary(self, activities: List[Dict[str, Any]]) -> Dict[str, Any]:
        """Calcular resumen presupuestal"""
        total_budget = 0.0
        activities_with_budget = 0
        activities_without_budget = 0
        
        rubro_totals = {}
        
        for activity in activities:
            rubro = activity.get("rubro", "Otros")
            total = activity.get("total", 0) or 0
            
            if total > 0:
                activities_with_budget += 1
                total_budget += total
                
                if rubro not in rubro_totals:
                    rubro_totals[rubro] = 0.0
                rubro_totals[rubro] += total
            else:
                activities_without_budget += 1
        
        return {
            "total_budget": total_budget,
            "activities_with_budget": activities_with_budget,
            "activities_without_budget": activities_without_budget,
            "rubro_totals": rubro_totals,
            "needs_llm_generation": activities_without_budget > 0
        }




