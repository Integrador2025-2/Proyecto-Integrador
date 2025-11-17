# Servicio RAG para Automatización de Presupuestos

Este servicio implementa un sistema RAG (Retrieval-Augmented Generation) para automatizar la generación de presupuestos basándose en documentos de proyectos.

## Características

- **Procesamiento de Documentos**: Soporta PDF, DOCX, TXT y XLSX
- **Base de Datos Vectorial**: Utiliza ChromaDB para almacenamiento de embeddings
- **Búsqueda Semántica**: Modelo de embeddings multilingüe para búsquedas precisas
- **Generación de Presupuestos**: Automatización basada en documentos de proyecto
- **Integración con .NET**: API REST para comunicación con el backend

## Tecnologías

- **FastAPI**: Framework web para Python
- **ChromaDB**: Base de datos vectorial
- **Sentence Transformers**: Modelo de embeddings
- **LangChain**: Framework para aplicaciones de IA
- **OpenPyXL**: Manipulación de archivos Excel

## Instalación

### Requisitos

- Python 3.11+
- Docker (opcional)

### Instalación Local

1. Clonar el repositorio
2. Navegar al directorio del servicio RAG:
   ```bash
   cd RAG-Service
   ```

3. Crear entorno virtual:
   ```bash
   python -m venv venv
   source venv/bin/activate  # En Windows: venv\Scripts\activate
   ```

4. Instalar dependencias:
   ```bash
   pip install -r requirements.txt
   ```

5. Configurar variables de entorno:
   ```bash
   cp env.example .env
   # Editar .env con tus configuraciones
   ```

6. Ejecutar el servicio:
   ```bash
   uvicorn main:app --host 0.0.0.0 --port 8001 --reload
   ```

### Instalación con Docker

```bash
docker-compose up rag-service
```

## Uso

### Endpoints Principales

#### 1. Subir Documento
```http
POST /documents/upload
Content-Type: multipart/form-data

file: [archivo]
project_id: [ID del proyecto]
document_type: [tipo de documento]
```

#### 2. Consultar Documentos
```http
POST /query
Content-Type: application/json

{
  "question": "¿Cuáles son los costos estimados?",
  "project_id": 1,
  "top_k": 5
}
```

#### 3. Generar Presupuesto
```http
POST /budget/generate
Content-Type: application/json

{
  "project_id": 1,
  "project_description": "Descripción del proyecto",
  "budget_categories": ["TalentoHumano", "ServiciosTecnologicos"],
  "duration_years": 2
}
```

#### 4. Obtener Documentos del Proyecto
```http
GET /projects/{project_id}/documents
```

#### 5. Obtener Sugerencias de Presupuesto
```http
GET /projects/{project_id}/budget/suggestions?category=TalentoHumano
```

### Categorías de Presupuesto

El sistema reconoce las siguientes categorías:

- **TalentoHumano**: Recursos humanos, salarios, honorarios
- **ServiciosTecnologicos**: Servicios de tecnología, consultoría técnica
- **EquiposSoftware**: Equipos de cómputo, software, licencias
- **MaterialesInsumos**: Materiales, insumos, suministros
- **CapacitacionEventos**: Capacitaciones, eventos, talleres
- **GastosViaje**: Gastos de viaje, transporte, hospedaje

## Configuración

### Variables de Entorno

```env
# Configuración del servicio RAG
OPENAI_API_KEY=your_openai_api_key_here
EMBEDDING_MODEL=paraphrase-multilingual-MiniLM-L12-v2
CHROMA_DB_PATH=./chroma_db

# Configuración del backend .NET
BACKEND_API_URL=http://localhost:5000
BACKEND_API_KEY=your_backend_api_key_here

# Configuración de archivos
UPLOAD_DIR=./uploads
GENERATED_BUDGETS_DIR=./generated_budgets
MAX_FILE_SIZE_MB=50

# Configuración de la aplicación
DEBUG=True
LOG_LEVEL=INFO
```

## Estructura del Proyecto

```
RAG-Service/
├── main.py                 # Aplicación principal FastAPI
├── requirements.txt        # Dependencias de Python
├── Dockerfile             # Imagen Docker
├── env.example            # Variables de entorno de ejemplo
├── models/
│   └── schemas.py         # Modelos Pydantic
├── services/
│   ├── document_processor.py    # Procesamiento de documentos
│   ├── rag_service.py           # Servicio RAG principal
│   └── budget_automation.py     # Automatización de presupuestos
└── README.md              # Este archivo
```

## Desarrollo

### Ejecutar en Modo Desarrollo

```bash
uvicorn main:app --host 0.0.0.0 --port 8001 --reload
```

### Ejecutar Tests

```bash
pytest tests/
```

### Linting

```bash
flake8 .
black .
```

## Integración

### Con Backend .NET

El servicio se integra con el backend .NET a través de:

1. **RAGController**: Controlador que expone endpoints para el frontend
2. **HttpClient**: Comunicación HTTP con el servicio RAG
3. **Configuración**: URLs y configuraciones en appsettings.json

### Con Frontend React

El frontend consume el servicio a través de:

1. **ragService.ts**: Cliente HTTP para comunicación
2. **RAGDashboard.tsx**: Interfaz principal
3. **Componentes especializados**: Para subida, consultas y presupuestos

## Monitoreo

### Health Check

```http
GET /health
```

### Logs

Los logs se generan en la consola y pueden ser configurados para archivos.

### Métricas

- Número de documentos procesados
- Tiempo de respuesta de consultas
- Confianza de las respuestas generadas
- Uso de memoria y CPU

## Troubleshooting

### Problemas Comunes

1. **Error de conexión a ChromaDB**: Verificar permisos de escritura en el directorio
2. **Modelo de embeddings no carga**: Verificar conexión a internet para descarga inicial
3. **Error de memoria**: Reducir el tamaño de chunks o usar un modelo más pequeño
4. **Archivos no se procesan**: Verificar formatos soportados y tamaño máximo

### Logs de Debug

```bash
LOG_LEVEL=DEBUG uvicorn main:app --host 0.0.0.0 --port 8001
```

## Contribución

1. Fork el proyecto
2. Crear una rama para tu feature
3. Commit tus cambios
4. Push a la rama
5. Abrir un Pull Request

## Licencia

Este proyecto está bajo la Licencia MIT.




