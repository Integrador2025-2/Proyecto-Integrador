## GUÍA RÁPIDA: Obtener Token JWT para Pruebas

### PASO 1: Inicia el Backend
Abre una terminal PowerShell o CMD y ejecuta:
```
dotnet run --project Backend.csproj
```

Espera a ver el mensaje: "Now listening on: http://localhost:5043"

---

### PASO 2: Obtén tu Token

#### OPCIÓN A: Con PowerShell (Más Fácil)
En otra terminal PowerShell:
```powershell
cd Backend
.\scripts\Get-Token.ps1
```

#### OPCIÓN B: Con curl (Git Bash)
```bash
curl -X POST http://localhost:5043/api/auth/dev-login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"Test123!"}'
```

#### OPCIÓN C: Con Swagger UI
1. Abre en tu navegador: http://localhost:5043/swagger
2. Busca: **POST /api/auth/dev-login**
3. Click "Try it out"
4. Pega esto:
```json
{
  "email": "test@example.com",
  "password": "Test123!"
}
```
5. Click "Execute"
6. Copia el valor de "token" de la respuesta

---

### PASO 3: Usa el Token

El token se ve así (ejemplo):
```
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwiZW1haWwiOiJ0ZXN0QGV4YW1wbGUuY29tIiwibmFtZSI6IlVzdWFyaW8gVGVzdCIsInJvbGVfaWQiOiIxIiwicm9sZV9uYW1lIjoiQWRtaW4iLCJwcm92aWRlciI6ImxvY2FsIiwibmJmIjoxNzMyMTQ4ODAwLCJleHAiOjE3MzIxNTI0MDAsImlhdCI6MTczMjE0ODgwMCwiaXNzIjoiUHJveWVjdG9JbnRlZ3JhZG9yIiwiYXVkIjoiUHJveWVjdG9JbnRlZ3JhZG9yIn0.abc123...
```

**En todas las peticiones HTTP, agrega este header:**
```
Authorization: Bearer TU_TOKEN_AQUI
```

#### Ejemplo con curl:
```bash
TOKEN="pega_aqui_tu_token"

curl -H "Authorization: Bearer $TOKEN" \
     http://localhost:5043/api/talentohumano
```

#### Ejemplo con PowerShell:
```powershell
$token = "pega_aqui_tu_token"
$headers = @{ Authorization = "Bearer $token" }
Invoke-RestMethod -Uri "http://localhost:5043/api/talentohumano" -Headers $headers
```

---

### SI EL USUARIO NO EXISTE:

Primero créalo con:
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

Luego repite el PASO 2.

---

### ENDPOINTS DISPONIBLES PARA PROBAR:

Todos requieren el header `Authorization: Bearer TOKEN`

**Talento Humano:**
- GET    /api/talentohumano
- GET    /api/talentohumano/{id}
- POST   /api/talentohumano
- PUT    /api/talentohumano/{id}
- DELETE /api/talentohumano/{id}

**Contrataciones:**
- GET    /api/contrataciones
- POST   /api/contrataciones

**Proyectos:**
- GET    /api/proyectos
- POST   /api/proyectos

**Actividades:**
- GET    /api/actividades
- POST   /api/actividades

Y todos los demás recursos del sistema...
