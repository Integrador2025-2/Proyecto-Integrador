# Script de instalación automática de RAG-Service con Docker (Windows)
# Ejecutar con: .\install-docker.ps1

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "   RAG-Service - Instalación Automática" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

function Print-Step {
    param([string]$Message)
    Write-Host "▶ $Message" -ForegroundColor Yellow
}

function Print-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Print-Error {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

# 1. Verificar Docker
Print-Step "Paso 1/6: Verificando Docker..."
try {
    $dockerVersion = docker --version
    $composeVersion = docker-compose --version
    Print-Success "Docker instalado"
    Write-Host $dockerVersion
    Write-Host $composeVersion
} catch {
    Print-Error "Docker no está instalado"
    Write-Host ""
    Write-Host "Por favor, instala Docker Desktop desde:"
    Write-Host "  https://www.docker.com/products/docker-desktop" -ForegroundColor Cyan
    exit 1
}
Write-Host ""

# 2. Verificar archivo .env
Print-Step "Paso 2/6: Configurando variables de entorno..."
if (-not (Test-Path ".env")) {
    Copy-Item ".env.docker" ".env"
    Print-Success "Archivo .env creado desde .env.docker"
    
    Write-Host ""
    Write-Host "IMPORTANTE: Necesitas configurar tu API Key" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "1. Obtén tu API Key de Google Gemini (gratis):"
    Write-Host "   https://aistudio.google.com/app/apikey" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "2. Edita el archivo .env y reemplaza:"
    Write-Host "   GEMINI_API_KEY=tu_api_key_de_gemini_aqui"
    Write-Host ""
    
    $hasKey = Read-Host "¿Ya tienes tu API Key? (s/n)"
    
    if ($hasKey -match '^[Ss]$') {
        $apiKey = Read-Host "Ingresa tu GEMINI_API_KEY"
        if ($apiKey) {
            # Reemplazar en el archivo .env
            $content = Get-Content ".env"
            $content = $content -replace "GEMINI_API_KEY=.*", "GEMINI_API_KEY=$apiKey"
            Set-Content ".env" $content
            Print-Success "API Key configurada"
        }
    } else {
        Print-Error "Necesitas configurar la API Key en .env antes de continuar"
        Write-Host "Ejecuta: notepad .env"
        Write-Host "Luego ejecuta este script nuevamente"
        exit 1
    }
} else {
    Print-Success "Archivo .env ya existe"
}
Write-Host ""

# 3. Crear directorios necesarios
Print-Step "Paso 3/6: Creando directorios..."
New-Item -ItemType Directory -Force -Path "chroma_db", "uploads", "generated_budgets" | Out-Null
Print-Success "Directorios creados"
Write-Host ""

# 4. Configuración de permisos (no necesario en Windows)
Print-Step "Paso 4/6: Configurando permisos..."
Print-Success "Permisos configurados (Windows)"
Write-Host ""

# 5. Construir imágenes Docker
Print-Step "Paso 5/6: Construyendo imágenes Docker..."
Write-Host "Esto puede tardar varios minutos la primera vez..." -ForegroundColor Yellow
Write-Host ""

try {
    docker-compose build
    Print-Success "Imágenes construidas exitosamente"
} catch {
    Print-Error "Error al construir imágenes"
    exit 1
}
Write-Host ""

# 6. Iniciar servicios
Print-Step "Paso 6/6: Iniciando servicios..."
try {
    docker-compose up -d
    Print-Success "Servicios iniciados"
} catch {
    Print-Error "Error al iniciar servicios"
    exit 1
}
Write-Host ""

# Esperar a que el servicio esté listo
Print-Step "Esperando a que el servicio esté listo..."
Start-Sleep -Seconds 10

# Verificar health check
$maxAttempts = 12
$attempt = 0
$ready = $false

while ($attempt -lt $maxAttempts) {
    try {
        $response = Invoke-RestMethod -Uri "http://localhost:8001/health" -Method Get -ErrorAction Stop
        Print-Success "Servicio está listo"
        $ready = $true
        break
    } catch {
        $attempt++
        Write-Host "Intento $attempt/$maxAttempts..."
        Start-Sleep -Seconds 5
    }
}

if (-not $ready) {
    Print-Error "El servicio no respondió después de varios intentos"
    Write-Host "Verifica los logs con: docker-compose logs rag-service"
    exit 1
}
Write-Host ""

# Mostrar estado
Write-Host "==================================================" -ForegroundColor Green
Write-Host "   ¡Instalación Completada!" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Green
Write-Host ""
Write-Host "Tu RAG-Service está funcionando correctamente 🎉" -ForegroundColor Green
Write-Host ""
Write-Host "URLs disponibles:" -ForegroundColor Cyan
Write-Host "  📖 API Docs:    http://localhost:8001/docs"
Write-Host "  🌐 API:         http://localhost:8001"
Write-Host "  💻 Streamlit:   http://localhost:8501"
Write-Host ""
Write-Host "Comandos útiles:" -ForegroundColor Cyan
Write-Host "  .\docker-manage.ps1 status   - Ver estado"
Write-Host "  .\docker-manage.ps1 logs     - Ver logs"
Write-Host "  .\docker-manage.ps1 stop     - Detener"
Write-Host "  .\docker-manage.ps1 restart  - Reiniciar"
Write-Host "  .\docker-manage.ps1 help     - Ver todos los comandos"
Write-Host ""
Write-Host "Próximos pasos:" -ForegroundColor Cyan
Write-Host "  1. Abre http://localhost:8001/docs en tu navegador"
Write-Host "  2. Prueba el endpoint /health"
Write-Host "  3. Lee API_EXAMPLES.md para ejemplos de uso"
Write-Host "  4. Lee DOCKER_README.md para documentación completa"
Write-Host ""
Write-Host "Tip:" -ForegroundColor Yellow -NoNewline
Write-Host " Ejecuta '.\test-docker.ps1' para hacer pruebas automáticas"
Write-Host ""

# Ejecutar test automático si el usuario quiere
$runTest = Read-Host "¿Quieres ejecutar las pruebas automáticas ahora? (s/n)"
if ($runTest -match '^[Ss]$') {
    Write-Host ""
    & .\test-docker.ps1
}

Write-Host ""
Write-Host "¡Disfruta usando RAG-Service! 🚀" -ForegroundColor Green
Write-Host ""
