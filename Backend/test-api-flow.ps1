# Script para probar el flujo completo del Backend
# Ejecutar: powershell -File test-api-flow.ps1

$baseUrl = "http://localhost:5043/api"
$headers = @{"Content-Type" = "application/json"}

Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "PRUEBA DE FLUJO COMPLETO - BACKEND API" -ForegroundColor Cyan
Write-Host "========================================`n" -ForegroundColor Cyan

# Variables para almacenar IDs
$token = ""
$usuarioId = 0
$proyectoId = 0
$objetivoId = 0
$cadenaId = 0
$actividadId = 0
$tareaId = 0
$entidadId = 0

# 1. REGISTRO DE USUARIO
Write-Host "1️⃣  Registrando nuevo usuario..." -ForegroundColor Yellow
$registerBody = @{
    email = "test@minciencias.com"
    password = "Password123!"
    firstName = "Juan"
    lastName = "Pérez"
} | ConvertTo-Json

try {
    $registerResponse = Invoke-RestMethod -Uri "$baseUrl/auth/register" -Method Post -Headers $headers -Body $registerBody -ErrorAction Stop
    Write-Host "   ✅ Usuario registrado exitosamente" -ForegroundColor Green
    Write-Host "   📧 Email: $($registerResponse.email)" -ForegroundColor Gray
} catch {
    if ($_.Exception.Response.StatusCode -eq 400) {
        Write-Host "   ⚠️  Usuario ya existe, continuando con login..." -ForegroundColor Yellow
    } else {
        Write-Host "   ❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Start-Sleep -Seconds 1

# 2. LOGIN
Write-Host "`n2️⃣  Iniciando sesión..." -ForegroundColor Yellow
$loginBody = @{
    email = "test@minciencias.com"
    password = "Password123!"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/auth/login" -Method Post -Headers $headers -Body $loginBody -ErrorAction Stop
    $token = $loginResponse.token
    $usuarioId = $loginResponse.userId
    Write-Host "   ✅ Login exitoso" -ForegroundColor Green
    Write-Host "   🔑 Token obtenido (primeros 50 chars): $($token.Substring(0,50))..." -ForegroundColor Gray
    Write-Host "   👤 Usuario ID: $usuarioId" -ForegroundColor Gray
    
    # Actualizar headers con el token
    $headers["Authorization"] = "Bearer $token"
} catch {
    Write-Host "   ❌ Error en login: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Start-Sleep -Seconds 1

# 3. CREAR PROYECTO
Write-Host "`n3️⃣  Creando proyecto..." -ForegroundColor Yellow
$proyectoBody = @{
    usuarioId = $usuarioId
    nombre = "Proyecto de Investigación IA - 2025"
    descripcion = "Investigación sobre Inteligencia Artificial aplicada a MinCiencias"
    estado = "En Progreso"
    fechaInicio = "2025-01-01T00:00:00"
    fechaFin = "2025-12-31T23:59:59"
} | ConvertTo-Json

try {
    $proyectoResponse = Invoke-RestMethod -Uri "$baseUrl/proyectos" -Method Post -Headers $headers -Body $proyectoBody -ErrorAction Stop
    $proyectoId = $proyectoResponse.proyectoId
    Write-Host "   ✅ Proyecto creado exitosamente" -ForegroundColor Green
    Write-Host "   📁 ID: $proyectoId" -ForegroundColor Gray
    Write-Host "   📝 Nombre: $($proyectoResponse.nombre)" -ForegroundColor Gray
} catch {
    Write-Host "   ❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "   Detalles: $($_.ErrorDetails.Message)" -ForegroundColor Red
    exit 1
}

Start-Sleep -Seconds 1

# 4. CREAR OBJETIVO
Write-Host "`n4️⃣  Creando objetivo..." -ForegroundColor Yellow
$objetivoBody = @{
    proyectoId = $proyectoId
    nombre = "Objetivo General del Proyecto"
    descripcion = "Desarrollar un sistema de IA robusto y escalable"
    resultadoEsperado = "Sistema de IA implementado y funcionando con 95% de precisión"
} | ConvertTo-Json

try {
    $objetivoResponse = Invoke-RestMethod -Uri "$baseUrl/objetivos" -Method Post -Headers $headers -Body $objetivoBody -ErrorAction Stop
    $objetivoId = $objetivoResponse.objetivoId
    Write-Host "   ✅ Objetivo creado exitosamente" -ForegroundColor Green
    Write-Host "   🎯 ID: $objetivoId" -ForegroundColor Gray
    Write-Host "   📝 Nombre: $($objetivoResponse.nombre)" -ForegroundColor Gray
} catch {
    Write-Host "   ❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Start-Sleep -Seconds 1

# 5. CREAR CADENA DE VALOR
Write-Host "`n5️⃣  Creando cadena de valor..." -ForegroundColor Yellow
$cadenaBody = @{
    objetivoId = $objetivoId
    nombre = "Cadena de Investigación y Desarrollo"
    objetivoEspecifico = "Implementar módulo de Machine Learning con TensorFlow"
} | ConvertTo-Json

try {
    $cadenaResponse = Invoke-RestMethod -Uri "$baseUrl/cadenasdevalor" -Method Post -Headers $headers -Body $cadenaBody -ErrorAction Stop
    $cadenaId = $cadenaResponse.cadenaDeValorId
    Write-Host "   ✅ Cadena de valor creada exitosamente" -ForegroundColor Green
    Write-Host "   🔗 ID: $cadenaId" -ForegroundColor Gray
    Write-Host "   📝 Nombre: $($cadenaResponse.nombre)" -ForegroundColor Gray
} catch {
    Write-Host "   ❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Start-Sleep -Seconds 1

# 6. CREAR ACTIVIDAD
Write-Host "`n6️⃣  Creando actividad..." -ForegroundColor Yellow
$actividadBody = @{
    cadenaDeValorId = $cadenaId
    nombre = "Recolección y Procesamiento de Datos"
    descripcion = "Recopilar, limpiar y preparar datasets para entrenamiento del modelo"
    duracionAnios = 1
    valorUnitario = 15000000
} | ConvertTo-Json

try {
    $actividadResponse = Invoke-RestMethod -Uri "$baseUrl/actividades" -Method Post -Headers $headers -Body $actividadBody -ErrorAction Stop
    $actividadId = $actividadResponse.actividadId
    Write-Host "   ✅ Actividad creada exitosamente" -ForegroundColor Green
    Write-Host "   ⚡ ID: $actividadId" -ForegroundColor Gray
    Write-Host "   📝 Nombre: $($actividadResponse.nombre)" -ForegroundColor Gray
    Write-Host "   💰 Valor: $([math]::Round($actividadResponse.valorUnitario, 0))" -ForegroundColor Gray
} catch {
    Write-Host "   ❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Start-Sleep -Seconds 1

# 7. CREAR TAREA
Write-Host "`n7️⃣  Creando tarea..." -ForegroundColor Yellow
$tareaBody = @{
    actividadId = $actividadId
    nombre = "Análisis Exploratorio de Datos"
    descripcion = "Realizar análisis estadístico y visualización de datos recolectados"
    periodo = "2025-Q1"
    monto = 5000000
} | ConvertTo-Json

try {
    $tareaResponse = Invoke-RestMethod -Uri "$baseUrl/tareas" -Method Post -Headers $headers -Body $tareaBody -ErrorAction Stop
    $tareaId = $tareaResponse.tareaId
    Write-Host "   ✅ Tarea creada exitosamente" -ForegroundColor Green
    Write-Host "   ✅ ID: $tareaId" -ForegroundColor Gray
    Write-Host "   📝 Nombre: $($tareaResponse.nombre)" -ForegroundColor Gray
    Write-Host "   💰 Monto: $([math]::Round($tareaResponse.monto, 0))" -ForegroundColor Gray
} catch {
    Write-Host "   ❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Start-Sleep -Seconds 1

# 8. CREAR ENTIDAD
Write-Host "`n8️⃣  Creando entidad participante..." -ForegroundColor Yellow
$entidadBody = @{
    nombre = "Universidad Nacional de Colombia"
} | ConvertTo-Json

try {
    $entidadResponse = Invoke-RestMethod -Uri "$baseUrl/entidades" -Method Post -Headers $headers -Body $entidadBody -ErrorAction Stop
    $entidadId = $entidadResponse.entidadId
    Write-Host "   ✅ Entidad creada exitosamente" -ForegroundColor Green
    Write-Host "   🏢 ID: $entidadId" -ForegroundColor Gray
    Write-Host "   📝 Nombre: $($entidadResponse.nombre)" -ForegroundColor Gray
} catch {
    Write-Host "   ❌ Error: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

Start-Sleep -Seconds 1

# 9. VERIFICAR DATOS - OBTENER PROYECTO COMPLETO
Write-Host "`n9️⃣  Verificando datos creados..." -ForegroundColor Yellow

try {
    # Obtener proyecto
    $proyecto = Invoke-RestMethod -Uri "$baseUrl/proyectos/$proyectoId" -Method Get -Headers $headers -ErrorAction Stop
    Write-Host "   ✅ Proyecto verificado: $($proyecto.nombre)" -ForegroundColor Green
    
    # Obtener objetivos del proyecto
    $objetivos = Invoke-RestMethod -Uri "$baseUrl/objetivos/proyecto/$proyectoId" -Method Get -Headers $headers -ErrorAction Stop
    Write-Host "   ✅ Objetivos encontrados: $($objetivos.Count)" -ForegroundColor Green
    
    # Obtener cadenas del objetivo
    $cadenas = Invoke-RestMethod -Uri "$baseUrl/cadenasdevalor/objetivo/$objetivoId" -Method Get -Headers $headers -ErrorAction Stop
    Write-Host "   ✅ Cadenas de valor encontradas: $($cadenas.Count)" -ForegroundColor Green
    
    # Obtener actividades de la cadena
    $actividades = Invoke-RestMethod -Uri "$baseUrl/actividades/cadena/$cadenaId" -Method Get -Headers $headers -ErrorAction Stop
    Write-Host "   ✅ Actividades encontradas: $($actividades.Count)" -ForegroundColor Green
    
    # Obtener tareas de la actividad
    $tareas = Invoke-RestMethod -Uri "$baseUrl/tareas/actividad/$actividadId" -Method Get -Headers $headers -ErrorAction Stop
    Write-Host "   ✅ Tareas encontradas: $($tareas.Count)" -ForegroundColor Green
    
} catch {
    Write-Host "   ❌ Error en verificación: $($_.Exception.Message)" -ForegroundColor Red
}

# RESUMEN FINAL
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "RESUMEN DEL FLUJO COMPLETADO" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "👤 Usuario ID: $usuarioId" -ForegroundColor White
Write-Host "📁 Proyecto ID: $proyectoId - $($proyecto.nombre)" -ForegroundColor White
Write-Host "🎯 Objetivo ID: $objetivoId - $($objetivoResponse.nombre)" -ForegroundColor White
Write-Host "🔗 Cadena ID: $cadenaId - $($cadenaResponse.nombre)" -ForegroundColor White
Write-Host "⚡ Actividad ID: $actividadId - $($actividadResponse.nombre)" -ForegroundColor White
Write-Host "✅ Tarea ID: $tareaId - $($tareaResponse.nombre)" -ForegroundColor White
Write-Host "🏢 Entidad ID: $entidadId - $($entidadResponse.nombre)" -ForegroundColor White
Write-Host "`n✅ FLUJO COMPLETADO EXITOSAMENTE! 🎉" -ForegroundColor Green
Write-Host "========================================`n" -ForegroundColor Cyan

Write-Host "💡 Próximos pasos:" -ForegroundColor Yellow
Write-Host "   - Abre Swagger UI en: http://localhost:5043" -ForegroundColor Gray
Write-Host "   - Explora los endpoints creados" -ForegroundColor Gray
Write-Host "   - Prueba los endpoints de consulta (GET)" -ForegroundColor Gray
Write-Host "   - Crea recursos, cronogramas, etc." -ForegroundColor Gray
