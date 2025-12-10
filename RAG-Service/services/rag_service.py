import os
import uuid
from typing import List, Dict, Any, Optional
import chromadb
from chromadb.config import Settings
from sentence_transformers import SentenceTransformer
import numpy as np
from datetime import datetime

from models.schemas import Activity, Resource, ResourceAssignment
from .llm_service import LLMService
    
class RAGService:
    """Servicio RAG para búsqueda semántica y generación de respuestas"""
    
    def __init__(self):
        # Inicializar ChromaDB
        self.chroma_client = chromadb.PersistentClient(
            path="./chroma_db",
            settings=Settings(anonymized_telemetry=False)
        )
        
        # Inicializar modelo de embeddings
        self.embedding_model = SentenceTransformer('paraphrase-multilingual-MiniLM-L12-v2')
        
        # Inicializar servicio LLM (opcional, solo si está configurado)
        try:
            self.llm_service = LLMService()
            self.use_llm = True
        except Exception as e:
            # Si no hay API key configurada, usar modo sin LLM
            self.llm_service = None
            self.use_llm = False
            print(f"Advertencia: LLM no disponible. Usando modo básico. Error: {str(e)}")
        
        # Obtener o crear colección
        try:
            self.collection = self.chroma_client.get_collection("project_documents")
        except:
            self.collection = self.chroma_client.create_collection(
                name="project_documents",
                metadata={"description": "Documentos de proyectos para RAG"}
            )
    
    async def add_document(self, content: str, metadata: Dict[str, Any]) -> str:
        """Agregar un documento a la base de datos vectorial"""
        try:
            # Generar ID único para el documento
            document_id = str(uuid.uuid4())
            
            # Dividir contenido en chunks para mejor procesamiento
            chunks = self._split_text_into_chunks(content)
            
            # Generar embeddings para cada chunk
            embeddings = self.embedding_model.encode(chunks).tolist()
            
            # Limpiar metadatos: ChromaDB no acepta None, convertir a valores válidos
            cleaned_metadata = self._clean_metadata(metadata)
            
            # Preparar metadatos para cada chunk
            chunk_metadata = []
            chunk_ids = []
            
            for i, chunk in enumerate(chunks):
                chunk_id = f"{document_id}_chunk_{i}"
                chunk_metadata.append({
                    **cleaned_metadata,
                    "chunk_index": i,
                    "total_chunks": len(chunks),
                    "chunk_length": len(chunk),
                    "added_at": datetime.now().isoformat()
                })
                chunk_ids.append(chunk_id)
            
            # Agregar a ChromaDB
            self.collection.add(
                embeddings=embeddings,
                documents=chunks,
                metadatas=chunk_metadata,
                ids=chunk_ids
            )
            
            return document_id
            
        except Exception as e:
            raise Exception(f"Error agregando documento: {str(e)}")
    
    def _clean_metadata(self, metadata: Dict[str, Any]) -> Dict[str, Any]:
        """Limpiar metadatos para que sean compatibles con ChromaDB (no acepta None)"""
        cleaned = {}
        for key, value in metadata.items():
            if value is None:
                # Convertir None a string vacío o valor por defecto según el tipo esperado
                if key == "project_id":
                    # Si project_id es None, lanzar error ya que es obligatorio
                    raise ValueError("project_id es obligatorio y no puede ser None")
                else:
                    # Para otros campos None, usar string vacío
                    cleaned[key] = ""
            elif isinstance(value, (str, int, float, bool)):
                # ChromaDB acepta str, int, float, bool
                cleaned[key] = value
            else:
                # Convertir otros tipos a string
                cleaned[key] = str(value)
        return cleaned
    
    async def query(self, question: str, project_id: Optional[int] = None, top_k: int = 5) -> Dict[str, Any]:
        """Realizar consulta semántica sobre los documentos"""
        try:
            # Generar embedding para la pregunta
            query_embedding = self.embedding_model.encode([question]).tolist()[0]
            
            # Preparar filtros si se especifica project_id
            where_filter = None
            if project_id is not None:
                where_filter = {"project_id": project_id}
            
            # Buscar documentos similares
            results = self.collection.query(
                query_embeddings=[query_embedding],
                n_results=top_k,
                where=where_filter
            )
            
            # Procesar resultados
            sources = []
            relevant_docs = []
            
            if results['documents'] and results['documents'][0]:
                for i, doc in enumerate(results['documents'][0]):
                    source = {
                        "content": doc,
                        "metadata": results['metadatas'][0][i],
                        "similarity": 1 - results['distances'][0][i]  # Convertir distancia a similitud
                    }
                    sources.append(source)
                    relevant_docs.append(doc)
            
            # Generar respuesta basada en el contexto
            answer = await self._generate_answer(question, relevant_docs)
            
            # Calcular confianza basada en similitud promedio
            confidence = 0.0
            if sources:
                confidence = sum(source["similarity"] for source in sources) / len(sources)
            
            return {
                "answer": answer,
                "sources": sources,
                "confidence": confidence
            }
            
        except Exception as e:
            raise Exception(f"Error en consulta: {str(e)}")
    
    async def get_project_documents(self, project_id: int) -> List[Dict[str, Any]]:
        """Obtener todos los documentos de un proyecto específico"""
        try:
            results = self.collection.get(
                where={"project_id": project_id}
            )
            
            documents = []
            if results['metadatas']:
                # Agrupar chunks por documento
                doc_groups = {}
                for i, metadata in enumerate(results['metadatas']):
                    doc_id = metadata.get('filename', 'unknown')
                    if doc_id not in doc_groups:
                        doc_groups[doc_id] = {
                            "document_id": doc_id,
                            "filename": metadata.get('filename', 'unknown'),
                            "document_type": metadata.get('document_type', 'unknown'),
                            "project_id": metadata.get('project_id', project_id),  # Usar el project_id de metadata
                            "upload_date": metadata.get('added_at', ''),
                            "chunks": []
                        }
                    
                    doc_groups[doc_id]["chunks"].append({
                        "content": results['documents'][i],
                        "chunk_index": metadata.get('chunk_index', 0)
                    })
                
                # Convertir a lista y agregar preview del contenido
                for doc in doc_groups.values():
                    # Ordenar chunks por índice
                    doc["chunks"].sort(key=lambda x: x["chunk_index"])
                    # Crear preview con los primeros chunks
                    preview_chunks = doc["chunks"][:2]
                    doc["content_preview"] = " ".join([chunk["content"] for chunk in preview_chunks])
                    documents.append(doc)
            
            return documents
            
        except Exception as e:
            raise Exception(f"Error obteniendo documentos del proyecto: {str(e)}")
    
    async def delete_document(self, document_id: str):
        """Eliminar un documento de la base de datos vectorial"""
        try:
            # Buscar todos los chunks del documento
            results = self.collection.get(
                where={"filename": document_id}
            )
            
            if results['ids']:
                # Eliminar todos los chunks del documento
                self.collection.delete(ids=results['ids'])
            
        except Exception as e:
            raise Exception(f"Error eliminando documento: {str(e)}")
    
    def _split_text_into_chunks(self, text: str, chunk_size: int = 1000, overlap: int = 200) -> List[str]:
        """Dividir texto en chunks para procesamiento"""
        if len(text) <= chunk_size:
            return [text]
        
        chunks = []
        start = 0
        
        while start < len(text):
            end = start + chunk_size
            
            # Intentar dividir en un punto lógico (final de oración)
            if end < len(text):
                # Buscar el último punto, exclamación o interrogación
                for i in range(end, max(start + chunk_size // 2, end - 100), -1):
                    if text[i] in '.!?':
                        end = i + 1
                        break
            
            chunk = text[start:end].strip()
            if chunk:
                chunks.append(chunk)
            
            start = end - overlap
            if start >= len(text):
                break
        
        return chunks
    
    async def plan_resources(
        self,
        activities: List[Activity],
        resources: List[Resource],
        project_id: Optional[int] = None,
        max_budget: Optional[float] = None,
    ) -> Dict[str, Any]:
        """
        Generar un plan de asignación de recursos usando LLM si está disponible,
        o heurística básica como fallback.
        """
        if not activities or not resources:
            return {
                "project_id": project_id,
                "assignments": [],
                "summary": "No se proporcionaron actividades o recursos para planificar.",
                "criteria": [
                    "Validación básica de entrada: se requieren actividades y recursos."
                ],
                "confidence": 0.0,
            }

        # Intentar usar LLM si está disponible
        if self.use_llm and self.llm_service:
            try:
                # Convertir actividades y recursos a diccionarios
                activities_dict = [
                    {
                        "id": a.id,
                        "nombre": a.nombre,
                        "descripcion": a.descripcion or "",
                        "duracion_dias": a.duracion_dias,
                        "especificaciones_tecnicas": getattr(a, 'especificaciones_tecnicas', '') if hasattr(a, 'especificaciones_tecnicas') else ""
                    }
                    for a in activities
                ]
                
                resources_dict = [
                    {
                        "id": r.id,
                        "nombre": r.nombre,
                        "tipo": r.tipo,
                        "costo_unitario": r.costo_unitario,
                        "unidad": r.unidad,
                        "disponibilidad": r.disponibilidad
                    }
                    for r in resources
                ]
                
                # Obtener contexto del proyecto si hay project_id
                project_context = None
                if project_id:
                    try:
                        project_docs = await self.get_project_documents(project_id)
                        if project_docs:
                            project_context = " ".join([
                                " ".join([chunk["content"] for chunk in doc["chunks"]])
                                for doc in project_docs[:3]  # Limitar a 3 documentos
                            ])
                    except:
                        pass
                
                # Generar plan con LLM
                plan_result = await self.llm_service.generate_resource_plan(
                    activities=activities_dict,
                    resources=resources_dict,
                    project_context=project_context,
                    max_budget=max_budget
                )
                
                # Convertir asignaciones a ResourceAssignment
                assignments = []
                for assignment_dict in plan_result.get("assignments", []):
                    assignments.append(ResourceAssignment(
                        actividad_id=assignment_dict.get("actividad_id"),
                        actividad_nombre=assignment_dict.get("actividad_nombre", ""),
                        recurso_id=assignment_dict.get("recurso_id"),
                        recurso_nombre=assignment_dict.get("recurso_nombre", ""),
                        recurso_tipo=assignment_dict.get("recurso_tipo", ""),
                        cantidad=float(assignment_dict.get("cantidad", 0)),
                        costo_estimado=assignment_dict.get("costo_estimado"),
                        justificacion=assignment_dict.get("justificacion")
                    ))
                
                return {
                    "project_id": project_id,
                    "assignments": [a.dict() for a in assignments],
                    "summary": plan_result.get("summary", ""),
                    "criteria": plan_result.get("criteria", []),
                    "confidence": plan_result.get("confidence", 0.7)
                }
                
            except Exception as e:
                print(f"Error usando LLM para planificación, usando método básico: {str(e)}")
                # Continuar con método básico
        
        # Método básico (fallback)
        assignments: List[ResourceAssignment] = []
        remaining_budget = max_budget if max_budget is not None else None

        for activity in activities:
            assigned_resource: Optional[Resource] = None
            if resources:
                assigned_resource = resources[0]

            if not assigned_resource:
                continue

            cantidad = activity.duracion_dias or 1
            costo_estimado = None

            if assigned_resource.costo_unitario is not None:
                costo_estimado = assigned_resource.costo_unitario * cantidad

                if remaining_budget is not None and costo_estimado > remaining_budget:
                    continue

            if remaining_budget is not None and costo_estimado is not None:
                remaining_budget -= costo_estimado

            assignments.append(
                ResourceAssignment(
                    actividad_id=activity.id,
                    actividad_nombre=activity.nombre,
                    recurso_id=assigned_resource.id,
                    recurso_nombre=assigned_resource.nombre,
                    recurso_tipo=assigned_resource.tipo,
                    cantidad=float(cantidad),
                    costo_estimado=costo_estimado,
                    justificacion=(
                        "Asignación heurística inicial basada en el primer recurso disponible."
                    ),
                )
            )

        covered_activities = {a.actividad_nombre for a in assignments}
        coverage_ratio = (
            len(covered_activities) / len(activities) if activities else 0.0
        )

        summary = (
            f"Se generaron asignaciones para {len(assignments)} actividades "
            f"de un total de {len(activities)}."
        )

        return {
            "project_id": project_id,
            "assignments": [a.dict() for a in assignments],
            "summary": summary,
            "criteria": [
                "Asignación heurística usando el primer recurso disponible.",
                "Estimación de costo basada en costo_unitario x duración_dias.",
                "Respeto del presupuesto máximo cuando se proporcionó.",
            ],
            "confidence": float(coverage_ratio),
        }

    async def _generate_answer(self, question: str, context_docs: List[str]) -> str:
        """Generar respuesta basada en el contexto de los documentos"""
        if not context_docs:
            return "No se encontró información relevante para responder tu pregunta."
        
        # Combinar contexto
        context = "\n\n".join(context_docs)

        # Detectar si el usuario está pidiendo un resumen del proyecto (para cualquier proyecto)
        # y construir un prompt de sistema especializado para este tipo de tarea.
        system_prompt = None
        q_lower = question.lower()

        if (
            "resumen" in q_lower
            or "resumen ejecutivo" in q_lower
            or "summary" in q_lower
        ) and ("proyecto" in q_lower or "project" in q_lower):
            system_prompt = """
Eres un experto en formulación, evaluación y seguimiento de proyectos (especialmente en ciencia, tecnología, innovación y salud).
Debes elaborar RESÚMENES EJECUTIVOS COMPLETOS usando EXCLUSIVAMENTE la información disponible en el contexto proporcionado.

INSTRUCCIONES GENERALES:
- Responde SIEMPRE en español, con tono claro, profesional y bien estructurado.
- No inventes datos: si algo no aparece en el contexto, indícalo explícitamente.
- Evita centrarte en detalles administrativos muy específicos (como códigos internos, números de cotización, IDs, etc.) salvo que sean clave para entender el proyecto.

CUANDO TE PIDAN UN RESUMEN DE PROYECTO, INTENTA CUBRIR, SI APARECE EN EL CONTEXTO:
- Justificación y contexto del problema (por qué existe el proyecto, qué problema aborda, situación actual).
- Objetivo general y, si están definidos, los objetivos específicos (OE1, OE2, etc.) y los principales indicadores de éxito.
- Alcance y territorio de intervención (regiones, departamentos, municipios, instituciones o entornos donde se ejecuta).
- Población objetivo y beneficiarios (número de personas, grupos poblacionales, enfoques diferenciales como género, comunidades étnicas, víctimas del conflicto, etc.).
- Componentes, ejes o líneas de trabajo principales (por ejemplo: telemedicina, modelos predictivos, biomarcadores, plataformas tecnológicas, apropiación social del conocimiento, formación de talento humano, etc.).
- Actividades clave y metodología general (qué se hará de manera resumida para lograr los objetivos).
- Resultados esperados y productos principales (prototipos, publicaciones, políticas públicas, fortalecimiento institucional, etc.).
- Gobernanza y actores del proyecto (instituciones participantes, equipos, roles relevantes, alianzas, ecosistema de innovación) si la información está disponible.
- Consideraciones de presupuesto y recursos solo al nivel necesario para entender la magnitud del proyecto, sin entrar en tablas de costos detalladas.

FORMATO DE RESPUESTA:
- Estructura la respuesta en varios párrafos coherentes (al menos 4–6), organizados lógicamente.
- Puedes usar subtítulos en negrita (por ejemplo: **Justificación**, **Objetivos**, **Componentes principales**, **Resultados esperados**, etc.).
- Si un elemento importante no aparece en el contexto, menciona brevemente algo como: "En los documentos proporcionados no se encontró información explícita sobre X".
"""

        # Usar LLM si está disponible
        if self.use_llm and self.llm_service:
            try:
                return await self.llm_service.generate_answer(
                    question,
                    context,
                    system_prompt=system_prompt,
                )
            except Exception as e:
                # Si falla el LLM, usar método básico como fallback
                print(f"Error usando LLM, usando método básico: {str(e)}")
                return self._generate_basic_answer(question, context)
        else:
            # Método básico sin LLM
            return self._generate_basic_answer(question, context)
    
    def _generate_basic_answer(self, question: str, context: str) -> str:
        """Generar respuesta básica sin LLM (fallback)"""
        if "presupuesto" in question.lower() or "costo" in question.lower():
            return self._extract_budget_info(context)
        elif "actividad" in question.lower() or "tarea" in question.lower():
            return self._extract_activity_info(context)
        else:
            # Respuesta genérica basada en el contexto más relevante
            return context[:500] + "..." if len(context) > 500 else context
    
    def _extract_budget_info(self, context: str) -> str:
        """Extraer información de presupuesto del contexto"""
        # Buscar patrones relacionados con presupuesto
        budget_keywords = ["presupuesto", "costo", "precio", "valor", "gasto", "inversión"]
        lines = context.split('\n')
        
        budget_lines = []
        for line in lines:
            if any(keyword in line.lower() for keyword in budget_keywords):
                budget_lines.append(line.strip())
        
        if budget_lines:
            return "Información de presupuesto encontrada:\n" + "\n".join(budget_lines[:5])
        else:
            return "No se encontró información específica de presupuesto en los documentos."
    
    def _extract_activity_info(self, context: str) -> str:
        """Extraer información de actividades del contexto"""
        activity_keywords = ["actividad", "tarea", "objetivo", "metodología", "cronograma"]
        lines = context.split('\n')
        
        activity_lines = []
        for line in lines:
            if any(keyword in line.lower() for keyword in activity_keywords):
                activity_lines.append(line.strip())
        
        if activity_lines:
            return "Información de actividades encontrada:\n" + "\n".join(activity_lines[:5])
        else:
            return "No se encontró información específica de actividades en los documentos."
