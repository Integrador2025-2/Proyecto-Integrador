# Script de gestión de Docker para RAG-Service (Windows PowerShell)
# Uso: .\docker-manage.ps1 [comando]

param(
    [Parameter(Position=0)]
    [string]$Command = "help",
    [Parameter(Position=1)]
    [string]$Service = "rag-service"
)

# Funciones de ayuda con colores
function Print-Success {
    param([string]$Message)
    Write-Host "✓ $Message" -ForegroundColor Green
}

function Print-Error {
    param([string]$Message)
    Write-Host "✗ $Message" -ForegroundColor Red
}

function Print-Info {
    param([string]$Message)
    Write-Host "ℹ $Message" -ForegroundColor Yellow
}

# Verificar que Docker está instalado
function Check-Docker {
    try {
        $dockerVersion = docker --version
        $composeVersion = docker-compose --version
        Print-Success "Docker y Docker Compose están instalados"
        return $true
    } catch {
        Print-Error "Docker no está instalado. Por favor, instala Docker Desktop primero."
        return $false
    }
}

# Verificar archivo .env
function Check-Env {
    if (-not (Test-Path ".env")) {
        Print-Info "Archivo .env no encontrado. Creando desde .env.docker..."
        Copy-Item ".env.docker" ".env"
        Print-Error "Por favor, edita el archivo .env con tus API keys antes de continuar."
        return $false
    }
    Print-Success "Archivo .env encontrado"
    return $true
}

# Construir imágenes
function Build-Images {
    Print-Info "Construyendo imágenes Docker..."
    docker-compose build --no-cache
    if ($LASTEXITCODE -eq 0) {
        Print-Success "Imágenes construidas exitosamente"
    } else {
        Print-Error "Error al construir imágenes"
    }
}

# Iniciar servicios
function Start-Services {
    Print-Info "Iniciando servicios..."
    docker-compose up -d
    
    if ($LASTEXITCODE -eq 0) {
        Print-Success "Servicios iniciados"
        
        Print-Info "Esperando a que los servicios estén listos..."
        Start-Sleep -Seconds 5
        
        docker-compose ps
        
        Print-Success "Servicios disponibles en:"
        Write-Host "  - API: http://localhost:8001" -ForegroundColor Cyan
        Write-Host "  - Docs: http://localhost:8001/docs" -ForegroundColor Cyan
        Write-Host "  - Streamlit: http://localhost:8501" -ForegroundColor Cyan
    } else {
        Print-Error "Error al iniciar servicios"
    }
}

# Detener servicios
function Stop-Services {
    Print-Info "Deteniendo servicios..."
    docker-compose stop
    Print-Success "Servicios detenidos"
}

# Reiniciar servicios
function Restart-Services {
    Print-Info "Reiniciando servicios..."
    docker-compose restart
    Print-Success "Servicios reiniciados"
}

# Ver logs
function Show-Logs {
    if ($Service -eq "rag-service" -and $args.Count -eq 0) {
        docker-compose logs -f
    } else {
        docker-compose logs -f $Service
    }
}

# Ver estado
function Show-Status {
    Print-Info "Estado de los servicios:"
    docker-compose ps
    
    Write-Host ""
    Print-Info "Health check del servicio RAG:"
    try {
        $response = Invoke-RestMethod -Uri "http://localhost:8001/health" -Method Get
        $response | ConvertTo-Json
        Print-Success "Servicio funcionando correctamente"
    } catch {
        Print-Error "Servicio no disponible"
    }
}

# Limpiar todo
function Clean-All {
    $response = Read-Host "¿Estás seguro de que quieres eliminar todos los contenedores y volúmenes? (y/n)"
    if ($response -match '^[Yy]') {
        Print-Info "Limpiando contenedores y volúmenes..."
        docker-compose down -v
        Print-Success "Limpieza completada"
    } else {
        Print-Info "Operación cancelada"
    }
}

# Entrar al contenedor
function Enter-Shell {
    Print-Info "Accediendo al shell del contenedor $Service..."
    docker exec -it $Service bash
}

# Ver uso de recursos
function Show-Stats {
    docker stats
}

# Backup de datos
function Backup-Data {
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $backupDir = ".\backups\$timestamp"
    
    Print-Info "Creando backup en $backupDir..."
    New-Item -ItemType Directory -Path $backupDir -Force | Out-Null
    
    Copy-Item -Recurse "chroma_db" "$backupDir\"
    Copy-Item -Recurse "uploads" "$backupDir\"
    Copy-Item -Recurse "generated_budgets" "$backupDir\"
    
    Print-Success "Backup completado en $backupDir"
}

# Restaurar desde backup
function Restore-Data {
    param([string]$BackupDir)
    
    if (-not $BackupDir) {
        Print-Error "Uso: .\docker-manage.ps1 restore <directorio_backup>"
        return
    }
    
    if (-not (Test-Path $BackupDir)) {
        Print-Error "Directorio de backup no encontrado: $BackupDir"
        return
    }
    
    Print-Info "Restaurando desde $BackupDir..."
    Stop-Services
    
    Remove-Item -Recurse -Force "chroma_db", "uploads", "generated_budgets" -ErrorAction SilentlyContinue
    Copy-Item -Recurse "$BackupDir\chroma_db" "."
    Copy-Item -Recurse "$BackupDir\uploads" "."
    Copy-Item -Recurse "$BackupDir\generated_budgets" "."
    
    Start-Services
    Print-Success "Restauración completada"
}

# Actualizar
function Update-Service {
    Print-Info "Actualizando servicio..."
    Stop-Services
    Build-Images
    Start-Services
    Print-Success "Actualización completada"
}

# Menú de ayuda
function Show-Help {
    Write-Host "RAG-Service Docker Management Script (Windows)" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "Uso: .\docker-manage.ps1 [comando] [servicio]" -ForegroundColor White
    Write-Host ""
    Write-Host "Comandos disponibles:" -ForegroundColor Yellow
    Write-Host "  check       - Verificar requisitos de Docker"
    Write-Host "  build       - Construir imágenes Docker"
    Write-Host "  start       - Iniciar servicios"
    Write-Host "  stop        - Detener servicios"
    Write-Host "  restart     - Reiniciar servicios"
    Write-Host "  logs [svc]  - Ver logs (opcionalmente de un servicio específico)"
    Write-Host "  status      - Ver estado de los servicios"
    Write-Host "  clean       - Eliminar contenedores y volúmenes"
    Write-Host "  shell [svc] - Acceder al shell del contenedor"
    Write-Host "  stats       - Ver uso de recursos"
    Write-Host "  backup      - Hacer backup de los datos"
    Write-Host "  restore     - Restaurar desde backup"
    Write-Host "  update      - Actualizar servicio (stop, build, start)"
    Write-Host "  help        - Mostrar esta ayuda"
    Write-Host ""
    Write-Host "Ejemplos:" -ForegroundColor Cyan
    Write-Host "  .\docker-manage.ps1 start                           # Iniciar todos los servicios"
    Write-Host "  .\docker-manage.ps1 logs rag-service                # Ver logs del servicio RAG"
    Write-Host "  .\docker-manage.ps1 shell rag-service               # Acceder al shell"
    Write-Host "  .\docker-manage.ps1 restore .\backups\20231218_120000  # Restaurar backup"
}

# Comando principal
switch ($Command.ToLower()) {
    "check" {
        $dockerOk = Check-Docker
        $envOk = Check-Env
        if (-not ($dockerOk -and $envOk)) {
            exit 1
        }
    }
    "build" {
        if (-not (Check-Docker)) { exit 1 }
        if (-not (Check-Env)) { exit 1 }
        Build-Images
    }
    "start" {
        if (-not (Check-Docker)) { exit 1 }
        if (-not (Check-Env)) { exit 1 }
        Start-Services
    }
    "stop" {
        Stop-Services
    }
    "restart" {
        Restart-Services
    }
    "logs" {
        Show-Logs
    }
    "status" {
        Show-Status
    }
    "clean" {
        Clean-All
    }
    "shell" {
        Enter-Shell
    }
    "stats" {
        Show-Stats
    }
    "backup" {
        Backup-Data
    }
    "restore" {
        Restore-Data -BackupDir $Service
    }
    "update" {
        Update-Service
    }
    "help" {
        Show-Help
    }
    default {
        Print-Error "Comando desconocido: $Command"
        Write-Host ""
        Show-Help
        exit 1
    }
}
