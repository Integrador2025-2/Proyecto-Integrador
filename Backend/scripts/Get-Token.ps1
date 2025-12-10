# Script PowerShell para obtener token JWT de prueba
# Requiere que el backend esté corriendo en http://localhost:5043

$ApiUrl = "http://localhost:5043/api"
$Email = "test@example.com"
$Password = "Test123!"

Write-Host "==========================================" -ForegroundColor Cyan
Write-Host "  Generador de Token de Prueba" -ForegroundColor Cyan
Write-Host "  (Modo Desarrollo - Sin 2FA)" -ForegroundColor Cyan
Write-Host "==========================================" -ForegroundColor Cyan
Write-Host ""

# Datos de login
$body = @{
    email = $Email
    password = $Password
} | ConvertTo-Json

try {
    Write-Host "Obteniendo token..." -ForegroundColor Yellow
    
    $response = Invoke-RestMethod -Uri "$ApiUrl/auth/dev-login" `
        -Method Post `
        -ContentType "application/json" `
        -Body $body
    
    Write-Host "✓ Login exitoso`n" -ForegroundColor Green
    
    Write-Host "==========================================" -ForegroundColor Cyan
    Write-Host "  INFORMACIÓN DEL USUARIO" -ForegroundColor Cyan
    Write-Host "==========================================" -ForegroundColor Cyan
    Write-Host "Email: $($response.user.email)" -ForegroundColor White
    Write-Host "Nombre: $($response.user.fullName)" -ForegroundColor White
    Write-Host "Rol: $($response.user.roleName)`n" -ForegroundColor White
    
    Write-Host "==========================================" -ForegroundColor Cyan
    Write-Host "  TOKEN JWT GENERADO" -ForegroundColor Cyan
    Write-Host "==========================================" -ForegroundColor Cyan
    Write-Host $response.token -ForegroundColor Green
    Write-Host ""
    
    Write-Host "==========================================" -ForegroundColor Cyan
    Write-Host "  CÓMO USAR EL TOKEN" -ForegroundColor Cyan
    Write-Host "==========================================" -ForegroundColor Cyan
    Write-Host ""
    Write-Host "1. En PowerShell:" -ForegroundColor Yellow
    Write-Host "   `$token = `"$($response.token)`""
    Write-Host "   `$headers = @{ Authorization = `"Bearer `$token`" }"
    Write-Host "   Invoke-RestMethod -Uri `"$ApiUrl/talentohumano`" -Headers `$headers`n"
    
    Write-Host "2. En curl (Git Bash):" -ForegroundColor Yellow
    Write-Host "   curl -H `"Authorization: Bearer $($response.token)`" \"
    Write-Host "        $ApiUrl/talentohumano`n"
    
    Write-Host "3. En Postman/Thunder Client:" -ForegroundColor Yellow
    Write-Host "   - Headers > Authorization"
    Write-Host "   - Value: Bearer $($response.token)`n"
    
    # Guardar en variable de entorno para esta sesión
    $env:JWT_TOKEN = $response.token
    Write-Host "Token guardado en variable: `$env:JWT_TOKEN" -ForegroundColor Green
    
} catch {
    Write-Host "✗ Error al obtener token`n" -ForegroundColor Red
    Write-Host "Detalles del error:" -ForegroundColor Yellow
    Write-Host $_.Exception.Message -ForegroundColor Red
    Write-Host ""
    Write-Host "Posibles soluciones:" -ForegroundColor Yellow
    Write-Host "1. Verifica que el backend esté corriendo:"
    Write-Host "   dotnet run --project Backend.csproj`n"
    Write-Host "2. Si el usuario no existe, créalo primero:"
    Write-Host "   POST $ApiUrl/auth/register"
    Write-Host "   Body: { `"nombre`": `"Usuario Test`", `"email`": `"test@example.com`", `"password`": `"Test123!`", `"confirmPassword`": `"Test123!`", `"telefono`": `"1234567890`" }"
}
