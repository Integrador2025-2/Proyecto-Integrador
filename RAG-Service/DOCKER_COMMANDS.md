# 🚀 Comandos para Montar RAG-Service con Docker

Guía paso a paso con los comandos exactos para levantar el servicio.

---

## 📋 Prerrequisitos

- Docker Desktop instalado y corriendo
- Git (para clonar el repositorio)

---

## 🔧 Pasos de Instalación

### 1. Clonar el repositorio

```bash
git clone <url-del-repositorio>
cd RAG-Service
```

### 2. Configurar variables de entorno

**Windows (CMD/PowerShell):**
```powershell
copy .env.docker .env
```

**Linux/Mac:**
```bash
cp .env.docker .env
```

### 3. Editar el archivo .env

Abre el archivo `.env` con tu editor favorito y agrega tu API Key de Gemini:

**Windows:**
```powershell
notepad .env
```

**Linux/Mac:**
```bash
nano .env
# o
vim .env
```

Modifica esta línea:
```bash
GEMINI_API_KEY=tu_api_key_de_gemini_aqui
```

Reemplaza `tu_api_key_de_gemini_aqui` con tu API Key real.

📌 **Obtener API Key gratis:** https://aistudio.google.com/app/apikey

### 4. Construir y levantar el servicio

```bash
docker-compose up -d --build
```

**Explicación del comando:**
- `docker-compose`: Usa Docker Compose para orquestar servicios
- `up`: Inicia los servicios
- `-d`: Modo detached (background)
- `--build`: Construye las imágenes antes de iniciar

⏱️ **Tiempo estimado:** 3-5 minutos (la primera vez)

### 5. Verificar que está corriendo

```bash
docker ps
```

Deberías ver algo como:
```
CONTAINER ID   IMAGE                     COMMAND             STATUS
f71d57c61395   rag-service-rag-service   "python main.py"    Up 3 minutes (healthy)
```

### 6. Probar el servicio

```bash
curl http://localhost:8001/health
```

Respuesta esperada:
```json
{"status":"healthy","message":"RAG Budget Automation Service is running"}
```

---

## 🌐 Acceder al Servicio

Una vez levantado, el servicio está disponible en:

- **API**: http://localhost:8001
- **Documentación interactiva (Swagger)**: http://localhost:8001/docs
- **Documentación alternativa (ReDoc)**: http://localhost:8001/redoc

---

## 📊 Comandos de Gestión

### Ver logs en tiempo real

```bash
docker-compose logs -f
```

Presiona `Ctrl+C` para salir.

### Ver estado del contenedor

```bash
docker-compose ps
```

### Detener el servicio

```bash
docker-compose down
```

### Reiniciar el servicio

```bash
docker-compose restart
```

### Detener y eliminar todo (incluyendo volúmenes)

⚠️ **Esto eliminará los datos persistentes (base de datos, documentos subidos, etc.)**

```bash
docker-compose down -v
```

### Reconstruir después de cambios en el código

```bash
docker-compose up -d --build
```

### Ver logs de las últimas líneas

```bash
docker-compose logs --tail=50
```

### Ver uso de recursos

```bash
docker stats
```

---

## 🧪 Probar la API

### Health Check

```bash
curl http://localhost:8001/health
```

### Subir un documento

```bash
curl -X POST "http://localhost:8001/documents/upload" \
  -F "file=@tu_documento.pdf" \
  -F "project_id=1" \
  -F "document_type=project_document"
```

### Hacer una consulta

```bash
curl -X POST "http://localhost:8001/query" \
  -H "Content-Type: application/json" \
  -d '{
    "question": "¿Cuál es el objetivo del proyecto?",
    "project_id": 1,
    "top_k": 5
  }'
```

---

## 🐛 Solución de Problemas

### Error: Puerto 8001 ya en uso

```bash
# Windows
netstat -ano | findstr :8001

# Linux/Mac
lsof -i :8001

# Solución: Detener el proceso que usa el puerto o cambiar el puerto en docker-compose.yml
```

### Error: GEMINI_API_KEY no configurada

Verifica que el archivo `.env` existe y contiene la API Key:

```bash
# Windows
type .env | findstr GEMINI

# Linux/Mac
cat .env | grep GEMINI
```

### El contenedor se detiene inmediatamente

Ver los logs para identificar el error:

```bash
docker-compose logs
```

### Problemas de permisos (Linux)

```bash
sudo chown -R $USER:$USER chroma_db uploads generated_budgets
```

---

## 🔄 Actualizar a Nueva Versión

```bash
# 1. Detener servicio
docker-compose down

# 2. Actualizar código
git pull origin main

# 3. Reconstruir imagen
docker-compose build --no-cache

# 4. Iniciar nuevamente
docker-compose up -d

# 5. Verificar
curl http://localhost:8001/health
```

---

## 📦 Comandos Completos de Instalación (Copy-Paste)

**Para Windows (PowerShell):**

```powershell
# Clonar repositorio
git clone <url-del-repositorio>
cd RAG-Service

# Configurar
copy .env.docker .env
notepad .env
# [Agregar tu GEMINI_API_KEY y guardar]

# Iniciar
docker-compose up -d --build

# Verificar
docker ps
curl http://localhost:8001/health

# Ver documentación
start http://localhost:8001/docs
```

**Para Linux/Mac (Bash):**

```bash
# Clonar repositorio
git clone <url-del-repositorio>
cd RAG-Service

# Configurar
cp .env.docker .env
nano .env
# [Agregar tu GEMINI_API_KEY y guardar con Ctrl+X, Y, Enter]

# Iniciar
docker-compose up -d --build

# Verificar
docker ps
curl http://localhost:8001/health

# Ver documentación (Linux con xdg-open, Mac con open)
xdg-open http://localhost:8001/docs  # Linux
# o
open http://localhost:8001/docs      # Mac
```

---

## 📊 Resumen de Comandos Esenciales

| Acción | Comando |
|--------|---------|
| Iniciar servicio | `docker-compose up -d` |
| Detener servicio | `docker-compose down` |
| Ver logs | `docker-compose logs -f` |
| Ver estado | `docker-compose ps` |
| Reiniciar | `docker-compose restart` |
| Reconstruir | `docker-compose up -d --build` |
| Health check | `curl http://localhost:8001/health` |
| Abrir docs | http://localhost:8001/docs |

---

## 🆘 ¿Necesitas Ayuda?

1. **Ver documentación completa**: [DOCKER_README.md](DOCKER_README.md)
2. **Ver ejemplos de API**: [API_EXAMPLES.md](API_EXAMPLES.md)
3. **Configurar Gemini**: [GEMINI_SETUP.md](GEMINI_SETUP.md)

---

## ✅ Checklist de Instalación

- [ ] Docker Desktop instalado y corriendo
- [ ] Repositorio clonado
- [ ] Archivo `.env` creado y configurado con GEMINI_API_KEY
- [ ] `docker-compose up -d --build` ejecutado exitosamente
- [ ] `docker ps` muestra el contenedor corriendo (status: Up)
- [ ] `curl http://localhost:8001/health` responde con status "healthy"
- [ ] http://localhost:8001/docs abre la documentación

---

**¡Listo! Tu RAG-Service está corriendo en Docker 🎉**
