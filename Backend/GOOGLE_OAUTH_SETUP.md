# 🔐 Configuración de Google OAuth

## 📋 Pasos para configurar Google OAuth

### 1. Crear un proyecto en Google Cloud Console

1. Ve a [Google Cloud Console](https://console.cloud.google.com/)
2. Crea un nuevo proyecto o selecciona uno existente
3. Habilita la API de Google+ (ahora Google Identity)

### 2. Configurar OAuth 2.0

1. Ve a **APIs & Services** > **Credentials**
2. Haz clic en **Create Credentials** > **OAuth 2.0 Client IDs**
3. Selecciona **Web application**
4. Configura los **Authorized redirect URIs**:
   - Para desarrollo: `http://localhost:3000/auth/google/callback`
   - Para producción: `https://tu-dominio.com/auth/google/callback`

### 3. Obtener las credenciales

1. Copia el **Client ID** y **Client Secret**
2. Actualiza el archivo `appsettings.json`:

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

### 4. Variables de entorno (Recomendado para producción)

Para mayor seguridad, usa variables de entorno:

```bash
# Windows
set GOOGLE_CLIENT_ID=tu-client-id.apps.googleusercontent.com
set GOOGLE_CLIENT_SECRET=tu-client-secret

# Linux/Mac
export GOOGLE_CLIENT_ID=tu-client-id.apps.googleusercontent.com
export GOOGLE_CLIENT_SECRET=tu-client-secret
```

### 5. Configuración del frontend

El frontend necesitará configurar la autenticación con Google. Ejemplo con React:

```javascript
// Instalar: npm install @google-cloud/oauth2
import { GoogleOAuthProvider, GoogleLogin } from '@react-oauth/google';

function App() {
  const handleGoogleSuccess = async (credentialResponse) => {
    const response = await fetch('/api/auth/google-login', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify({
        googleToken: credentialResponse.credential
      })
    });
    
    const data = await response.json();
    // Manejar la respuesta del login
  };

  return (
    <GoogleOAuthProvider clientId="tu-client-id.apps.googleusercontent.com">
      <GoogleLogin
        onSuccess={handleGoogleSuccess}
        onError={() => console.log('Login Failed')}
      />
    </GoogleOAuthProvider>
  );
}
```

## 🔧 Endpoints de autenticación disponibles

### POST `/api/auth/register`
Registra un nuevo usuario con email y contraseña.

```json
{
  "firstName": "Juan",
  "lastName": "Pérez",
  "email": "juan@email.com",
  "password": "MiPassword123!",
  "roleId": 2
}
```

### POST `/api/auth/login`
Inicia sesión con email y contraseña.

```json
{
  "email": "juan@email.com",
  "password": "MiPassword123!"
}
```

### POST `/api/auth/google-login`
Inicia sesión con Google OAuth.

```json
{
  "googleToken": "token-from-google"
}
```

### POST `/api/auth/refresh-token`
Renueva el token de acceso.

```json
{
  "refreshToken": "refresh-token"
}
```

### POST `/api/auth/logout`
Cierra sesión.

```json
{
  "refreshToken": "refresh-token"
}
```

### POST `/api/auth/change-password`
Cambia la contraseña del usuario autenticado.

```json
{
  "currentPassword": "PasswordActual",
  "newPassword": "NuevoPassword123!"
}
```

### GET `/api/auth/me`
Obtiene información del usuario autenticado.

## 🔒 Configuración de JWT

El sistema usa JWT para la autenticación. Configuración en `appsettings.json`:

```json
{
  "JwtSettings": {
    "SecretKey": "tu-clave-secreta-muy-larga-y-segura",
    "Issuer": "ProyectoIntegrador",
    "Audience": "ProyectoIntegrador",
    "ExpirationMinutes": 60
  }
}
```

## 🚨 Notas de seguridad

1. **Nunca commits las credenciales** en el repositorio
2. **Usa variables de entorno** en producción
3. **Rota las claves** regularmente
4. **Usa HTTPS** en producción
5. **Valida todos los tokens** en el backend

## 📚 Recursos adicionales

- [Google OAuth 2.0 Documentation](https://developers.google.com/identity/protocols/oauth2)
- [JWT.io](https://jwt.io/) - Para debuggear tokens JWT
- [ASP.NET Core Authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/)
