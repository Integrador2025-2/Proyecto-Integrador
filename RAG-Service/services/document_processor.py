import os
import tempfile
from typing import Dict, Any
from fastapi import UploadFile
import PyPDF2
from docx import Document
import pandas as pd

class DocumentProcessor:
    """Procesador de documentos para extraer texto de diferentes formatos"""
    
    def __init__(self):
        self.supported_formats = {
            '.pdf': self._process_pdf,
            '.docx': self._process_docx,
            '.txt': self._process_txt,
            '.xlsx': self._process_xlsx
        }
    
    async def process_document(self, file: UploadFile) -> str:
        """Procesar un documento y extraer su contenido de texto"""
        try:
            # Obtener extensión del archivo
            file_extension = os.path.splitext(file.filename)[1].lower()
            
            if file_extension not in self.supported_formats:
                raise ValueError(f"Formato de archivo no soportado: {file_extension}")
            
            # Leer contenido del archivo
            content = await file.read()
            
            # Crear archivo temporal
            with tempfile.NamedTemporaryFile(delete=False, suffix=file_extension) as temp_file:
                temp_file.write(content)
                temp_file_path = temp_file.name
            
            try:
                # Procesar según el tipo de archivo
                processor = self.supported_formats[file_extension]
                text_content = processor(temp_file_path)
                
                return text_content
                
            finally:
                # Limpiar archivo temporal
                os.unlink(temp_file_path)
                
        except Exception as e:
            raise Exception(f"Error procesando documento {file.filename}: {str(e)}")
    
    def _process_pdf(self, file_path: str) -> str:
        """Procesar archivo PDF"""
        text = ""
        try:
            with open(file_path, 'rb') as file:
                pdf_reader = PyPDF2.PdfReader(file)
                for page in pdf_reader.pages:
                    text += page.extract_text() + "\n"
        except Exception as e:
            raise Exception(f"Error procesando PDF: {str(e)}")
        
        return text.strip()
    
    def _process_docx(self, file_path: str) -> str:
        """Procesar archivo DOCX"""
        try:
            doc = Document(file_path)
            text = ""
            for paragraph in doc.paragraphs:
                text += paragraph.text + "\n"
        except Exception as e:
            raise Exception(f"Error procesando DOCX: {str(e)}")
        
        return text.strip()
    
    def _process_txt(self, file_path: str) -> str:
        """Procesar archivo TXT"""
        try:
            with open(file_path, 'r', encoding='utf-8') as file:
                text = file.read()
        except UnicodeDecodeError:
            # Intentar con diferentes encodings
            try:
                with open(file_path, 'r', encoding='latin-1') as file:
                    text = file.read()
            except Exception as e:
                raise Exception(f"Error procesando TXT: {str(e)}")
        except Exception as e:
            raise Exception(f"Error procesando TXT: {str(e)}")
        
        return text.strip()
    
    def _process_xlsx(self, file_path: str) -> str:
        """Procesar archivo XLSX"""
        excel_file = None
        text = ""
        try:
            # Leer todas las hojas del archivo Excel
            excel_file = pd.ExcelFile(file_path)
            sheet_names = excel_file.sheet_names  # Guardar nombres de hojas antes de cerrar
            
            # Leer todas las hojas y procesarlas
            for sheet_name in sheet_names:
                # Leer la hoja usando el objeto ExcelFile ya abierto
                df = pd.read_excel(excel_file, sheet_name=sheet_name)
                text += f"Hoja: {sheet_name}\n"
                text += df.to_string(index=False) + "\n\n"
            
            # Cerrar el archivo Excel explícitamente antes de salir
            excel_file.close()
            excel_file = None  # Marcar como cerrado
            
        except Exception as e:
            # Asegurarse de cerrar el archivo incluso si hay un error
            if excel_file is not None:
                try:
                    excel_file.close()
                except:
                    pass
            raise Exception(f"Error procesando XLSX: {str(e)}")
        
        return text.strip()
    
    def extract_metadata(self, content: str, filename: str) -> Dict[str, Any]:
        """Extraer metadatos del contenido del documento"""
        metadata = {
            "filename": filename,
            "word_count": len(content.split()),
            "char_count": len(content),
            "has_budget_info": self._detect_budget_keywords(content),
            "has_project_info": self._detect_project_keywords(content)
        }
        
        return metadata
    
    def _detect_budget_keywords(self, content: str) -> bool:
        """Detectar si el documento contiene información de presupuesto"""
        budget_keywords = [
            "presupuesto", "costo", "precio", "valor", "gasto",
            "inversión", "financiamiento", "recursos", "rubro",
            "talento humano", "servicios tecnológicos", "equipos",
            "materiales", "capacitación", "viajes"
        ]
        
        content_lower = content.lower()
        return any(keyword in content_lower for keyword in budget_keywords)
    
    def _detect_project_keywords(self, content: str) -> bool:
        """Detectar si el documento contiene información de proyecto"""
        project_keywords = [
            "proyecto", "objetivo", "alcance", "metodología",
            "cronograma", "actividades", "entregables", "resultados"
        ]
        
        content_lower = content.lower()
        return any(keyword in content_lower for keyword in project_keywords)
