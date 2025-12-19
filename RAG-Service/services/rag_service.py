import os
import uuid
from typing import List, Dict, Any, Optional
try:
    import chromadb
    from chromadb.config import Settings
    CHROMADB_AVAILABLE = True
except Exception as e:
    CHROMADB_AVAILABLE = False
    print(f"Warning: ChromaDB not available: {e}")
from sentence_transformers import SentenceTransformer
import numpy as np
from datetime import datetime
import logging

from models.schemas import Activity, Resource, ResourceAssignment
from .llm_service import LLMService

logger = logging.getLogger(__name__)
    
class RAGService:
    """Servicio RAG para búsqueda semántica y generación de respuestas"""
    
    def __init__(self):
        # Inicializar ChromaDB
        if CHROMADB_AVAILABLE:
            try:
                self.chroma_client = chromadb.PersistentClient(
                    path="./chroma_db",
                    settings=Settings(anonymized_telemetry=False)
                )
                # Obtener o crear colección
                try:
                    self.collection = self.chroma_client.get_collection("project_documents")
                except:
                    self.collection = self.chroma_client.create_collection(
                        name="project_documents",
                        metadata={"description": "Documentos de proyectos para RAG"}
                    )
            except Exception as e:
                print(f"Warning: ChromaDB initialization failed: {e}")
                self.chroma_client = None
                self.collection = None
        else:
            self.chroma_client = None
            self.collection = None
            print("Warning: Running without ChromaDB - vector search disabled")
        
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
                    # Si project_id es None, usar -1 como valor por defecto para indicar "sin proyecto"
                    # Esto permite que ChromaDB pueda filtrar correctamente
                    cleaned[key] = -1
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
    
    async def query(self, question: str, project_id: Optional[int] = None, top_k: int = 10) -> Dict[str, Any]:
        """
        Realizar consulta semántica sobre los documentos con recuperación mejorada
        
        Args:
            question: Pregunta del usuario
            project_id: ID del proyecto (opcional)
            top_k: Número de documentos a recuperar (por defecto 10 para más contexto)
        """
        try:
            # Generar embedding para la pregunta
            query_embedding = self.embedding_model.encode([question]).tolist()[0]
            
            # Preparar filtros si se especifica project_id
            where_filter = None
            if project_id is not None:
                where_filter = {"project_id": project_id}
            
            # Aumentar top_k para obtener más contexto (mínimo 10, máximo 20)
            effective_top_k = max(10, min(top_k, 20))
            
            # Buscar documentos similares
            results = self.collection.query(
                query_embeddings=[query_embedding],
                n_results=effective_top_k,
                where=where_filter
            )
            
            # Procesar resultados y filtrar por similitud mínima
            sources = []
            relevant_docs = []
            min_similarity = 0.3  # Umbral mínimo de similitud
            
            if results['documents'] and results['documents'][0]:
                for i, doc in enumerate(results['documents'][0]):
                    similarity = 1 - results['distances'][0][i]  # Convertir distancia a similitud
                    
                    # Solo incluir documentos con similitud razonable
                    if similarity >= min_similarity:
                        source = {
                            "content": doc,
                            "metadata": results['metadatas'][0][i],
                            "similarity": similarity
                        }
                        sources.append(source)
                        relevant_docs.append(doc)
            
            # Si no hay documentos relevantes, intentar con umbral más bajo
            if not relevant_docs and results['documents'] and results['documents'][0]:
                # Usar al menos los 3 más similares
                for i, doc in enumerate(results['documents'][0][:3]):
                    similarity = 1 - results['distances'][0][i]
                    source = {
                        "content": doc,
                        "metadata": results['metadatas'][0][i],
                        "similarity": similarity
                    }
                    sources.append(source)
                    relevant_docs.append(doc)
            
            # Generar respuesta basada en el contexto (ahora con más documentos)
            answer = await self._generate_answer(question, relevant_docs, project_id)
            
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
                            "project_id": project_id,
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

    async def _generate_answer(self, question: str, context_docs: List[str], project_id: Optional[int] = None) -> str:
        """
        Generar respuesta basada en el contexto de los documentos con mejoras para respuestas más completas
        
        Args:
            question: Pregunta del usuario
            context_docs: Lista de documentos relevantes recuperados
            project_id: ID del proyecto (opcional, para obtener contexto adicional)
        """
        if not context_docs:
            return "No se encontró información relevante para responder tu pregunta."
        
        # Combinar contexto con mejor organización
        # Agrupar documentos relacionados y ordenar por relevancia
        context = "\n\n--- Documento {0} ---\n{1}".format(
            "1", context_docs[0]
        ) if len(context_docs) == 1 else "\n\n".join(
            [f"--- Documento {i+1} ---\n{doc}" for i, doc in enumerate(context_docs)]
        )
        
        # Obtener contexto adicional del proyecto si está disponible
        additional_context = ""
        if project_id and self.use_llm:
            try:
                project_docs = await self.get_project_documents(project_id)
                if project_docs:
                    # Agregar información general del proyecto
                    project_summary = " ".join([
                        " ".join([chunk["content"] for chunk in doc["chunks"][:2]])
                        for doc in project_docs[:2]  # Primeros 2 documentos
                    ])
                    if project_summary and len(project_summary) > 100:
                        additional_context = f"\n\n--- Contexto adicional del proyecto ---\n{project_summary[:1500]}"
            except Exception as e:
                logger.warning(f"No se pudo obtener contexto adicional del proyecto: {str(e)}")
        
        # Combinar todo el contexto
        full_context = context + additional_context

        # Detectar tipo de pregunta y construir prompt especializado
        system_prompt = None
        q_lower = question.lower()

        if (
            "resumen" in q_lower
            or "resumen ejecutivo" in q_lower
            or "summary" in q_lower
        ) and ("proyecto" in q_lower or "project" in q_lower):
            system_prompt = """
Eres un experto en formulación, evaluación y seguimiento de proyectos (especialmente en ciencia, tecnología, innovación y salud).
Debes elaborar RESÚMENES EJECUTIVOS COMPLETOS Y EXHAUSTIVOS usando EXCLUSIVAMENTE la información disponible en el contexto proporcionado.

INSTRUCCIONES GENERALES:
- Responde SIEMPRE en español, con tono claro, profesional y bien estructurado.
- Proporciona respuestas COMPLETAS y DETALLADAS, no te limites a respuestas cortas.
- Utiliza TODA la información relevante del contexto proporcionado.
- No inventes datos: si algo no aparece en el contexto, indícalo explícitamente.
- Evita centrarte en detalles administrativos muy específicos (como códigos internos, números de cotización, IDs, etc.) salvo que sean clave para entender el proyecto.

CUANDO TE PIDAN UN RESUMEN DE PROYECTO, INTENTA CUBRIR EXHAUSTIVAMENTE, SI APARECE EN EL CONTEXTO:
- Justificación y contexto del problema (por qué existe el proyecto, qué problema aborda, situación actual, antecedentes relevantes).
- Objetivo general y, si están definidos, los objetivos específicos (OE1, OE2, etc.) y los principales indicadores de éxito.
- Alcance y territorio de intervención (regiones, departamentos, municipios, instituciones o entornos donde se ejecuta).
- Población objetivo y beneficiarios (número de personas, grupos poblacionales, enfoques diferenciales como género, comunidades étnicas, víctimas del conflicto, etc.).
- Componentes, ejes o líneas de trabajo principales (por ejemplo: telemedicina, modelos predictivos, biomarcadores, plataformas tecnológicas, apropiación social del conocimiento, formación de talento humano, etc.).
- Actividades clave y metodología general (qué se hará de manera resumida para lograr los objetivos, enfoque metodológico, fases del proyecto).
- Resultados esperados y productos principales (prototipos, publicaciones, políticas públicas, fortalecimiento institucional, etc.).
- Gobernanza y actores del proyecto (instituciones participantes, equipos, roles relevantes, alianzas, ecosistema de innovación) si la información está disponible.
- Consideraciones de presupuesto y recursos solo al nivel necesario para entender la magnitud del proyecto, sin entrar en tablas de costos detalladas.
- Cronograma general y fases del proyecto si está disponible.
- Impacto esperado y sostenibilidad del proyecto.

FORMATO DE RESPUESTA:
- Estructura la respuesta en VARIOS párrafos coherentes y detallados (mínimo 6-8 párrafos, idealmente más).
- Usa subtítulos en negrita para organizar la información (por ejemplo: **Justificación**, **Objetivos**, **Componentes principales**, **Resultados esperados**, etc.).
- Sé exhaustivo: incluye todos los detalles relevantes que encuentres en el contexto.
- Si un elemento importante no aparece en el contexto, menciona brevemente: "En los documentos proporcionados no se encontró información explícita sobre X".
- Proporciona ejemplos y detalles específicos cuando estén disponibles en el contexto.
"""
        elif "presupuesto" in q_lower or "costo" in q_lower or "presupuest" in q_lower:
            system_prompt = """
Eres un experto en análisis de presupuestos de proyectos de investigación e innovación.
Debes proporcionar respuestas COMPLETAS y DETALLADAS sobre presupuestos, costos y recursos financieros.

INSTRUCCIONES:
- Analiza TODA la información presupuestaria disponible en el contexto.
- Proporciona desgloses detallados por categorías, rubros y actividades.
- Incluye cantidades, costos unitarios, totales y justificaciones cuando estén disponibles.
- Estructura la información de manera clara y organizada.
- Si hay información sobre distribución temporal (por años, meses), inclúyela.
- Sé exhaustivo: no omitas detalles relevantes del presupuesto.
"""
        elif "actividad" in q_lower or "tarea" in q_lower or "metodología" in q_lower:
            system_prompt = """
Eres un experto en análisis de actividades y metodologías de proyectos.
Debes proporcionar respuestas COMPLETAS sobre las actividades, tareas y metodología del proyecto.

INSTRUCCIONES:
- Describe TODAS las actividades relevantes mencionadas en el contexto.
- Incluye detalles sobre metodología, cronograma, responsables y resultados esperados.
- Estructura la información de manera clara y organizada.
- Sé exhaustivo: incluye todos los detalles relevantes sobre actividades y metodología.
"""
        else:
            # Prompt general mejorado para respuestas más completas
            system_prompt = """
Eres un asistente experto en análisis de proyectos y presupuestos.
Tu tarea es responder preguntas de manera COMPLETA, DETALLADA y EXHAUSTIVA basándote en la información proporcionada en el contexto.

INSTRUCCIONES:
- Utiliza TODA la información relevante del contexto proporcionado.
- Proporciona respuestas COMPLETAS y BIEN ESTRUCTURADAS, no te limites a respuestas cortas.
- Si la pregunta tiene múltiples aspectos, aborda cada uno de manera exhaustiva.
- Incluye ejemplos, detalles específicos y explicaciones cuando sea apropiado.
- Estructura tu respuesta con párrafos claros y, si es necesario, con subtítulos o listas.
- Si la información no está disponible en el contexto, indica claramente que no tienes esa información.
- Responde siempre en español y de manera clara y profesional.
- Sé exhaustivo: no omitas información relevante que pueda ayudar a responder la pregunta completamente.
"""

        # Usar LLM si está disponible
        if self.use_llm and self.llm_service:
            try:
                return await self.llm_service.generate_answer(
                    question,
                    full_context,
                    system_prompt=system_prompt,
                )
            except Exception as e:
                # Si falla el LLM, usar método básico como fallback
                print(f"Error usando LLM, usando método básico: {str(e)}")
                return self._generate_basic_answer(question, full_context)
        else:
            # Método básico sin LLM
            return self._generate_basic_answer(question, full_context)
    
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
