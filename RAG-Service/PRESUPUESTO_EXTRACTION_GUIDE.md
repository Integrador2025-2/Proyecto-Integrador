# Guía de Extracción y Generación Inteligente de Presupuestos

## Descripción General

El servicio RAG ahora cuenta con capacidades avanzadas para:

1. **Extraer automáticamente** actividades y presupuestos desde archivos Excel y Word
2. **Mapear inteligentemente** las columnas a los rubros del sistema (TalentoHumano, EquiposSoftware, etc.)
3. **Completar valores faltantes** usando IA cuando el documento no tiene todos los costos
4. **Guardar en la base de datos** el presupuesto extraído con un solo llamado

## Características Principales

### 🎯 Extracción Inteligente

- **Detección automática de columnas**: El sistema identifica qué columnas corresponden a "Actividad", "Cantidad", "Valor Unitario", "Total", etc.
- **Clasificación de rubros**: Usa NLP y palabras clave para determinar si una actividad pertenece a TalentoHumano, EquiposSoftware, MaterialesInsumos, etc.
- **Soporte multi-formato**: Excel (.xlsx, .xls) y Word (.docx) con tablas
- **Manejo de múltiples hojas**: Procesa todas las hojas de un Excel automáticamente

### 🤖 Generación con IA

- **Completar presupuestos faltantes**: Si una actividad no tiene valores, el LLM genera estimaciones realistas
- **Contexto del proyecto**: Usa los documentos cargados previamente para generar presupuestos más precisos
- **Precios de mercado colombiano**: El LLM está entrenado para usar precios realistas en COP

## Endpoints Disponibles

### 1. Extraer Presupuesto desde Archivo

**POST** `/budget/extract-from-file`

Extrae actividades y presupuesto directamente desde un archivo Excel o Word.

**Parámetros:**
- `file` (multipart/form-data): Archivo Excel o Word
- `project_id` (optional): ID del proyecto

**Ejemplo con cURL:**

```bash
curl -X POST "http://localhost:8001/budget/extract-from-file" \
  -F "file=@Presupuesto_Proyecto.xlsx" \
  -F "project_id=1"
```

**Respuesta:**

```json
{
  "message": "Presupuesto extraído exitosamente",
  "filename": "Presupuesto_Proyecto.xlsx",
  "project_id": 1,
  "total_activities": 45,
  "rubros_found": [
    "TalentoHumano",
    "EquiposSoftware",
    "ServiciosTecnologicos",
    "MaterialesInsumos",
    "CapacitacionEventos",
    "GastosViaje"
  ],
  "grouped_by_rubro": {
    "TalentoHumano": 12,
    "EquiposSoftware": 8,
    "ServiciosTecnologicos": 10,
    "MaterialesInsumos": 7,
    "CapacitacionEventos": 5,
    "GastosViaje": 3
  },
  "budget_summary": {
    "total_budget": 250000000.0,
    "activities_with_budget": 40,
    "activities_without_budget": 5,
    "rubro_totals": {
      "TalentoHumano": 150000000.0,
      "EquiposSoftware": 45000000.0,
      "ServiciosTecnologicos": 30000000.0,
      "MaterialesInsumos": 15000000.0,
      "CapacitacionEventos": 7000000.0,
      "GastosViaje": 3000000.0
    },
    "needs_llm_generation": true
  },
  "activities": [
    {
      "nombre": "Coordinador de proyecto - tiempo completo",
      "rubro": "TalentoHumano",
      "cantidad": 12,
      "valor_unitario": 8000000,
      "total": 96000000,
      "justificacion": "Coordinación general del proyecto",
      "especificaciones_tecnicas": "Profesional con maestría en gerencia de proyectos",
      "periodo": 1,
      "source_sheet": "Presupuesto_Año1",
      "has_budget_values": true
    },
    // ... más actividades
  ],
  "extraction_confidence": 0.85
}
```

### 2. Generar Presupuesto (Mejorado)

**POST** `/budget/generate`

Genera presupuesto completo para un proyecto. Ahora con tres estrategias automáticas:

1. **Extracción desde documentos Excel/DOCX** (si están cargados)
2. **Generación con LLM** basada en actividades
3. **Análisis RAG** de documentos textuales

**Ejemplo:**

```json
{
  "project_id": 1,
  "project_description": "Desarrollo de plataforma de telemedicina cardiovascular",
  "budget_categories": [
    "TalentoHumano",
    "ServiciosTecnologicos",
    "EquiposSoftware",
    "MaterialesInsumos",
    "CapacitacionEventos",
    "GastosViaje"
  ],
  "duration_years": 2,
  "activities": []  // Opcional: si no se pasa, el sistema intenta extraer desde documentos
}
```

### 3. Guardar Presupuesto Extraído (Backend)

**POST** `/api/RAG/budget/save-extracted`

Guarda el presupuesto extraído en la base de datos del backend.

**Ejemplo:**

```json
{
  "projectId": 1,
  "actividadId": 5,
  "items": [
    {
      "nombre": "Desarrollador Senior - Backend",
      "rubro": "TalentoHumano",
      "cantidad": 12,
      "valorUnitario": 8000000,
      "total": 96000000,
      "justificacion": "Desarrollo de APIs y servicios",
      "especificacionesTecnicas": "5+ años experiencia .NET",
      "periodo": 1,
      "sourceSheet": "Presupuesto_2024",
      "hasBudgetValues": true
    },
    {
      "nombre": "Servidor de base de datos",
      "rubro": "EquiposSoftware",
      "cantidad": 2,
      "valorUnitario": 15000000,
      "total": 30000000,
      "justificacion": "Infraestructura de base de datos",
      "especificacionesTecnicas": "SQL Server Enterprise",
      "periodo": 1,
      "sourceSheet": "Presupuesto_2024",
      "hasBudgetValues": true
    }
  ],
  "extractionMethod": "intelligent_extraction"
}
```

**Respuesta:**

```json
{
  "success": true,
  "message": "Presupuesto guardado exitosamente",
  "itemsCreated": 45,
  "itemsPerRubro": {
    "TalentoHumano": 12,
    "EquiposSoftware": 8,
    "ServiciosTecnologicos": 10,
    "MaterialesInsumos": 7,
    "CapacitacionEventos": 5,
    "GastosViaje": 3
  },
  "errors": []
}
```

## Flujo de Trabajo Recomendado

### Opción 1: Extracción desde Excel con valores completos

1. **Subir documento Excel** con presupuesto completo
2. **Extraer presupuesto** usando `/budget/extract-from-file`
3. **Guardar en BD** usando `/api/RAG/budget/save-extracted`

```bash
# Paso 1: Extraer
curl -X POST "http://localhost:8001/budget/extract-from-file" \
  -F "file=@Presupuesto_Completo.xlsx" \
  -F "project_id=1" > extracted.json

# Paso 2: Guardar (desde tu frontend o script)
curl -X POST "http://localhost:5000/api/RAG/budget/save-extracted" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -d @extracted.json
```

### Opción 2: Extracción + Completar con IA

1. **Subir documento Excel** con actividades pero sin todos los valores
2. **Llamar a** `/budget/generate` - automáticamente:
   - Extrae lo que tiene valores
   - Completa con LLM lo que falta
3. **Guardar en BD**

### Opción 3: Solo subir documentos y generar todo

1. **Subir documentos** del proyecto (PDFs, Word, etc.) usando `/documents/upload`
2. **Llamar a** `/budget/generate` - el sistema:
   - Busca Excel/DOCX con presupuestos
   - Si no encuentra, usa las actividades de la BD
   - Como último recurso, analiza el contenido de los docs y genera con LLM

## Formato de Excel Esperado

El sistema es **flexible** y detecta automáticamente las columnas, pero funciona mejor con estos formatos:

### Formato Recomendado 1 (Detallado)

| Actividad | Rubro | Cantidad | Valor Unitario | Total | Justificación | Especificaciones Técnicas | Periodo |
|-----------|-------|----------|----------------|-------|---------------|---------------------------|---------|
| Coordinador de proyecto | Talento Humano | 12 | 8000000 | 96000000 | Coordinación | Profesional senior | 1 |
| Licencias Office 365 | Equipos Software | 20 | 150000 | 3000000 | Productividad | Microsoft 365 Business | 1 |

### Formato Recomendado 2 (Simplificado)

| Descripción | Cantidad | Costo Unitario | Costo Total |
|-------------|----------|----------------|-------------|
| Desarrollador Backend Senior | 6 | 8000000 | 48000000 |
| Servidor de aplicaciones | 2 | 12000000 | 24000000 |

El sistema **infiere el rubro** basándose en las palabras clave en la descripción.

### Formato Recomendado 3 (Múltiples Hojas por Rubro)

- **Hoja "Talento Humano"**: Solo actividades de talento humano
- **Hoja "Equipos y Software"**: Solo equipos
- **Hoja "Servicios Tecnológicos"**: Solo servicios
- etc.

## Mapeo Automático de Rubros

El sistema usa **palabras clave** para clasificar actividades:

| Rubro | Palabras Clave |
|-------|---------------|
| **TalentoHumano** | talento humano, recurso humano, personal, salario, honorario, empleado, profesional, investigador, coordinador |
| **ServiciosTecnologicos** | servicio, consultoría, asesoría, desarrollo, implementación, soporte técnico, mantenimiento |
| **EquiposSoftware** | equipo, software, licencia, hardware, computador, servidor, dispositivo, tecnología, aplicación |
| **MaterialesInsumos** | material, insumo, suministro, consumible, reactivo, papelería |
| **CapacitacionEventos** | capacitación, evento, taller, curso, formación, seminario, congreso |
| **GastosViaje** | viaje, transporte, desplazamiento, hospedaje, viático, pasaje, hotel |

## Configuración de Variables de Entorno

Para usar la generación con LLM, configura en `.env`:

```bash
# LLM Provider: "gemini" o "openai"
LLM_PROVIDER=gemini

# Si usas Gemini
GEMINI_API_KEY=tu_api_key_aqui
GEMINI_MODEL=gemini-1.5-flash-latest

# Si usas OpenAI
OPENAI_API_KEY=tu_api_key_aqui
OPENAI_MODEL=gpt-4o-mini

# Temperatura para generación (0.0 - 1.0)
LLM_TEMPERATURE=0.3
```

## Ejemplos de Uso

### Ejemplo Python (Extraer y Guardar)

```python
import requests

# 1. Extraer presupuesto
with open('Presupuesto_Proyecto.xlsx', 'rb') as f:
    files = {'file': f}
    data = {'project_id': 1}
    
    response = requests.post(
        'http://localhost:8001/budget/extract-from-file',
        files=files,
        data=data
    )
    
    extracted = response.json()

# 2. Guardar en BD
save_request = {
    "projectId": 1,
    "actividadId": 5,
    "items": extracted['activities'],
    "extractionMethod": extracted['extraction_method']
}

response = requests.post(
    'http://localhost:5000/api/RAG/budget/save-extracted',
    json=save_request,
    headers={'Authorization': f'Bearer {token}'}
)

print(response.json())
```

### Ejemplo JavaScript/TypeScript

```typescript
// Extraer presupuesto
const formData = new FormData();
formData.append('file', fileInput.files[0]);
formData.append('project_id', '1');

const extractResponse = await fetch('http://localhost:8001/budget/extract-from-file', {
  method: 'POST',
  body: formData
});

const extracted = await extractResponse.json();

// Guardar en BD
const saveResponse = await fetch('http://localhost:5000/api/RAG/budget/save-extracted', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${token}`
  },
  body: JSON.stringify({
    projectId: 1,
    actividadId: 5,
    items: extracted.activities,
    extractionMethod: extracted.extraction_method
  })
});

const result = await saveResponse.json();
console.log(`Creados ${result.itemsCreated} items de presupuesto`);
```

## Troubleshooting

### Problema: No detecta las columnas correctamente

**Solución:** Asegúrate de que la primera fila del Excel contenga los encabezados claros. Usa palabras como:
- "Actividad" o "Descripción" para la actividad
- "Cantidad" o "Cant" para cantidades
- "Valor Unitario" o "Costo Unitario" para el precio por unidad
- "Total" o "Valor Total" para el costo total

### Problema: Clasifica mal el rubro

**Solución:** Incluye palabras clave más específicas en el nombre de la actividad o agrega una columna "Rubro" explícita.

### Problema: Valores muy altos o muy bajos

**Solución:** Verifica que el Excel no tenga símbolos de moneda extraños o formatos numéricos incorrectos. El extractor limpia automáticamente puntos y comas, pero formatos muy raros pueden confundirlo.

### Problema: LLM genera presupuestos irreales

**Solución:** 
1. Asegúrate de tener documentos del proyecto cargados para dar más contexto
2. Ajusta `LLM_TEMPERATURE` a un valor más bajo (0.2 - 0.3) para respuestas más conservadoras
3. Proporciona una descripción más detallada del proyecto en `project_description`

## Mejoras Futuras

- [ ] Soporte para archivos CSV
- [ ] Detección de fórmulas en Excel para calcular totales
- [ ] Integración con APIs de precios de mercado en tiempo real
- [ ] Validación de presupuestos contra políticas de la organización
- [ ] Exportar presupuesto generado a formato estándar de MinCiencias

## Soporte

Para reportar problemas o sugerir mejoras, contacta al equipo de desarrollo.

