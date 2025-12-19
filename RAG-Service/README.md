# RAG Budget Automation Service 🚀

Servicio RAG (Retrieval-Augmented Generation) para automatización inteligente de presupuestos de proyectos de CTeI.

## 🐳 Inicio Rápido con Docker (Recomendado)

**¿Quieres ejecutar el servicio sin instalar dependencias?** Usa Docker:

```bash
# 1. Configurar variables de entorno
cp .env.docker .env
# Edita .env y agrega tu GEMINI_API_KEY

# 2. Iniciar servicio
docker-compose up -d

# 3. Verificar que está funcionando
curl http://localhost:8001/health

# 4. Ver logs
docker-compose logs -f
```

El servicio estará disponible en **http://localhost:8001**

📖 **Documentación:**
- API Docs: http://localhost:8001/docs
- Guía Docker: [DOCKER_README.md](DOCKER_README.md)
- Ejemplos: [API_EXAMPLES.md](API_EXAMPLES.md)

---

## ✨ Características Principales

### Gestión de Documentos
- ✅ Procesamiento de documentos (PDF, DOCX, TXT, XLSX)
- ✅ Búsqueda semántica con embeddings multilingües
- ✅ Almacenamiento vectorial con ChromaDB
- ✅ Extracción automática de metadatos

### Generación de Presupuestos
- ✅ **Extracción inteligente** desde Excel/Word con detección automática de columnas
- ✅ **Mapeo automático** a rubros del sistema (TalentoHumano, EquiposSoftware, etc.)
- ✅ **Completar valores faltantes** usando LLM (Gemini o OpenAI)
- ✅ **Generación de presupuestos** basada en actividades y contexto del proyecto
- ✅ Exportación a Excel con formato profesional

### Inteligencia Artificial
- ✅ Resúmenes ejecutivos de proyectos con contexto completo
- ✅ Respuestas contextuales a preguntas sobre documentos
- ✅ Estimación de costos basada en precios del mercado colombiano
- ✅ Planificación de recursos con justificación automática

## 📦 Instalación

```bash
# Crear entorno virtual
python -m venv venv

# Activar entorno virtual
# En Windows:
venv\Scripts\activate
# En Linux/Mac:
source venv/bin/activate

# Instalar dependencias
pip install -r requirements.txt
```

## ⚙️ Configuración

Crear archivo `.env` basado en `env.example`:

```bash
# Proveedor de LLM: "gemini" o "openai"
LLM_PROVIDER=gemini

# Gemini (Google AI)
GEMINI_API_KEY=tu_api_key_aqui
GEMINI_MODEL=gemini-1.5-flash-latest

# OpenAI (alternativa)
# OPENAI_API_KEY=tu_api_key_aqui
# OPENAI_MODEL=gpt-4o-mini

# Temperatura para generación (0.0 - 1.0)
LLM_TEMPERATURE=0.3
```

📖 **Consulta** `GEMINI_SETUP.md` para obtener tu API key de Google Gemini.

## 🏃 Ejecutar el Servicio

### Opción 1: Con Docker (Recomendado) 🐳

```bash
# Usar script de ayuda (Windows)
.\docker-manage.ps1 start

# O usar script de ayuda (Linux/Mac)
./docker-manage.sh start

# O directamente con docker-compose
docker-compose up -d
```

### Opción 2: Instalación Local

```bash
# Método 1: Directo
python main.py

# Método 2: Con uvicorn (recomendado para desarrollo)
uvicorn main:app --reload --port 8001 --host 0.0.0.0
```

El servicio estará disponible en: **http://localhost:8001**

📖 Para más opciones de Docker, consulta: [DOCKER_README.md](DOCKER_README.md)

## 📚 API Endpoints

### Documentos
- `POST /documents/upload` - Subir y procesar documentos
- `GET /projects/{project_id}/documents` - Obtener documentos de un proyecto
- `DELETE /documents/{document_id}` - Eliminar documento

### Consultas RAG
- `POST /query` - Realizar consultas semánticas sobre documentos
  - Soporta resúmenes ejecutivos inteligentes
  - Contexto automático por proyecto

### Presupuestos
- `POST /budget/extract-from-file` - **🆕 NUEVO:** Extraer presupuesto desde Excel/Word
- `POST /budget/generate` - Generar presupuesto completo (con 3 estrategias automáticas)
- `GET /projects/{project_id}/budget/suggestions` - Obtener sugerencias de presupuesto

### Planificación de Recursos
- `POST /resources/plan` - Generar plan de asignación de recursos con IA

## 📖 Documentación Detallada

- **API Interactiva (Swagger):** http://localhost:8001/docs
- **API Docs (ReDoc):** http://localhost:8001/redoc
- **Guía de Extracción de Presupuestos:** Ver `PRESUPUESTO_EXTRACTION_GUIDE.md`
- **Configuración de Gemini:** Ver `GEMINI_SETUP.md`
- **Guía de Ejecución:** Ver `GUIA_EJECUCION.md`

## 🎯 Ejemplo de Uso Rápido

### 1. Subir un documento de proyecto

```bash
curl -X POST "http://localhost:8001/documents/upload" \
  -F "file=@DocumentoTecnico.pdf" \
  -F "project_id=1" \
  -F "document_type=project_document"
```

### 2. Generar resumen ejecutivo del proyecto

```bash
curl -X POST "http://localhost:8001/query" \
  -H "Content-Type: application/json" \
  -d '{
    "question": "Genera un RESUMEN EJECUTIVO COMPLETO de este proyecto. Incluye justificación, objetivos, alcance territorial, población objetivo, componentes principales, actividades clave, resultados esperados y actores involucrados.",
    "project_id": 1,
    "top_k": 8
  }'
```

### 3. Extraer presupuesto desde Excel

```bash
curl -X POST "http://localhost:8001/budget/extract-from-file" \
  -F "file=@Presupuesto.xlsx" \
  -F "project_id=1"
```

### 4. Generar presupuesto completo con IA

```bash
curl -X POST "http://localhost:8001/budget/generate" \
  -H "Content-Type: application/json" \
  -d '{
    "project_id": 1,
    "project_description": "Desarrollo de plataforma de telemedicina cardiovascular",
    "duration_years": 2
  }'
```

## 🏗️ Arquitectura

```
RAG-Service/
├── main.py                          # API FastAPI principal
├── services/
│   ├── document_processor.py       # Procesamiento de documentos
│   ├── rag_service.py              # Servicio RAG core con búsqueda semántica
│   ├── llm_service.py              # Integración con LLMs (Gemini/OpenAI)
│   ├── budget_automation.py        # Automatización de presupuestos
│   └── budget_extractor.py         # 🆕 Extracción inteligente de Excel/Word
├── models/
│   └── schemas.py                  # Modelos Pydantic
├── chroma_db/                      # Base de datos vectorial ChromaDB
└── generated_budgets/              # Presupuestos generados en Excel
```

## 🔧 Tecnologías Utilizadas

- **FastAPI** - Framework web moderno y rápido
- **ChromaDB** - Base de datos vectorial para embeddings
- **Sentence Transformers** - Generación de embeddings multilingües
- **Google Gemini / OpenAI** - Modelos de lenguaje para generación
- **Pandas + openpyxl** - Procesamiento y generación de Excel
- **python-docx** - Procesamiento de documentos Word
- **PyPDF2** - Extracción de texto de PDFs

## 🧪 Testing

```bash
# Verificar salud del servicio
curl http://localhost:8001/health

# Listar modelos disponibles de Gemini
python list_gemini_models.py
```

## 📝 Notas Importantes

1. **API Key de LLM requerida:** Para generación de presupuestos y resúmenes avanzados, necesitas una API key de Google Gemini o OpenAI.

2. **Formatos de Excel soportados:** El extractor es flexible y detecta automáticamente las columnas, pero funciona mejor con encabezados claros.

3. **Mapeo de rubros:** El sistema usa palabras clave en español para clasificar actividades automáticamente.

4. **Contexto del proyecto:** Mientras más documentos subas del proyecto, mejores serán las generaciones del LLM.

## 🐛 Troubleshooting

### Error: "GEMINI_API_KEY no está configurada"
- Verifica que el archivo `.env` existe y contiene la variable `GEMINI_API_KEY`
- Asegúrate de que el archivo `.env` está en la raíz del proyecto `RAG-Service/`

### El extractor no detecta las columnas correctamente
- Usa encabezados claros en la primera fila: "Actividad", "Cantidad", "Valor Unitario", "Total"
- Consulta `PRESUPUESTO_EXTRACTION_GUIDE.md` para formatos recomendados

### El LLM genera presupuestos irreales
- Baja la temperatura en `.env`: `LLM_TEMPERATURE=0.2`
- Proporciona más contexto subiendo documentos del proyecto
- Usa descripciones más detalladas en `project_description`

## 🤝 Integración con Backend

El servicio RAG se integra con el backend .NET a través del `RAGController`:

```
Backend API (http://localhost:5000)
    ↓
    ├─ /api/RAG/documents/upload → RAG Service
    ├─ /api/RAG/query → RAG Service
    ├─ /api/RAG/budget/generate → RAG Service
    └─ /api/RAG/budget/save-extracted → Guarda en BD SQL Server
```

## 📄 Licencia

Este proyecto es parte del sistema de gestión de proyectos de investigación.

## 👥 Equipo de Desarrollo

Desarrollado para facilitar la formulación y ejecución de proyectos de CTeI.
