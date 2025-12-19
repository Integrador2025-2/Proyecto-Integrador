# 🐳 RAG Budget Automation Service - Guía Docker

Microservicio de automatización de presupuestos utilizando RAG (Retrieval-Augmented Generation) con FastAPI y modelos de lenguaje (Gemini/OpenAI).

## 📋 Requisitos Previos

- **Docker**: versión 20.10 o superior
- **Docker Compose**: versión 2.0 o superior
- **API Key de Gemini o OpenAI**: para el servicio de LLM

### Verificar instalación de Docker

```bash
docker --version
docker-compose --version
```

---

## 🚀 Inicio Rápido

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd RAG-Service
```

### 2. Configurar variables de entorno

Crea un archivo `.env` en la raíz del proyecto basado en `.env.docker`:

```bash
cp .env.docker .env
```

Edita el archivo `.env` y agrega tus API keys:

```bash
# API Keys
GEMINI_API_KEY=tu_api_key_de_gemini_aqui
# OPENAI_API_KEY=tu_api_key_de_openai_aqui (opcional)

# Backend Configuration
BACKEND_API_URL=http://host.docker.internal:5000
BACKEND_API_KEY=tu_backend_api_key_aqui
```

#### 🔑 Obtener API Key de Gemini

1. Ve a [Google AI Studio](https://aistudio.google.com/app/apikey)
2. Inicia sesión con tu cuenta de Google
3. Haz clic en "Get API Key" o "Create API Key"
4. Copia la API key generada
5. Pégala en el archivo `.env`

### 3. Construir y ejecutar con Docker Compose

#### Opción A: Ejecutar solo el servicio RAG

```bash
docker-compose up -d rag-service
```

#### Opción B: Ejecutar servicio RAG + Interfaz Streamlit

```bash
docker-compose up -d
```

### 4. Verificar que los servicios están corriendo

```bash
docker-compose ps
```

Deberías ver:

```
NAME                COMMAND                  SERVICE             STATUS              PORTS
rag-service         "uvicorn main:app --…"   rag-service         Up                  0.0.0.0:8001->8001/tcp
rag-streamlit       "streamlit run strea…"   streamlit-app       Up                  0.0.0.0:8501->8501/tcp
```

### 5. Probar el servicio

#### Health Check

```bash
curl http://localhost:8001/health
```

Respuesta esperada:
```json
{
  "status": "healthy",
  "message": "RAG Budget Automation Service is running"
}
```

#### Documentación de la API

Abre en tu navegador:
- **Swagger UI**: http://localhost:8001/docs
- **ReDoc**: http://localhost:8001/redoc

#### Interfaz Streamlit (si la ejecutaste)

Abre en tu navegador:
- **Streamlit**: http://localhost:8501

---

## 🔧 Comandos Docker Útiles

### Ver logs en tiempo real

```bash
# Todos los servicios
docker-compose logs -f

# Solo RAG Service
docker-compose logs -f rag-service

# Solo Streamlit
docker-compose logs -f streamlit-app
```

### Detener los servicios

```bash
docker-compose stop
```

### Detener y eliminar contenedores

```bash
docker-compose down
```

### Detener, eliminar contenedores y volúmenes (⚠️ elimina datos persistentes)

```bash
docker-compose down -v
```

### Reconstruir las imágenes

```bash
docker-compose build --no-cache
docker-compose up -d
```

### Ejecutar comandos dentro del contenedor

```bash
# Acceder al shell del contenedor
docker exec -it rag-service bash

# Ejecutar un comando específico
docker exec rag-service python -c "print('Hello from container')"
```

### Ver uso de recursos

```bash
docker stats
```

---

## 🏗️ Construcción Manual (sin Docker Compose)

### 1. Construir la imagen

```bash
docker build -t rag-service:latest .
```

### 2. Ejecutar el contenedor

```bash
docker run -d \
  --name rag-service \
  -p 8001:8001 \
  -e GEMINI_API_KEY=tu_api_key_aqui \
  -e BACKEND_API_URL=http://host.docker.internal:5000 \
  -v $(pwd)/chroma_db:/app/chroma_db \
  -v $(pwd)/uploads:/app/uploads \
  -v $(pwd)/generated_budgets:/app/generated_budgets \
  rag-service:latest
```

### 3. Ver logs

```bash
docker logs -f rag-service
```

### 4. Detener y eliminar

```bash
docker stop rag-service
docker rm rag-service
```

---

## 📂 Estructura de Volúmenes

Los siguientes directorios se montan como volúmenes para persistir datos:

```
./chroma_db              → Base de datos vectorial (ChromaDB)
./uploads                → Documentos subidos
./generated_budgets      → Presupuestos generados
```

---

## 🧪 Pruebas de la API

### 1. Subir un documento

```bash
curl -X POST "http://localhost:8001/documents/upload" \
  -H "Content-Type: multipart/form-data" \
  -F "file=@/ruta/a/tu/documento.pdf" \
  -F "project_id=1" \
  -F "document_type=project_document"
```

### 2. Hacer una consulta RAG

```bash
curl -X POST "http://localhost:8001/query" \
  -H "Content-Type: application/json" \
  -d '{
    "query": "¿Cuáles son los objetivos del proyecto?",
    "project_id": 1,
    "top_k": 3
  }'
```

### 3. Generar un presupuesto

```bash
curl -X POST "http://localhost:8001/budgets/generate" \
  -H "Content-Type: application/json" \
  -d '{
    "project_id": 1,
    "budget_type": "detailed",
    "include_resources": true
  }'
```

---

## 🔍 Solución de Problemas

### El contenedor no inicia

```bash
# Ver logs detallados
docker-compose logs rag-service

# Verificar que el puerto 8001 no esté en uso
netstat -ano | findstr :8001  # Windows
lsof -i :8001                  # Linux/Mac
```

### Error de API Key

```bash
# Verificar que las variables de entorno están configuradas
docker exec rag-service env | grep GEMINI_API_KEY
```

### Problemas de conexión con el backend

Si tu backend está corriendo en tu máquina host (no en Docker):

- **Windows/Mac**: Usa `http://host.docker.internal:5000`
- **Linux**: Usa `http://172.17.0.1:5000` o la IP de tu interfaz docker0

```bash
# Encontrar la IP del host en Linux
ip addr show docker0
```

### Permisos en Linux

Si tienes problemas de permisos con los volúmenes en Linux:

```bash
sudo chown -R 1000:1000 chroma_db uploads generated_budgets
```

### Limpiar todo y empezar de nuevo

```bash
# Detener y eliminar todo
docker-compose down -v

# Eliminar imágenes
docker rmi rag-service:latest

# Limpiar directorios
rm -rf chroma_db/* uploads/* generated_budgets/*

# Volver a construir
docker-compose build --no-cache
docker-compose up -d
```

---

## 🌐 Configuración de Producción

### Variables de entorno importantes para producción

```bash
# .env para producción
LLM_PROVIDER=gemini
GEMINI_API_KEY=tu_api_key_produccion
DEBUG=False
LOG_LEVEL=WARNING
MAX_FILE_SIZE_MB=50
```

### Recomendaciones de seguridad

1. **No commits de .env**: Asegúrate de que `.env` está en `.gitignore`
2. **Secrets Management**: Usa Docker secrets o servicios de gestión de secretos
3. **Firewall**: Restringe el acceso al puerto 8001
4. **HTTPS**: Usa un reverse proxy (nginx) con SSL/TLS
5. **Rate Limiting**: Implementa límites de tasa en el API Gateway

### Ejemplo con nginx como reverse proxy

```nginx
server {
    listen 80;
    server_name tu-dominio.com;

    location / {
        proxy_pass http://localhost:8001;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

---

## 📊 Monitoreo

### Health checks

Docker Compose incluye health checks automáticos. Para verificar el estado:

```bash
docker inspect rag-service --format='{{.State.Health.Status}}'
```

### Métricas básicas

```bash
# CPU y memoria
docker stats rag-service

# Espacio en disco usado por volúmenes
docker system df -v
```

---

## 🆘 Soporte

### Logs importantes

```bash
# Logs de inicio
docker-compose logs rag-service | head -50

# Logs de errores
docker-compose logs rag-service | grep ERROR

# Logs en tiempo real filtrados
docker-compose logs -f rag-service | grep -i "error\|warning"
```

### Información del sistema

```bash
docker info
docker-compose version
docker inspect rag-service
```

---

## 📝 Notas Adicionales

- **Tiempo de inicio**: El primer inicio puede tardar 1-2 minutos mientras se descargan los modelos de embeddings
- **Uso de memoria**: El servicio requiere aproximadamente 2-4GB de RAM según los modelos cargados
- **Persistencia**: Los datos en `chroma_db` persisten entre reinicios del contenedor

---

## 🔄 Actualizar a una nueva versión

```bash
# Detener servicios
docker-compose down

# Actualizar código (git pull o copiar archivos)
git pull origin main

# Reconstruir imágenes
docker-compose build

# Iniciar servicios
docker-compose up -d

# Verificar logs
docker-compose logs -f
```

---

## 📚 Recursos Adicionales

- **Documentación de FastAPI**: https://fastapi.tiangolo.com/
- **Docker Docs**: https://docs.docker.com/
- **Google Gemini API**: https://ai.google.dev/docs
- **ChromaDB**: https://docs.trychroma.com/

---

## 📄 Licencia

[Tu licencia aquí]

---

## 👥 Contribuciones

Las contribuciones son bienvenidas. Por favor:

1. Fork el proyecto
2. Crea una rama para tu feature (`git checkout -b feature/AmazingFeature`)
3. Commit tus cambios (`git commit -m 'Add some AmazingFeature'`)
4. Push a la rama (`git push origin feature/AmazingFeature`)
5. Abre un Pull Request

---

**¿Preguntas?** Abre un issue en el repositorio o contacta al equipo de desarrollo.
