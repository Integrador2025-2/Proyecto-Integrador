# Script de prueba rápida para RAG-Service (Windows PowerShell)

$BaseURL = "http://localhost:8001"

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "   RAG-Service - Pruebas Rápidas" -ForegroundColor Cyan
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""

# Test 1: Health Check
Write-Host "Test 1: Health Check" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$BaseURL/health" -Method Get
    Write-Host "✓ Servicio respondiendo correctamente" -ForegroundColor Green
    $response | ConvertTo-Json
} catch {
    Write-Host "✗ Servicio no responde" -ForegroundColor Red
    Write-Host $_.Exception.Message -ForegroundColor Red
    exit 1
}
Write-Host ""

# Test 2: API Docs disponible
Write-Host "Test 2: Documentación de la API" -ForegroundColor Yellow
try {
    $response = Invoke-WebRequest -Uri "$BaseURL/docs" -Method Get -UseBasicParsing
    if ($response.StatusCode -eq 200) {
        Write-Host "✓ Documentación disponible en $BaseURL/docs" -ForegroundColor Green
    }
} catch {
    Write-Host "✗ Documentación no disponible" -ForegroundColor Red
}
Write-Host ""

# Test 3: Verificar que LLM está configurado
Write-Host "Test 3: Configuración de LLM" -ForegroundColor Yellow
try {
    $envVars = docker exec rag-service env
    if ($envVars -match "GEMINI_API_KEY") {
        Write-Host "✓ GEMINI_API_KEY configurada" -ForegroundColor Green
    } else {
        Write-Host "✗ GEMINI_API_KEY no configurada" -ForegroundColor Red
    }
} catch {
    Write-Host "! No se pudo verificar (contenedor puede no estar corriendo)" -ForegroundColor Yellow
}
Write-Host ""

# Test 4: Verificar volúmenes
Write-Host "Test 4: Volúmenes de datos" -ForegroundColor Yellow
if (Test-Path ".\chroma_db") {
    Write-Host "✓ Directorio chroma_db existe" -ForegroundColor Green
} else {
    Write-Host "✗ Directorio chroma_db no existe" -ForegroundColor Red
}

if (Test-Path ".\uploads") {
    Write-Host "✓ Directorio uploads existe" -ForegroundColor Green
} else {
    Write-Host "✗ Directorio uploads no existe" -ForegroundColor Red
}

if (Test-Path ".\generated_budgets") {
    Write-Host "✓ Directorio generated_budgets existe" -ForegroundColor Green
} else {
    Write-Host "✗ Directorio generated_budgets no existe" -ForegroundColor Red
}
Write-Host ""

# Test 5: Logs recientes
Write-Host "Test 5: Últimas 5 líneas de logs" -ForegroundColor Yellow
docker-compose logs --tail=5 rag-service
Write-Host ""

Write-Host "==================================================" -ForegroundColor Cyan
Write-Host "Pruebas completadas" -ForegroundColor Green
Write-Host "==================================================" -ForegroundColor Cyan
Write-Host ""
Write-Host "URLs útiles:" -ForegroundColor Cyan
Write-Host "  - API: $BaseURL"
Write-Host "  - Swagger Docs: $BaseURL/docs"
Write-Host "  - ReDoc: $BaseURL/redoc"
Write-Host ""
