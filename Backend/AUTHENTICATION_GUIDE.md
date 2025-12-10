# Guía de Autenticación para Pruebas

## Opción 1: Script Automático (PowerShell - Recomendado para Windows)

1. **Asegúrate de que el backend esté corriendo:**
   ```powershell
   dotnet run --project Backend.csproj
   ```

2. **En otra terminal PowerShell, ejecuta:**
   ```powershell
   .\scripts\Get-Token.ps1
   ```

3. **El script mostrará el token JWT que puedes copiar**

## Opción 2: Script Bash (Git Bash)

```bash
bash scripts/get-token.sh
```

## Opción 3: Manualmente con curl

### Paso 1: Crear un usuario (solo la primera vez)

```bash
curl -X POST http://localhost:5043/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "nombre": "Usuario Test",
    "email": "test@example.com",
    "password": "Test123!",
    "confirmPassword": "Test123!",
    "telefono": "1234567890"
  }'
```

### Paso 2: Obtener el token (dev-login sin 2FA)

```bash
curl -X POST http://localhost:5043/api/auth/dev-login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "test@example.com",
    "password": "Test123!"
  }'
```

Respuesta esperada:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "...",
  "user": {
    "id": 1,
    "fullName": "Usuario Test",
    "email": "test@example.com",
    "roleName": "Admin"
  }
}
```

## Opción 4: Swagger UI

1. Abre http://localhost:5043/swagger
2. Busca el endpoint **POST /api/auth/dev-login**
3. Haz clic en "Try it out"
4. Ingresa:
   ```json
   {
     "email": "test@example.com",
     "password": "Test123!"
   }
   ```
5. Copia el `token` de la respuesta
6. Haz clic en el botón "Authorize" (candado) arriba
7. Ingresa: `Bearer TU_TOKEN_AQUI`
8. Ahora puedes probar todos los endpoints protegidos

## Usar el Token en las Peticiones

### Con curl:
```bash
export TOKEN="tu_token_aqui"

curl -H "Authorization: Bearer $TOKEN" \
     http://localhost:5043/api/talentohumano
```

### Con PowerShell:
```powershell
$token = "tu_token_aqui"
$headers = @{ Authorization = "Bearer $token" }

Invoke-RestMethod -Uri "http://localhost:5043/api/talentohumano" -Headers $headers
```

### Con Postman / Thunder Client / Insomnia:
1. Ve a la pestaña **Headers**
2. Agrega un nuevo header:
   - **Key:** `Authorization`
   - **Value:** `Bearer tu_token_aqui`

## Endpoints Disponibles para Pruebas

Una vez autenticado, puedes probar:

- GET `/api/talentohumano` - Listar todos los talentos humanos
- GET `/api/talentohumano/{id}` - Obtener por ID
- POST `/api/talentohumano` - Crear nuevo
- PUT `/api/talentohumano/{id}` - Actualizar
- DELETE `/api/talentohumano/{id}` - Eliminar

Y todos los demás recursos: proyectos, actividades, tareas, contrataciones, etc.

## Notas Importantes

- **El endpoint `/api/auth/dev-login` es SOLO para desarrollo** y no requiere 2FA
- En producción, usa `/api/auth/login/init` + `/api/auth/2fa/verify` con autenticación de dos factores
- El token expira en 60 minutos por defecto
- Puedes renovarlo con `/api/auth/refresh-token`

## Solución de Problemas

### Error: "Credenciales inválidas"
- Verifica que el email y password sean correctos
- Si el usuario no existe, créalo con `/api/auth/register`

### Error: "Connection refused"
- El backend no está corriendo
- Ejecuta: `dotnet run --project Backend.csproj`

### Error: "Redis connection"
- El login normal requiere Redis para 2FA
- Usa el endpoint `/api/auth/dev-login` que NO requiere Redis
