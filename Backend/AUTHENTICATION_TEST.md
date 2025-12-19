# 🧪 Pruebas de Autenticación

## 📋 Endpoints de Autenticación Disponibles

La aplicación está ejecutándose en: `https://localhost:7000` (o `http://localhost:5000`)

### 🔧 Swagger UI
Accede a la documentación interactiva en: `https://localhost:7000` (o `http://localhost:5000`)

## 🧪 Pruebas Manuales

### 1. **Registro de Usuario**
```bash
POST https://localhost:7000/api/auth/register
Content-Type: application/json

{
  "firstName": "Juan",
  "lastName": "Pérez",
  "email": "juan.perez@test.com",
  "password": "TestPassword123!",
  "roleId": 2
}
```

**Respuesta esperada:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "refresh-token-string",
  "expiresAt": "2025-10-04T02:00:00Z",
  "user": {
    "id": 4,
    "firstName": "Juan",
    "lastName": "Pérez",
    "email": "juan.perez@test.com",
    "fullName": "Juan Pérez",
    "isActive": true,
    "roleId": 2,
    "roleName": "Usuario",
    "provider": "local",
    "profilePictureUrl": null,
    "createdAt": "2025-10-04T01:00:00Z"
  }
}
```

### 2. **Login con Email y Contraseña**
```bash
POST https://localhost:7000/api/auth/login
Content-Type: application/json

{
  "email": "juan.perez@email.com",
  "password": "Admin123!"
}
```

### 3. **Obtener Usuario Actual**
```bash
GET https://localhost:7000/api/auth/me
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 4. **Renovar Token**
```bash
POST https://localhost:7000/api/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "refresh-token-string"
}
```

### 5. **Cambiar Contraseña**
```bash
POST https://localhost:7000/api/auth/change-password
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "currentPassword": "Admin123!",
  "newPassword": "NewPassword123!"
}
```

### 6. **Logout**
```bash
POST https://localhost:7000/api/auth/logout
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "refreshToken": "refresh-token-string"
}
```

## 🔐 Usuarios de Prueba Predefinidos

| Email | Contraseña | Rol |
|-------|------------|-----|
| juan.perez@email.com | Admin123! | Administrador |
| maria.gonzalez@email.com | User123! | Usuario |
| carlos.lopez@email.com | User123! | Usuario (Inactivo) |

## 🌐 Google OAuth (Configuración Requerida)

### Para probar Google OAuth:

1. **Configura las credenciales** en `appsettings.json`:
```json
{
  "Authentication": {
    "Google": {
      "ClientId": "tu-client-id.apps.googleusercontent.com",
      "ClientSecret": "tu-client-secret"
    }
  }
}
```

2. **Obtén la URL de autenticación:**
```bash
GET https://localhost:7000/api/auth/google-auth-url
```

3. **Login con Google:**
```bash
POST https://localhost:7000/api/auth/google-login
Content-Type: application/json

{
  "googleToken": "google-access-token"
}
```

## 🧪 Pruebas con cURL

### Registro:
```bash
curl -X POST https://localhost:7000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Test",
    "lastName": "User",
    "email": "test@example.com",
    "password": "TestPassword123!",
    "roleId": 2
  }'
```

### Login:
```bash
curl -X POST https://localhost:7000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "juan.perez@email.com",
    "password": "Admin123!"
  }'
```

### Usuario actual:
```bash
curl -X GET https://localhost:7000/api/auth/me \
  -H "Authorization: Bearer TU_TOKEN_AQUI"
```

## 🔍 Verificación de JWT

Puedes verificar el contenido del JWT en [jwt.io](https://jwt.io/):

1. Copia el token de la respuesta
2. Pégalo en jwt.io
3. Verifica que contenga los claims correctos:
   - `nameid` (User ID)
   - `email`
   - `name` (Full Name)
   - `role_id`
   - `role_name`
   - `provider`

## 🚨 Códigos de Error Comunes

| Código | Mensaje | Descripción |
|--------|---------|-------------|
| 400 | Bad Request | Datos de entrada inválidos |
| 401 | Unauthorized | Credenciales incorrectas o token inválido |
| 409 | Conflict | Email ya registrado |
| 500 | Internal Server Error | Error interno del servidor |

## 📝 Notas Importantes

1. **HTTPS**: La aplicación usa HTTPS en desarrollo
2. **Tokens**: Los JWT expiran en 60 minutos por defecto
3. **Refresh Tokens**: Válidos por 7 días
4. **Seguridad**: Las contraseñas se hashean con BCrypt
5. **Google OAuth**: Requiere configuración previa

## 🔧 Configuración de Desarrollo

Para cambiar la configuración JWT, edita `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "tu-clave-secreta-muy-larga",
    "Issuer": "ProyectoIntegrador",
    "Audience": "ProyectoIntegrador",
    "ExpirationMinutes": 60
  }
}
```
