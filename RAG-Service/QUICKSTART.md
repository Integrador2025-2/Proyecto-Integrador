# 🚀 Inicio Rápido - RAG Service

Guía de 5 minutos para poner en marcha el RAG-Service con Docker.

## Requisitos

- ✅ Docker Desktop instalado ([Descargar aquí](https://www.docker.com/products/docker-desktop))
- ✅ API Key de Google Gemini ([Obtener gratis aquí](https://aistudio.google.com/app/apikey))

## Pasos

### 1️⃣ Descargar el código

```bash
git clone <url-del-repositorio>
cd RAG-Service
```

### 2️⃣ Configurar API Key

**Windows:**
```powershell
copy .env.docker .env
notepad .env
```

**Linux/Mac:**
```bash
cp .env.docker .env
nano .env
```

Edita el archivo `.env` y pega tu API Key:

```bash
GEMINI_API_KEY=AIzaSy...tu_api_key_aqui
```

### 3️⃣ Iniciar el servicio

**Windows:**
```powershell
.\docker-manage.ps1 start
```

**Linux/Mac:**
```bash
chmod +x docker-manage.sh
./docker-manage.sh start
```

**O directamente con Docker Compose:**
```bash
docker-compose up -d
```

### 4️⃣ Verificar que funciona

Abre tu navegador en: **http://localhost:8001/docs**

O ejecuta:
```bash
curl http://localhost:8001/health
```

## 🎉 ¡Listo!

Tu servicio está funcionando. Ahora puedes:

- 📖 **Ver la API**: http://localhost:8001/docs
- 💻 **Usar Streamlit**: http://localhost:8501 (si lo iniciaste)
- 📚 **Leer documentación completa**: [DOCKER_README.md](DOCKER_README.md)

## 🧪 Prueba rápida

### Subir un documento

```bash
curl -X POST "http://localhost:8001/documents/upload" \
  -F "file=@tu_documento.pdf" \
  -F "project_id=1"
```

### Hacer una pregunta

```bash
curl -X POST "http://localhost:8001/query" \
  -H "Content-Type: application/json" \
  -d '{
    "question": "¿Cuál es el objetivo del proyecto?",
    "project_id": 1
  }'
```

## ⚙️ Comandos útiles

```bash
# Ver logs
docker-compose logs -f

# Detener servicio
docker-compose stop

# Reiniciar servicio
docker-compose restart

# Ver estado
docker-compose ps
```

## 🆘 ¿Problemas?

### El servicio no inicia
```bash
# Ver errores
docker-compose logs rag-service

# Verificar que Docker está corriendo
docker ps
```

### No encuentras tu API Key
1. Ve a https://aistudio.google.com/app/apikey
2. Inicia sesión con Google
3. Haz clic en "Create API Key"
4. Copia y pega en `.env`

### Puerto 8001 ya en uso
```bash
# Ver qué está usando el puerto (Windows)
netstat -ano | findstr :8001

# Ver qué está usando el puerto (Linux/Mac)
lsof -i :8001

# Cambiar puerto en docker-compose.yml
# Edita: "8002:8001" en lugar de "8001:8001"
```

## 📚 Siguiente paso

Lee la guía completa: [DOCKER_README.md](DOCKER_README.md)

## 💡 Tip

Si usas Windows, corre PowerShell como Administrador para mejor compatibilidad.
