# Guía de Ejecución del Servicio RAG

Esta guía te explica cómo ejecutar el servicio RAG tanto de forma local como con Docker.

## 📋 Requisitos Previos

### Para ejecución local:
- Python 3.11 o superior
- pip (gestor de paquetes de Python)
- Git (opcional, solo si clonas el repositorio)

### Para ejecución con Docker:
- Docker Desktop instalado y funcionando
- Docker Compose (incluido en Docker Desktop)

---

## 🚀 Opción 1: Ejecutar Localmente (Desarrollo)

### Paso 1: Navegar al directorio del servicio

```bash
cd RAG-Service
```

### Paso 2: Crear entorno virtual (Recomendado)

**En Windows:**
```bash
python -m venv venv
venv\Scripts\activate
```

**En Linux/Mac:**
```bash
python3 -m venv venv
source venv/bin/activate
```

### Paso 3: Instalar dependencias

```bash
pip install -r requirements.txt
```

⚠️ **Nota**: La primera instalación puede tardar varios minutos porque descarga el modelo de embeddings.

### Paso 4: Configurar variables de entorno

Crea un archivo `.env` basado en `env.example`:

```bash
# En Windows (PowerShell)
Copy-Item env.example .env

# En Linux/Mac
cp env.example .env
```

Edita el archivo `.env` con tus configuraciones. **IMPORTANTE**: El servicio puede funcionar sin `OPENAI_API_KEY` para búsqueda semántica, pero lo necesitarás si quieres usar generación avanzada.

### Paso 5: Crear directorios necesarios

```bash
# En Windows
mkdir chroma_db uploads generated_budgets

# En Linux/Mac
mkdir -p chroma_db uploads generated_budgets
```

### Paso 6: Ejecutar el servicio

```bash
uvicorn main:app --host 0.0.0.0 --port 8001 --reload
```

El flag `--reload` permite que el servidor se recargue automáticamente cuando cambies el código (útil para desarrollo).

### Paso 7: Verificar que funciona

Abre tu navegador y visita:
- **API Base**: http://localhost:8001
- **Documentación Swagger**: http://localhost:8001/docs
- **Health Check**: http://localhost:8001/health

Deberías ver una respuesta JSON con `{"status": "healthy", "message": "RAG Budget Automation Service is running"}`

---

## 🐳 Opción 2: Ejecutar con Docker (Recomendado para Producción)

### Paso 1: Ubicarse en la raíz del proyecto

```bash
cd ..  # Desde RAG-Service, volver a la raíz
# O si estás en la raíz:
# cd Proyecto-Integrador
```

### Paso 2: Configurar variables de entorno (opcional)

Si quieres usar una API key de OpenAI, puedes crear un archivo `.env` en la raíz del proyecto:

```env
OPENAI_API_KEY=tu_api_key_aqui
```

### Paso 3: Ejecutar solo el servicio RAG

```bash
docker-compose up rag-service
```

O si quieres ejecutarlo en segundo plano:

```bash
docker-compose up -d rag-service
```

### Paso 4: Ejecutar todos los servicios (RAG + Backend + Frontend)

```bash
docker-compose up
```

O en segundo plano:

```bash
docker-compose up -d
```

### Paso 5: Verificar que funciona

Visita: http://localhost:8001/health

### Ver logs del servicio

```bash
docker-compose logs -f rag-service
```

### Detener el servicio

```bash
# Solo el servicio RAG
docker-compose stop rag-service

# Todos los servicios
docker-compose down
```

---

## 🔧 Configuración de Puertos

El servicio RAG utiliza por defecto el puerto **8001**. Si necesitas cambiarlo:

### En ejecución local:
Modifica el comando:
```bash
uvicorn main:app --host 0.0.0.0 --port 8080 --reload
```

### En Docker:
Edita `docker-compose.yml`:
```yaml
rag-service:
  ports:
    - "8080:8001"  # Formato: "puerto_host:puerto_contenedor"
```

---

## 🧪 Probar el Servicio

### 1. Health Check
```bash
curl http://localhost:8001/health
```

### 2. Subir un documento (ejemplo con curl)
```bash
curl -X POST "http://localhost:8001/documents/upload" \
  -F "file=@ruta/a/tu/documento.pdf" \
  -F "project_id=1" \
  -F "document_type=project_document"
```

### 3. Realizar una consulta
```bash
curl -X POST "http://localhost:8001/query" \
  -H "Content-Type: application/json" \
  -d '{
    "question": "¿Cuáles son los costos estimados?",
    "project_id": 1,
    "top_k": 5
  }'
```

### 4. Usar la interfaz Swagger
Visita http://localhost:8001/docs para una interfaz interactiva donde puedes probar todos los endpoints.

---

## 🐛 Solución de Problemas

### Error: "No module named 'sentence_transformers'"
**Solución**: Asegúrate de haber activado el entorno virtual e instalado las dependencias:
```bash
pip install -r requirements.txt
```

### Error: "Port 8001 is already in use"
**Solución**: 
1. Encuentra el proceso que usa el puerto:
   ```bash
   # Windows
   netstat -ano | findstr :8001
   
   # Linux/Mac
   lsof -i :8001
   ```
2. Detén el proceso o cambia el puerto

### Error: "ChromaDB connection failed"
**Solución**: Verifica que tengas permisos de escritura en el directorio `chroma_db`:
```bash
# En Linux/Mac
chmod -R 755 chroma_db
```

### Error: Modelo no descarga
**Solución**: El modelo se descarga automáticamente la primera vez. Asegúrate de tener conexión a internet. Si falla, puedes descargarlo manualmente:
```python
from sentence_transformers import SentenceTransformer
model = SentenceTransformer('paraphrase-multilingual-MiniLM-L12-v2')
```

### El servicio inicia pero no responde
**Solución**: 
1. Verifica los logs: `docker-compose logs rag-service`
2. Asegúrate de que el puerto esté correctamente mapeado
3. Verifica que no haya un firewall bloqueando el puerto

---

## 📊 Monitoreo

### Ver uso de recursos (Docker)
```bash
docker stats proyecto_integrador_rag
```

### Ver logs en tiempo real
```bash
docker-compose logs -f rag-service
```

### Acceder al contenedor
```bash
docker exec -it proyecto_integrador_rag bash
```

---

## 🔄 Reiniciar el Servicio

### Local:
Simplemente presiona `Ctrl+C` y vuelve a ejecutar el comando.

### Docker:
```bash
docker-compose restart rag-service
```

---

## 📝 Notas Adicionales

1. **Primera ejecución**: La primera vez que ejecutas el servicio, descargará el modelo de embeddings (~420 MB). Esto puede tardar varios minutos dependiendo de tu conexión.

2. **Base de datos ChromaDB**: Los datos se almacenan localmente en `chroma_db/`. En Docker, estos datos persisten en un volumen.

3. **Archivos subidos**: Los archivos procesados se guardan temporalmente. En producción, considera configurar un límite de tamaño.

4. **Desarrollo vs Producción**: Para desarrollo usa `--reload`. Para producción, omite este flag y considera usar un servidor ASGI como Gunicorn con Uvicorn workers.

---

## ✅ Checklist de Verificación

- [ ] Python 3.11+ instalado
- [ ] Entorno virtual creado y activado (si ejecutas localmente)
- [ ] Dependencias instaladas (`pip install -r requirements.txt`)
- [ ] Archivo `.env` configurado (opcional)
- [ ] Directorios creados (`chroma_db`, `uploads`, `generated_budgets`)
- [ ] Servicio ejecutándose en http://localhost:8001
- [ ] Health check responde correctamente
- [ ] Swagger UI accesible en http://localhost:8001/docs

---

