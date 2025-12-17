import os
import json
import logging
from typing import List, Dict, Any, Optional
from openai import OpenAI
import google.generativeai as genai
from google.generativeai.types import HarmCategory, HarmBlockThreshold
from dotenv import load_dotenv

load_dotenv()

logger = logging.getLogger(__name__)

class LLMService:
    """Servicio para integración con modelos de lenguaje (OpenAI o Google Gemini)"""
    
    def __init__(self):
        # Determinar qué proveedor usar
        self.provider = os.getenv("LLM_PROVIDER", "gemini").lower()  # Por defecto Gemini
        
        if self.provider == "openai":
            self._init_openai()
        elif self.provider == "gemini":
            self._init_gemini()
        else:
            raise ValueError(f"Proveedor LLM no soportado: {self.provider}. Use 'openai' o 'gemini'")
        
        self.temperature = float(os.getenv("LLM_TEMPERATURE", "0.3"))
    
    def _init_openai(self):
        """Inicializar OpenAI"""
        api_key = os.getenv("OPENAI_API_KEY")
        if not api_key:
            raise ValueError("OPENAI_API_KEY no está configurada")
        
        self.client = OpenAI(api_key=api_key)
        self.model = os.getenv("OPENAI_MODEL", "gpt-4o-mini")
    
    def _init_gemini(self):
        """Inicializar Google Gemini"""
        api_key = os.getenv("GEMINI_API_KEY")
        if not api_key:
            raise ValueError("GEMINI_API_KEY no está configurada en las variables de entorno")
        
        genai.configure(api_key=api_key)
        # Gemini 1.5 Pro es mejor para respuestas más largas y detalladas
        # Alternativas: "gemini-1.5-flash-latest" (más rápido), "gemini-2.0-flash-exp" (experimental)
        self.model_name = os.getenv("GEMINI_MODEL", "gemini-1.5-pro-latest")
        self.model = genai.GenerativeModel(self.model_name)
    
    async def generate_answer(self, question: str, context: str, system_prompt: Optional[str] = None) -> str:
        """
        Generar respuesta usando LLM basado en contexto
        
        Args:
            question: Pregunta del usuario
            context: Contexto relevante de los documentos
            system_prompt: Prompt del sistema (opcional)
        
        Returns:
            Respuesta generada por el LLM
        """
        try:
            default_system_prompt = """Eres un asistente experto en análisis de proyectos y presupuestos. 
            Tu tarea es responder preguntas basándote en la información proporcionada en el contexto.
            Si la información no está disponible en el contexto, indica claramente que no tienes esa información.
            Responde siempre en español y de manera clara y profesional."""
            
            system_prompt = system_prompt or default_system_prompt
            
            if self.provider == "openai":
                return await self._generate_answer_openai(question, context, system_prompt)
            else:  # gemini
                return await self._generate_answer_gemini(question, context, system_prompt)
            
        except Exception as e:
            raise Exception(f"Error generando respuesta con LLM: {str(e)}")
    
    async def _generate_answer_openai(self, question: str, context: str, system_prompt: str) -> str:
        """Generar respuesta usando OpenAI"""
        messages = [
            {"role": "system", "content": system_prompt},
            {"role": "user", "content": f"Contexto:\n{context}\n\nPregunta: {question}\n\nRespuesta:"}
        ]
        
        response = self.client.chat.completions.create(
            model=self.model,
            messages=messages,
            temperature=self.temperature,
            max_tokens=1000
        )
        
        return response.choices[0].message.content.strip()
    
    async def _generate_answer_gemini(self, question: str, context: str, system_prompt: str) -> str:
        """Generar respuesta usando Google Gemini con soporte para respuestas más largas"""
        prompt = f"""{system_prompt}

Contexto:
{context}

Pregunta: {question}

Instrucciones adicionales:
- Proporciona una respuesta completa, detallada y bien estructurada
- Utiliza toda la información relevante del contexto proporcionado
- Si hay múltiples aspectos en la pregunta, aborda cada uno de manera exhaustiva
- Incluye ejemplos, detalles y explicaciones cuando sea apropiado
- Estructura tu respuesta con párrafos claros y, si es necesario, con subtítulos o listas
- No te limites a respuestas cortas; sé exhaustivo y completo

Respuesta:"""
        
        try:
            # Aumentar significativamente max_output_tokens para respuestas más largas
            # Gemini 1.5 Pro puede manejar hasta 8192 tokens de salida
            max_tokens = int(os.getenv("GEMINI_MAX_OUTPUT_TOKENS", "8192"))
            
            response = self.model.generate_content(
                prompt,
                generation_config=genai.types.GenerationConfig(
                    temperature=self.temperature,
                    max_output_tokens=max_tokens,
                    top_p=0.95,  # Nucleus sampling para mejor calidad
                    top_k=40  # Diversidad en la generación
                ),
                safety_settings={
                    HarmCategory.HARM_CATEGORY_HARASSMENT: HarmBlockThreshold.BLOCK_NONE,
                    HarmCategory.HARM_CATEGORY_HATE_SPEECH: HarmBlockThreshold.BLOCK_NONE,
                    HarmCategory.HARM_CATEGORY_SEXUALLY_EXPLICIT: HarmBlockThreshold.BLOCK_NONE,
                    HarmCategory.HARM_CATEGORY_DANGEROUS_CONTENT: HarmBlockThreshold.BLOCK_NONE,
                }
            )
            
            # Verificar si la respuesta tiene contenido válido
            if not response.parts:
                logger.warning(f"Gemini no generó respuesta. Finish reason: {response.candidates[0].finish_reason if response.candidates else 'unknown'}")
                return "Lo siento, no pude generar una respuesta. Por favor, intenta reformular tu pregunta."
            
            return response.text.strip()
            
        except Exception as e:
            logger.error(f"Error generando respuesta con Gemini: {str(e)}")
            return "Error al generar respuesta. Por favor, intenta de nuevo."
    
    async def generate_budget_from_activities(
        self,
        project_description: str,
        activities: List[Dict[str, Any]],
        project_documents_context: Optional[str] = None,
        duration_years: int = 1,
        budget_categories: Optional[List[str]] = None
    ) -> Dict[str, Any]:
        """
        Generar presupuesto inteligente basado en actividades usando LLM
        """
        try:
            # Preparar información de actividades para el prompt
            activities_text = self._format_activities_for_prompt(activities)
            
            # Preparar contexto de documentos si está disponible
            documents_context = ""
            if project_documents_context:
                documents_context = f"\n\nInformación adicional de documentos del proyecto:\n{project_documents_context[:2000]}"
            
            # Categorías por defecto
            if not budget_categories:
                budget_categories = [
                    "TalentoHumano",
                    "ServiciosTecnologicos",
                    "EquiposSoftware",
                    "MaterialesInsumos",
                    "CapacitacionEventos",
                    "GastosViaje"
                ]
            
            system_prompt = """Eres un experto en presupuestos de proyectos de investigación e innovación en Colombia.
Tu tarea es generar presupuestos realistas y detallados basados en las actividades del proyecto.

IMPORTANTE:
- Todos los precios deben estar en PESOS COLOMBIANOS (COP)
- Los precios deben ser realistas para el mercado colombiano (2024-2025)
- Considera el tipo de proyecto, su complejidad y duración
- Genera items detallados y justificados para cada categoría
- Distribuye los costos según la duración del proyecto
- Incluye cantidades, costos unitarios y totales

FORMATO DE RESPUESTA:
Debes responder ÚNICAMENTE con un objeto JSON válido. NO incluyas texto adicional antes o después del JSON.
En los campos de texto, evita usar comillas dobles, usa comillas simples si es necesario.
Mantén las descripciones y justificaciones breves y en una sola línea.

Ejemplo de formato:
{
    "categories": {
        "TalentoHumano": {
            "items": [
                {
                    "description": "Investigador senior - 6 meses",
                    "quantity": 1,
                    "unit_cost": 5000000,
                    "total_cost": 5000000,
                    "justification": "Liderazgo técnico del proyecto",
                    "year": 1
                }
            ],
            "subtotal": 5000000
        }
    },
    "total_budget": 0,
    "reasoning": "Presupuesto basado en actividades y precios de mercado colombiano"
}"""
            
            user_prompt = f"""Genera un presupuesto detallado para el siguiente proyecto:

DESCRIPCIÓN DEL PROYECTO:
{project_description}

DURACIÓN: {duration_years} año(s)

ACTIVIDADES DEL PROYECTO:
{activities_text}

CATEGORÍAS A CONSIDERAR: {', '.join(budget_categories)}
{documents_context}

Genera un presupuesto realista considerando:
1. El tipo y complejidad de cada actividad
2. Los recursos necesarios para ejecutar cada actividad
3. Precios de mercado colombiano actuales
4. La duración del proyecto
5. Costos de personal, equipos, servicios, materiales, capacitación y viajes según corresponda

Responde SOLO con el JSON, sin texto adicional."""
            
            if self.provider == "openai":
                budget_json = await self._generate_budget_openai(system_prompt, user_prompt)
            else:  # gemini
                budget_json = await self._generate_budget_gemini(system_prompt, user_prompt)
            
            # Validar y estructurar la respuesta
            return self._validate_and_structure_budget(budget_json, budget_categories, duration_years)
            
        except json.JSONDecodeError as e:
            raise Exception(f"Error parseando respuesta JSON del LLM: {str(e)}")
        except Exception as e:
            raise Exception(f"Error generando presupuesto con LLM: {str(e)}")
    
    async def _generate_budget_openai(self, system_prompt: str, user_prompt: str) -> Dict[str, Any]:
        """Generar presupuesto usando OpenAI"""
        messages = [
            {"role": "system", "content": system_prompt},
            {"role": "user", "content": user_prompt}
        ]
        
        response = self.client.chat.completions.create(
            model=self.model,
            messages=messages,
            temperature=self.temperature,
            max_tokens=4000,
            response_format={"type": "json_object"}
        )
        
        return json.loads(response.choices[0].message.content.strip())
    
    async def _generate_budget_gemini(self, system_prompt: str, user_prompt: str) -> Dict[str, Any]:
        """Generar presupuesto usando Google Gemini con soporte para respuestas más largas"""
        full_prompt = f"""{system_prompt}

{user_prompt}"""
        
        try:
            # Aumentar max_output_tokens para presupuestos más detallados
            max_tokens = int(os.getenv("GEMINI_MAX_OUTPUT_TOKENS_BUDGET", "8192"))
            
            response = self.model.generate_content(
                full_prompt,
                generation_config=genai.types.GenerationConfig(
                    temperature=self.temperature,
                    max_output_tokens=max_tokens,  # Máximo para respuestas detalladas
                    top_p=0.95,
                    top_k=40,
                    response_mime_type="application/json"  # Forzar respuesta JSON
                ),
                safety_settings={
                    HarmCategory.HARM_CATEGORY_HARASSMENT: HarmBlockThreshold.BLOCK_NONE,
                    HarmCategory.HARM_CATEGORY_HATE_SPEECH: HarmBlockThreshold.BLOCK_NONE,
                    HarmCategory.HARM_CATEGORY_SEXUALLY_EXPLICIT: HarmBlockThreshold.BLOCK_NONE,
                    HarmCategory.HARM_CATEGORY_DANGEROUS_CONTENT: HarmBlockThreshold.BLOCK_NONE,
                }
            )
            
            # Verificar si la respuesta tiene contenido válido
            if not response.parts:
                logger.error(f"Gemini no generó contenido. Finish reason: {response.candidates[0].finish_reason if response.candidates else 'unknown'}")
                logger.error(f"Safety ratings: {response.candidates[0].safety_ratings if response.candidates else 'N/A'}")
                raise Exception(
                    f"Gemini no generó contenido válido. "
                    f"Finish reason: {response.candidates[0].finish_reason if response.candidates else 'unknown'}. "
                    f"Esto puede deberse a límites de tokens o filtros de seguridad."
                )
            
            # Log de la respuesta para debugging
            logger.debug(f"Gemini raw response (first 500 chars): {response.text[:500]}")
            
            # Limpiar y parsear la respuesta JSON
            return self._parse_json_response(response.text)
            
        except Exception as e:
            if "finish_reason" in str(e) or "response.text" in str(e):
                # Si el error es por finish_reason, dar más contexto
                logger.error(f"Error con respuesta de Gemini: {str(e)}")
                raise Exception(
                    "Gemini no pudo generar el presupuesto. "
                    "Esto puede ocurrir si hay demasiadas actividades o el contexto es muy largo. "
                    "Intenta reducir el número de actividades o simplificar las descripciones."
                )
            logger.error(f"Error parsing Gemini response: {str(e)}")
            raise
    
    def _format_activities_for_prompt(self, activities: List[Dict[str, Any]]) -> str:
        """Formatear actividades para el prompt del LLM"""
        if not activities:
            return "No se proporcionaron actividades específicas."
        
        # Limitar el número de actividades para evitar exceder tokens
        max_activities = 15  # Ajusta según sea necesario
        activities_to_process = activities[:max_activities]
        
        if len(activities) > max_activities:
            logger.warning(f"Se truncaron {len(activities) - max_activities} actividades para evitar exceder el límite de tokens")
        
        formatted = []
        for i, activity in enumerate(activities_to_process, 1):
            activity_text = f"ACTIVIDAD {i}:\n"
            activity_text += f"- Nombre: {activity.get('nombre', 'N/A')[:200]}\n"  # Limitar longitud
            
            # Truncar descripción si es muy larga
            descripcion = activity.get('descripcion', 'N/A')
            if len(descripcion) > 300:
                descripcion = descripcion[:297] + "..."
            activity_text += f"- Descripción: {descripcion}\n"
            
            # Incluir justificación solo si no es muy larga
            justificacion = activity.get('justificacion', '')
            if justificacion and len(justificacion) <= 200:
                activity_text += f"- Justificación: {justificacion}\n"
            
            # Incluir especificaciones técnicas solo si no es muy larga
            especificaciones = activity.get('especificaciones_tecnicas', '')
            if especificaciones and len(especificaciones) <= 200:
                activity_text += f"- Especificaciones: {especificaciones}\n"
            
            if activity.get('cantidad_anios'):
                activity_text += f"- Duración: {activity.get('cantidad_anios')} año(s)\n"
            
            if activity.get('valor_unitario'):
                activity_text += f"- Valor Estimado: ${activity.get('valor_unitario'):,.0f} COP\n"
            
            formatted.append(activity_text)
        
        result = "\n\n".join(formatted)
        
        # Agregar nota si se truncaron actividades
        if len(activities) > max_activities:
            result += f"\n\n[Nota: Se muestran {max_activities} de {len(activities)} actividades totales]"
        
        return result
    
    def _validate_and_structure_budget(
        self,
        budget_json: Dict[str, Any],
        budget_categories: List[str],
        duration_years: int
    ) -> Dict[str, Any]:
        """Validar y estructurar el presupuesto generado por el LLM"""
        from datetime import datetime
        
        structured_budget = {
            "categories": {},
            "total_budget": 0.0,
            "confidence_score": 0.8,
            "generated_at": datetime.now().isoformat(),
            "reasoning": budget_json.get("reasoning", "Presupuesto generado por IA basado en actividades del proyecto")
        }
        
        # Procesar cada categoría
        categories_data = budget_json.get("categories", {})
        
        for category in budget_categories:
            if category in categories_data:
                category_data = categories_data[category]
                items = category_data.get("items", [])
                
                # Validar y limpiar items
                validated_items = []
                for item in items:
                    validated_item = {
                        "description": str(item.get("description", "")),
                        "quantity": float(item.get("quantity", 1)),
                        "unit_cost": float(item.get("unit_cost", 0)),
                        "total_cost": float(item.get("total_cost", item.get("unit_cost", 0) * item.get("quantity", 1))),
                        "justification": str(item.get("justification", "")),
                        "year": int(item.get("year", 1))
                    }
                    validated_items.append(validated_item)
                
                subtotal = category_data.get("subtotal", sum(item["total_cost"] for item in validated_items))
                
                structured_budget["categories"][category] = {
                    "items": validated_items,
                    "subtotal": float(subtotal),
                    "confidence": 0.8
                }
                
                structured_budget["total_budget"] += float(subtotal)
            else:
                # Si la categoría no está en la respuesta, crear estructura vacía
                structured_budget["categories"][category] = {
                    "items": [],
                    "subtotal": 0.0,
                    "confidence": 0.3
                }
        
        # Asegurar que el total_budget sea correcto
        if "total_budget" in budget_json:
            structured_budget["total_budget"] = float(budget_json.get("total_budget", structured_budget["total_budget"]))
        
        return structured_budget
    
    async def generate_resource_plan(
        self,
        activities: List[Dict[str, Any]],
        resources: List[Dict[str, Any]],
        project_context: Optional[str] = None,
        max_budget: Optional[float] = None
    ) -> Dict[str, Any]:
        """Generar plan de asignación de recursos usando LLM"""
        try:
            activities_text = self._format_activities_for_prompt(activities)
            resources_text = self._format_resources_for_prompt(resources)
            
            system_prompt = """Eres un experto en planificación de recursos para proyectos.
Tu tarea es asignar recursos de manera óptima a las actividades del proyecto.

Responde ÚNICAMENTE con un JSON válido en el siguiente formato:
{
    "assignments": [
        {
            "actividad_id": 1,
            "actividad_nombre": "Nombre",
            "recurso_id": 1,
            "recurso_nombre": "Nombre",
            "recurso_tipo": "TalentoHumano",
            "cantidad": 10.0,
            "costo_estimado": 5000000,
            "justificacion": "Justificación"
        }
    ],
    "summary": "Resumen del plan",
    "criteria": ["Criterio 1", "Criterio 2"],
    "confidence": 0.8
}"""
            
            budget_constraint = ""
            if max_budget:
                budget_constraint = f"\n\nPRESUPUESTO MÁXIMO: ${max_budget:,.0f} COP\nAsegúrate de que el costo total no exceda este presupuesto."
            
            user_prompt = f"""Genera un plan de asignación de recursos para las siguientes actividades:

ACTIVIDADES:
{activities_text}

RECURSOS DISPONIBLES:
{resources_text}
{budget_constraint}
{project_context if project_context else ""}

Asigna recursos de manera óptima considerando:
1. Compatibilidad entre recursos y actividades
2. Disponibilidad de recursos
3. Costos y presupuesto disponible
4. Eficiencia en el uso de recursos

Responde SOLO con el JSON, sin texto adicional."""
            
            if self.provider == "openai":
                plan_json = await self._generate_resource_plan_openai(system_prompt, user_prompt)
            else:  # gemini
                plan_json = await self._generate_resource_plan_gemini(system_prompt, user_prompt)
            
            return {
                "assignments": plan_json.get("assignments", []),
                "summary": plan_json.get("summary", ""),
                "criteria": plan_json.get("criteria", []),
                "confidence": float(plan_json.get("confidence", 0.7))
            }
            
        except Exception as e:
            raise Exception(f"Error generando plan de recursos con LLM: {str(e)}")
    
    async def _generate_resource_plan_openai(self, system_prompt: str, user_prompt: str) -> Dict[str, Any]:
        """Generar plan de recursos usando OpenAI"""
        messages = [
            {"role": "system", "content": system_prompt},
            {"role": "user", "content": user_prompt}
        ]
        
        response = self.client.chat.completions.create(
            model=self.model,
            messages=messages,
            temperature=self.temperature,
            max_tokens=3000,
            response_format={"type": "json_object"}
        )
        
        return json.loads(response.choices[0].message.content.strip())
    
    async def _generate_resource_plan_gemini(self, system_prompt: str, user_prompt: str) -> Dict[str, Any]:
        """Generar plan de recursos usando Google Gemini con soporte para respuestas más largas"""
        full_prompt = f"""{system_prompt}

{user_prompt}"""
        
        try:
            # Aumentar max_output_tokens para planes más detallados
            max_tokens = int(os.getenv("GEMINI_MAX_OUTPUT_TOKENS_PLAN", "8192"))
            
            response = self.model.generate_content(
                full_prompt,
                generation_config=genai.types.GenerationConfig(
                    temperature=self.temperature,
                    max_output_tokens=max_tokens,
                    top_p=0.95,
                    top_k=40,
                    response_mime_type="application/json"
                ),
                safety_settings={
                    HarmCategory.HARM_CATEGORY_HARASSMENT: HarmBlockThreshold.BLOCK_NONE,
                    HarmCategory.HARM_CATEGORY_HATE_SPEECH: HarmBlockThreshold.BLOCK_NONE,
                    HarmCategory.HARM_CATEGORY_SEXUALLY_EXPLICIT: HarmBlockThreshold.BLOCK_NONE,
                    HarmCategory.HARM_CATEGORY_DANGEROUS_CONTENT: HarmBlockThreshold.BLOCK_NONE,
                }
            )
            
            # Verificar si la respuesta tiene contenido válido
            if not response.parts:
                logger.error(f"Gemini no generó contenido para plan de recursos. Finish reason: {response.candidates[0].finish_reason if response.candidates else 'unknown'}")
                raise Exception("Gemini no pudo generar el plan de recursos. Intenta con menos actividades.")
            
            # Limpiar y parsear la respuesta JSON
            return self._parse_json_response(response.text)
            
        except Exception as e:
            if "finish_reason" in str(e) or "response.text" in str(e):
                raise Exception("Gemini no pudo generar el plan. Reduce el número de actividades o recursos.")
            raise
    
    def _format_resources_for_prompt(self, resources: List[Dict[str, Any]]) -> str:
        """Formatear recursos para el prompt del LLM"""
        if not resources:
            return "No se proporcionaron recursos específicos."
        
        formatted = []
        for i, resource in enumerate(resources, 1):
            resource_text = f"RECURSO {i}:\n"
            resource_text += f"- Nombre: {resource.get('nombre', 'N/A')}\n"
            resource_text += f"- Tipo: {resource.get('tipo', 'N/A')}\n"
            
            if resource.get('costo_unitario'):
                resource_text += f"- Costo Unitario: ${resource.get('costo_unitario'):,.0f} COP\n"
            
            if resource.get('unidad'):
                resource_text += f"- Unidad: {resource.get('unidad')}\n"
            
            if resource.get('disponibilidad'):
                resource_text += f"- Disponibilidad: {resource.get('disponibilidad')}\n"
            
            formatted.append(resource_text)
        
        return "\n\n".join(formatted)
    
    def _parse_json_response(self, response_text: str) -> Dict[str, Any]:
        """
        Parsear respuesta JSON de manera robusta, limpiando posibles errores de formato
        """
        import re
        
        # Limpiar la respuesta
        cleaned = response_text.strip()
        
        # Si viene envuelto en markdown, extraer el JSON
        if "```json" in cleaned:
            match = re.search(r'```json\s*(\{.*?\})\s*```', cleaned, re.DOTALL)
            if match:
                cleaned = match.group(1)
        elif "```" in cleaned:
            match = re.search(r'```\s*(\{.*?\})\s*```', cleaned, re.DOTALL)
            if match:
                cleaned = match.group(1)
        
        # Intentar parsear directamente
        try:
            return json.loads(cleaned)
        except json.JSONDecodeError as e:
            logger.warning(f"Primera tentativa de parsing falló: {str(e)}")
            
            # Si falla, intentar encontrar el JSON en el texto usando un patrón más agresivo
            # Buscar el JSON desde el primer { hasta el último }
            json_match = re.search(r'\{(?:[^{}]|(?:\{[^{}]*\}))*\}', cleaned, re.DOTALL)
            if json_match:
                try:
                    return json.loads(json_match.group(0))
                except json.JSONDecodeError:
                    logger.warning("Segunda tentativa de parsing falló")
            
            # Intentar limpiar strings problemáticos
            try:
                # Reemplazar saltos de línea dentro de strings
                cleaned_lines = cleaned.replace('\n', ' ').replace('\r', '')
                return json.loads(cleaned_lines)
            except json.JSONDecodeError:
                logger.warning("Tercera tentativa de parsing falló")
            
            # Si todo falla, registrar el error con más detalles
            error_context = cleaned[max(0, e.pos - 150):min(len(cleaned), e.pos + 150)]
            logger.error(f"Todas las tentativas fallaron. Contexto del error: {error_context}")
            raise json.JSONDecodeError(
                f"No se pudo parsear la respuesta JSON. Posición del error: {e.pos}. Contexto: ...{error_context}...",
                e.doc,
                e.pos
            )

    async def extract_activities_from_documents(self, context: str, project_id: int) -> Dict[str, Any]:
        """
        Extrae todas las actividades mencionadas en los documentos del proyecto.
        """
        system_prompt = """Eres un experto en análisis de proyectos.
Tu tarea es extraer TODAS las actividades mencionadas en los documentos del proyecto.

IMPORTANTE:
- Extrae cada actividad que encuentres en el texto.
- Para cada actividad, captura: nombre, descripción, justificación (si existe), especificaciones técnicas (si existen).
- Si se menciona duración o valores monetarios, inclúyelos.
- NO inventes información que no esté en el texto.
- NO te preocupes por objetivos, cadenas de valor o jerarquías complejas.
- SOLO enfócate en listar las actividades encontradas.

FORMATO DE RESPUESTA:
Debes responder ÚNICAMENTE con un objeto JSON válido:
{
    "project_id": 123,
    "activities": [
        {
            "name": "Nombre de la actividad",
            "description": "Descripción de la actividad",
            "justification": "Justificación si existe en el texto o null",
            "technical_specifications": "Especificaciones técnicas si existen o null",
            "duration_years": 1,
            "unit_value": 0
        }
    ],
    "total_activities": 0
}
"""
        user_prompt = f"Extrae todas las actividades del proyecto (ID: {project_id}) basándote en el siguiente contexto:\n\n{context}"
        
        try:
            if self.provider == "openai":
                response_text = await self._generate_answer_openai(user_prompt, context, system_prompt)
            else:
                response_text = await self._generate_answer_gemini(user_prompt, context, system_prompt)
            
            result = self._parse_json_response(response_text)
            # Asegurarse de que total_activities esté actualizado
            if "activities" in result:
                result["total_activities"] = len(result["activities"])
            return result
            
        except Exception as e:
            logger.error(f"Error extrayendo actividades: {str(e)}")
            return {"project_id": project_id, "activities": [], "total_activities": 0}
