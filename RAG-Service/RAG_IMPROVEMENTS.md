# Mejoras del Sistema RAG

Este documento describe las mejoras implementadas para generar respuestas más largas, completas y efectivas utilizando mayor cantidad de información.

## Resumen de Mejoras

### 1. Modelo Mejorado
- **Antes**: Gemini 1.5 Flash (rápido pero limitado para respuestas largas)
- **Ahora**: Gemini 1.5 Pro (optimizado para respuestas exhaustivas y detalladas)
- **Beneficio**: Mayor capacidad de generación de texto largo y más preciso

### 2. Tokens de Salida Aumentados
- **Antes**: 2,000 tokens máximo para respuestas normales
- **Ahora**: 8,192 tokens máximo (configurable)
- **Beneficio**: Respuestas mucho más largas y completas

### 3. Recuperación de Contexto Mejorada
- **Antes**: top_k = 5 documentos por defecto
- **Ahora**: top_k = 10 documentos por defecto (hasta 20)
- **Beneficio**: Más información disponible para generar respuestas

### 4. Prompts del Sistema Optimizados
- **Antes**: Prompts básicos y genéricos
- **Ahora**: Prompts especializados y detallados según el tipo de pregunta
- **Beneficio**: Respuestas más relevantes y estructuradas

### 5. Contexto Adicional del Proyecto
- **Nuevo**: Obtención automática de contexto adicional del proyecto cuando está disponible
- **Beneficio**: Respuestas más contextualizadas y completas

## Configuración

### Variables de Entorno

Agrega estas variables a tu archivo `.env`:

```env
# Modelo (recomendado: gemini-1.5-pro-latest para respuestas largas)
GEMINI_MODEL=gemini-1.5-pro-latest

# Tokens máximos de salida
GEMINI_MAX_OUTPUT_TOKENS=8192          # Para respuestas generales
GEMINI_MAX_OUTPUT_TOKENS_BUDGET=8192   # Para generación de presupuestos
GEMINI_MAX_OUTPUT_TOKENS_PLAN=8192    # Para planes de recursos
```

### Parámetros de Consulta

El parámetro `top_k` ahora tiene un valor por defecto de 10 (antes era 5):

```python
# Ejemplo de consulta
response = await rag_service.query(
    question="¿Cuál es el resumen del proyecto?",
    project_id=1,
    top_k=15  # Puedes aumentar hasta 20 para más contexto
)
```

## Tipos de Respuestas Mejoradas

### 1. Resúmenes Ejecutivos
- **Antes**: 2-3 párrafos
- **Ahora**: 6-8+ párrafos estructurados con subtítulos
- **Incluye**: Justificación, objetivos, alcance, población, componentes, actividades, resultados, gobernanza, impacto

### 2. Análisis de Presupuestos
- **Antes**: Información básica
- **Ahora**: Desgloses detallados por categorías, rubros, actividades con cantidades, costos unitarios, totales y justificaciones

### 3. Actividades y Metodología
- **Antes**: Lista básica
- **Ahora**: Descripciones completas con metodología, cronograma, responsables y resultados esperados

### 4. Respuestas Generales
- **Antes**: Respuestas cortas
- **Ahora**: Respuestas exhaustivas que utilizan toda la información relevante disponible

## Características Técnicas

### Parámetros de Generación Mejorados

```python
generation_config=genai.types.GenerationConfig(
    temperature=0.3,          # Creatividad controlada
    max_output_tokens=8192,   # Respuestas muy largas
    top_p=0.95,              # Nucleus sampling para mejor calidad
    top_k=40                 # Diversidad en la generación
)
```

### Filtrado de Similitud

- **Umbral mínimo**: 0.3 de similitud
- **Fallback**: Si no hay documentos con similitud suficiente, se usan los 3 más similares
- **Beneficio**: Mejor calidad de contexto recuperado

### Organización de Contexto

- Los documentos se organizan y numeran para mejor comprensión
- Contexto adicional del proyecto se agrega automáticamente cuando está disponible
- Mejor estructuración para el modelo LLM

## Ejemplos de Uso

### Consulta Básica con Más Contexto

```python
# El sistema ahora recupera automáticamente más documentos
response = await rag_service.query(
    question="Explícame el proyecto completo",
    project_id=1
    # top_k=10 por defecto (antes era 5)
)
```

### Consulta con Contexto Máximo

```python
# Para respuestas extremadamente detalladas
response = await rag_service.query(
    question="Necesito un análisis completo del presupuesto",
    project_id=1,
    top_k=20  # Máximo permitido
)
```

## Rendimiento

### Tiempos de Respuesta
- **Gemini 1.5 Pro**: ~2-5 segundos para respuestas largas
- **Gemini 1.5 Flash**: ~1-3 segundos (más rápido pero menos detallado)

### Límites de Tokens
- **Entrada**: Hasta 1,000,000 tokens (contexto muy largo)
- **Salida**: Hasta 8,192 tokens (respuestas muy largas)

## Recomendaciones

1. **Para respuestas largas y detalladas**: Usa `gemini-1.5-pro-latest`
2. **Para respuestas rápidas**: Usa `gemini-1.5-flash-latest`
3. **Para máximo contexto**: Aumenta `top_k` a 15-20
4. **Para proyectos grandes**: El sistema maneja automáticamente el contexto adicional

## Troubleshooting

### Respuestas muy cortas
- Verifica que `GEMINI_MAX_OUTPUT_TOKENS` esté configurado en 8192
- Aumenta `top_k` en la consulta
- Verifica que haya suficientes documentos cargados

### Respuestas genéricas
- Asegúrate de que los documentos estén correctamente cargados
- Verifica que `project_id` esté especificado para obtener contexto adicional
- Aumenta `top_k` para recuperar más documentos

### Errores de tokens
- Si el contexto es muy largo, el sistema automáticamente limita el número de documentos
- Considera usar `gemini-1.5-pro-latest` que maneja mejor contextos largos

## Próximas Mejoras

- [ ] Soporte para Claude (Anthropic) como alternativa
- [ ] Reranking de documentos para mejor relevancia
- [ ] Chunking inteligente basado en estructura del documento
- [ ] Caché de respuestas para consultas similares
- [ ] Streaming de respuestas para mejor UX

