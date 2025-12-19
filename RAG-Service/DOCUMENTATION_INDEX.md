# 📚 Índice de Documentación - RAG Service

Guía completa de toda la documentación disponible para el RAG-Service.

---

## 🚀 Para Empezar

### 1. **[QUICKSTART.md](QUICKSTART.md)** ⭐ Recomendado
   - Inicio en 5 minutos
   - Configuración mínima
   - Primeros pasos con Docker
   - **Ideal para**: Nuevos usuarios que quieren probar rápido

### 2. **[README.md](README.md)**
   - Descripción general del proyecto
   - Características principales
   - Instalación local (sin Docker)
   - Arquitectura del sistema
   - **Ideal para**: Entender qué hace el servicio

---

## 🐳 Docker (Recomendado para Producción)

### 3. **[DOCKER_README.md](DOCKER_README.md)** 📖 Guía Principal
   - Guía completa de Docker (47 secciones)
   - Instalación y configuración detallada
   - Comandos de gestión
   - Solución de problemas
   - Configuración de producción
   - **Ideal para**: Deployment y uso avanzado

### 4. **[DOCKER_SUMMARY.md](DOCKER_SUMMARY.md)**
   - Resumen ejecutivo de la dockerización
   - Archivos creados
   - Arquitectura Docker
   - Comandos rápidos
   - Métricas del proyecto
   - **Ideal para**: Vista general técnica

---

## 🔧 Scripts de Gestión

### Instalación Automática

#### 5. **install-docker.sh** (Linux/Mac)
   ```bash
   chmod +x install-docker.sh
   ./install-docker.sh
   ```
   - Instalación completamente automatizada
   - Verifica requisitos
   - Configura variables de entorno
   - Construye e inicia servicios
   - Ejecuta pruebas

#### 6. **install-docker.ps1** (Windows)
   ```powershell
   .\install-docker.ps1
   ```
   - Igual que arriba pero para Windows
   - PowerShell nativo
   - Interfaz con colores

### Gestión de Servicios

#### 7. **docker-manage.sh** (Linux/Mac)
   ```bash
   ./docker-manage.sh [comando]
   ```
   Comandos disponibles:
   - `start` - Iniciar servicios
   - `stop` - Detener servicios
   - `restart` - Reiniciar
   - `logs` - Ver logs
   - `status` - Ver estado
   - `backup` - Hacer backup
   - `restore` - Restaurar backup
   - `clean` - Limpiar todo
   - `update` - Actualizar servicio
   - **16 comandos en total**

#### 8. **docker-manage.ps1** (Windows)
   ```powershell
   .\docker-manage.ps1 [comando]
   ```
   - Mismas funcionalidades que el script de Linux
   - Versión PowerShell con colores

### Scripts de Pruebas

#### 9. **test-docker.sh** (Linux/Mac)
   ```bash
   ./test-docker.sh
   ```
   - Verificación automática de 5 tests
   - Health check
   - Documentación API
   - Variables de entorno
   - Volúmenes
   - Logs recientes

#### 10. **test-docker.ps1** (Windows)
   ```powershell
   .\test-docker.ps1
   ```
   - Mismas pruebas que el script de Linux
   - Versión PowerShell

---

## 📖 Uso de la API

### 11. **[API_EXAMPLES.md](API_EXAMPLES.md)** 🎯 Muy Útil
   - 10+ ejemplos completos con `curl`
   - Ejemplos en Python
   - Ejemplos en JavaScript
   - Casos de uso completos
   - Testing con diferentes archivos
   - **Ideal para**: Desarrolladores que integran el servicio

---

## 🔑 Configuración

### 12. **[GEMINI_SETUP.md](GEMINI_SETUP.md)**
   - Cómo obtener API Key de Google Gemini (gratis)
   - Configuración paso a paso
   - Modelos disponibles
   - **Ideal para**: Primera configuración

### 13. **env.example**
   - Template de variables de entorno
   - Todas las opciones configurables
   - Comentarios explicativos
   - **Uso**: Copiar a `.env` y modificar

### 14. **.env.docker**
   - Template específico para Docker
   - Variables esenciales
   - **Uso**: `cp .env.docker .env`

---

## 📋 Guías Específicas

### 15. **[GUIA_EJECUCION.md](GUIA_EJECUCION.md)**
   - Guía de ejecución local
   - Instalación sin Docker
   - Configuración manual
   - **Ideal para**: Desarrollo local

### 16. **[PRESUPUESTO_EXTRACTION_GUIDE.md](PRESUPUESTO_EXTRACTION_GUIDE.md)**
   - Guía de extracción de presupuestos desde Excel/Word
   - Formatos soportados
   - Mapeo de rubros
   - Ejemplos prácticos
   - **Ideal para**: Usar la funcionalidad de presupuestos

---

## 🛠️ Archivos de Configuración Docker

### 17. **Dockerfile**
   - Imagen principal del servicio FastAPI
   - Python 3.12.9
   - Multi-stage optimizado
   - Usuario no-root

### 18. **Dockerfile.streamlit**
   - Imagen para interfaz Streamlit
   - Separada del servicio principal
   - Optimizada para UI

### 19. **docker-compose.yml**
   - Orquestación de servicios
   - Configuración de red
   - Volúmenes persistentes
   - Variables de entorno
   - Health checks

### 20. **.dockerignore**
   - Archivos excluidos del build
   - Optimización de tamaño
   - Seguridad

---

## 📝 Código Fuente

### 21. **main.py**
   - API principal FastAPI
   - Endpoints REST
   - Middleware CORS
   - Inicialización de servicios

### 22. **streamlit_app.py**
   - Interfaz web con Streamlit
   - Formularios interactivos
   - Visualizaciones

### 23. **services/**
   - `rag_service.py` - Lógica RAG core
   - `llm_service.py` - Integración con LLMs
   - `document_processor.py` - Procesamiento de docs
   - `budget_automation.py` - Automatización presupuestos
   - `budget_extractor.py` - Extracción inteligente
   - `cotizacion_service.py` - Servicio de cotizaciones

### 24. **models/schemas.py**
   - Modelos Pydantic
   - Validación de datos
   - Tipos personalizados

---

## 📊 Utilidades

### 25. **list_gemini_models.py**
   - Script para listar modelos de Gemini disponibles
   - Verificación de API Key
   - **Uso**: `python list_gemini_models.py`

### 26. **requirements.txt**
   - Dependencias Python
   - Versiones específicas
   - Actualizado para Python 3.12.9

---

## 🗂️ Directorios Importantes

### 27. **chroma_db/**
   - Base de datos vectorial
   - Almacena embeddings
   - Persiste entre reinicios

### 28. **uploads/**
   - Documentos subidos por usuarios
   - PDF, DOCX, XLSX, TXT

### 29. **generated_budgets/**
   - Presupuestos generados en Excel
   - Exportaciones automáticas

---

## 📚 Guía de Lectura Recomendada

### Para Principiantes
1. **[QUICKSTART.md](QUICKSTART.md)** - Empezar en 5 minutos
2. **[README.md](README.md)** - Entender el proyecto
3. **[API_EXAMPLES.md](API_EXAMPLES.md)** - Probar la API
4. **[GEMINI_SETUP.md](GEMINI_SETUP.md)** - Configurar LLM

### Para Administradores
1. **[DOCKER_README.md](DOCKER_README.md)** - Guía completa
2. **[DOCKER_SUMMARY.md](DOCKER_SUMMARY.md)** - Vista general
3. Scripts de gestión - Automatización
4. Scripts de pruebas - Monitoreo

### Para Desarrolladores
1. **[README.md](README.md)** - Arquitectura
2. **[API_EXAMPLES.md](API_EXAMPLES.md)** - Integración
3. **[PRESUPUESTO_EXTRACTION_GUIDE.md](PRESUPUESTO_EXTRACTION_GUIDE.md)** - Features
4. Código fuente - Implementación

---

## 🎯 Casos de Uso por Documento

| Necesito... | Lee este documento |
|-------------|-------------------|
| Instalar rápido | [QUICKSTART.md](QUICKSTART.md) |
| Entender qué hace | [README.md](README.md) |
| Deployar en producción | [DOCKER_README.md](DOCKER_README.md) |
| Integrar con mi app | [API_EXAMPLES.md](API_EXAMPLES.md) |
| Extraer presupuestos | [PRESUPUESTO_EXTRACTION_GUIDE.md](PRESUPUESTO_EXTRACTION_GUIDE.md) |
| Obtener API Key | [GEMINI_SETUP.md](GEMINI_SETUP.md) |
| Solucionar problemas | [DOCKER_README.md](DOCKER_README.md) → Troubleshooting |
| Hacer backup | Scripts de gestión + [DOCKER_README.md](DOCKER_README.md) |
| Ver arquitectura | [DOCKER_SUMMARY.md](DOCKER_SUMMARY.md) |
| Comandos rápidos | [DOCKER_README.md](DOCKER_README.md) → Comandos útiles |

---

## 🔍 Búsqueda Rápida

### Temas Principales

- **Docker**: [DOCKER_README.md](DOCKER_README.md), [DOCKER_SUMMARY.md](DOCKER_SUMMARY.md), [QUICKSTART.md](QUICKSTART.md)
- **API**: [API_EXAMPLES.md](API_EXAMPLES.md), [README.md](README.md), main.py
- **Presupuestos**: [PRESUPUESTO_EXTRACTION_GUIDE.md](PRESUPUESTO_EXTRACTION_GUIDE.md), budget_*.py
- **Configuración**: [GEMINI_SETUP.md](GEMINI_SETUP.md), env.example, .env.docker
- **Scripts**: docker-manage.*, test-docker.*, install-docker.*
- **Troubleshooting**: [DOCKER_README.md](DOCKER_README.md) → Solución de problemas

---

## 📊 Estadísticas de Documentación

- **Total de archivos de documentación**: 16+ archivos Markdown
- **Total de scripts**: 8 scripts ejecutables
- **Líneas de documentación**: ~3,000+
- **Ejemplos de código**: 50+
- **Comandos documentados**: 150+
- **Secciones de troubleshooting**: 20+

---

## 🔄 Flujo de Trabajo Recomendado

### Primera vez con el proyecto
```
1. QUICKSTART.md          (5 min)
2. README.md              (10 min)
3. Probar servicio        (5 min)
4. API_EXAMPLES.md        (15 min)
```

### Deployment a producción
```
1. DOCKER_README.md       (30 min)
2. Configurar .env
3. ./install-docker.sh
4. ./test-docker.sh
5. Monitorear con ./docker-manage.sh logs
```

### Desarrollo de features
```
1. README.md              (Arquitectura)
2. Código fuente
3. API_EXAMPLES.md        (Testing)
4. PRESUPUESTO_EXTRACTION_GUIDE.md (Features específicas)
```

---

## 🆘 Soporte

### ¿Dónde buscar ayuda?

1. **Errores de instalación**: [DOCKER_README.md](DOCKER_README.md) → Troubleshooting
2. **Errores de API**: [API_EXAMPLES.md](API_EXAMPLES.md) → Debugging
3. **Configuración de LLM**: [GEMINI_SETUP.md](GEMINI_SETUP.md)
4. **Presupuestos**: [PRESUPUESTO_EXTRACTION_GUIDE.md](PRESUPUESTO_EXTRACTION_GUIDE.md)
5. **Docker**: [DOCKER_README.md](DOCKER_README.md) → Solución de problemas

---

## ✨ Actualizaciones

Este índice se actualiza cuando se agregan nuevos documentos al proyecto.

**Última actualización**: Diciembre 18, 2025

---

**🚀 ¡Explora la documentación y aprovecha al máximo RAG-Service!**
