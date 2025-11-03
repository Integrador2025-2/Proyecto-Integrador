# Proyecto Integrador - Sistema RAG para Automatización de Presupuestos

Este proyecto implementa un sistema completo de automatización de presupuestos utilizando tecnología RAG (Retrieval-Augmented Generation) basado en documentos de proyectos.

## 🏗️ Arquitectura del Sistema

El sistema está compuesto por tres servicios principales:

### 1. **Backend .NET** (Puerto 5000)
- API REST con autenticación JWT
- Gestión de proyectos, actividades y rubros
- Integración con el servicio RAG
- Base de datos SQL Server

### 2. **Servicio RAG Python** (Puerto 8001)
- Procesamiento de documentos (PDF, DOCX, TXT, XLSX)
- Base de datos vectorial con ChromaDB
- Búsqueda semántica con embeddings
- Generación automática de presupuestos Excel

### 3. **Frontend React** (Puerto 3000)
- Interfaz de usuario moderna con Tailwind CSS
- Dashboard RAG interactivo
- Subida de documentos
- Consultas semánticas
- Generación de presupuestos

## 🚀 Inicio Rápido

### Prerrequisitos

- Docker y Docker Compose
- Node.js 18+ (para desarrollo local)
- .NET 8 SDK (para desarrollo local)
- Python 3.11+ (para desarrollo local)

### Ejecutar con Docker (Recomendado)

1. **Clonar el repositorio**
   ```bash
   git clone <repository-url>
   cd Proyecto-Integrador
   ```

2. **Configurar variables de entorno**
   ```bash
   cp RAG-Service/env.example RAG-Service/.env
   # Editar RAG-Service/.env con tus configuraciones
   ```

3. **Ejecutar todos los servicios**
   ```bash
   docker-compose up -d
   ```

4. **Acceder a las aplicaciones**
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:5000
   - RAG Service: http://localhost:8001
   - Swagger UI: http://localhost:5000/swagger

### Ejecutar en Desarrollo Local

#### Backend .NET

```bash
cd Backend
dotnet restore
dotnet run
```

#### Servicio RAG Python

```bash
cd RAG-Service
python -m venv venv
source venv/bin/activate  # Windows: venv\Scripts\activate
pip install -r requirements.txt
uvicorn main:app --host 0.0.0.0 --port 8001 --reload
```

#### Frontend React

```bash
cd Frontend
npm install
npm run dev
```

## 📋 Funcionalidades

### 1. Gestión de Documentos
- Subida de documentos de proyecto
- Procesamiento automático de texto
- Almacenamiento en base de datos vectorial
- Soporte para múltiples formatos

### 2. Consultas Semánticas
- Búsqueda inteligente en documentos
- Respuestas basadas en contexto
- Puntuación de confianza
- Fuentes de información

### 3. Automatización de Presupuestos
- Generación automática basada en documentos
- Categorización por rubros del sistema
- Exportación a Excel
- Sugerencias inteligentes

### 4. Dashboard Interactivo
- Interfaz unificada para todas las funcionalidades
- Estadísticas en tiempo real
- Gestión de proyectos
- Análisis de documentos

## 🛠️ Tecnologías Utilizadas

### Backend
- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM
- **SQL Server**: Base de datos relacional
- **Redis**: Caché
- **JWT**: Autenticación
- **MediatR**: CQRS

### Servicio RAG
- **FastAPI**: Framework web
- **ChromaDB**: Base de datos vectorial
- **Sentence Transformers**: Embeddings
- **LangChain**: Framework de IA
- **OpenPyXL**: Manipulación Excel

### Frontend
- **React 19**: Framework UI
- **TypeScript**: Tipado estático
- **Tailwind CSS**: Estilos
- **Radix UI**: Componentes
- **Zustand**: Estado global

## 📁 Estructura del Proyecto

```
Proyecto-Integrador/
├── Backend/                 # API .NET
│   ├── Controllers/         # Controladores API
│   ├── Models/             # Modelos de dominio y DTOs
│   ├── Services/           # Servicios de negocio
│   ├── Infrastructure/     # Repositorios y contexto EF
│   └── Commands/Queries/   # CQRS
├── Frontend/               # Aplicación React
│   ├── src/
│   │   ├── components/     # Componentes UI
│   │   ├── services/       # Servicios API
│   │   └── types/          # Tipos TypeScript
├── RAG-Service/            # Servicio RAG Python
│   ├── services/           # Lógica de negocio
│   ├── models/             # Modelos Pydantic
│   └── main.py            # Aplicación FastAPI
├── docker-compose.yml      # Orquestación de servicios
└── README.md              # Este archivo
```

## 🔧 Configuración

### Variables de Entorno

#### RAG Service (.env)
```env
OPENAI_API_KEY=your_openai_api_key_here
EMBEDDING_MODEL=paraphrase-multilingual-MiniLM-L12-v2
CHROMA_DB_PATH=./chroma_db
BACKEND_API_URL=http://localhost:5000
```

#### Backend (appsettings.json)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ProyectoIntegradorDb;..."
  },
  "RAGService": {
    "BaseUrl": "http://localhost:8001"
  }
}
```

## 📊 Categorías de Presupuesto

El sistema maneja las siguientes categorías:

1. **Talento Humano**: Salarios, honorarios, recursos humanos
2. **Servicios Tecnológicos**: Consultoría, desarrollo, servicios técnicos
3. **Equipos y Software**: Hardware, software, licencias
4. **Materiales e Insumos**: Suministros, herramientas, consumibles
5. **Capacitación y Eventos**: Cursos, talleres, eventos
6. **Gastos de Viaje**: Transporte, hospedaje, viáticos

## 🔍 Uso del Sistema

### 1. Subir Documentos
1. Acceder al Dashboard RAG
2. Ir a la pestaña "Documentos"
3. Seleccionar archivo y tipo de documento
4. Subir al sistema

### 2. Realizar Consultas
1. Ir a la pestaña "Consultas"
2. Escribir pregunta sobre el proyecto
3. Obtener respuesta basada en documentos

### 3. Generar Presupuesto
1. Ir a la pestaña "Presupuesto"
2. Describir el proyecto
3. Seleccionar categorías de presupuesto
4. Generar y descargar Excel

## 🐳 Docker

### Comandos Útiles

```bash
# Iniciar todos los servicios
docker-compose up -d

# Ver logs
docker-compose logs -f

# Reiniciar un servicio
docker-compose restart rag-service

# Detener todos los servicios
docker-compose down

# Limpiar volúmenes
docker-compose down -v
```

## 🧪 Testing

### Backend
```bash
cd Backend
dotnet test
```

### Frontend
```bash
cd Frontend
npm test
```

### RAG Service
```bash
cd RAG-Service
pytest tests/
```

## 📈 Monitoreo

### Health Checks
- Backend: `GET /health`
- RAG Service: `GET /health`

### Métricas
- Número de documentos procesados
- Tiempo de respuesta de consultas
- Confianza de respuestas generadas
- Uso de recursos del sistema

## 🚨 Troubleshooting

### Problemas Comunes

1. **Error de conexión a base de datos**
   - Verificar que SQL Server esté ejecutándose
   - Revisar cadena de conexión

2. **Servicio RAG no responde**
   - Verificar que el puerto 8001 esté disponible
   - Revisar logs del contenedor

3. **Frontend no carga**
   - Verificar que el puerto 3000 esté disponible
   - Revisar configuración de proxy

4. **Documentos no se procesan**
   - Verificar formatos soportados
   - Revisar tamaño máximo de archivo

## 🤝 Contribución

1. Fork el proyecto
2. Crear una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abrir un Pull Request

## 📄 Licencia

Este proyecto está bajo la Licencia MIT. Ver `LICENSE` para más detalles.

## 👥 Equipo

- **Desarrollador Principal**: [Tu Nombre]
- **Email**: tu.email@ejemplo.com

## 📞 Soporte

Para soporte técnico o preguntas:
- Crear un issue en GitHub
- Contactar al equipo de desarrollo
- Revisar la documentación técnica

---

**Nota**: Este sistema está diseñado para automatizar la generación de presupuestos basándose en documentos de proyectos utilizando tecnología RAG. Asegúrate de revisar y validar todos los presupuestos generados antes de su uso en producción.
