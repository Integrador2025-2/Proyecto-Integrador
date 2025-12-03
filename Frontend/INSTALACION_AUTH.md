# Guía de Integración - Sistema de Autenticación Completo

## Archivos Generados

He creado todos los archivos necesarios para el sistema de autenticación completo. Aquí está la estructura y dónde colocar cada archivo:

### Estructura de Carpetas

```
src/
├── types/
│   └── auth.types.ts
├── services/
│   ├── AuthService.ts
│   └── api.ts
├── store/
│   └── authStore.ts
├── hooks/
│   └── useAuth.ts
├── components/
│   └── auth/
│       └── PrivateRoute.tsx
├── pages/
│   └── auth/
│       ├── LoginPage.tsx
│       ├── RegisterPage.tsx
│       ├── GoogleCallback.tsx
│       └── UnauthorizedPage.tsx
├── routes/
│   └── router.tsx
└── App.tsx
```

## Paso 1: Instalar Dependencias

Asegúrate de tener instaladas estas dependencias:

```bash
npm install react-router-dom
npm install zustand
npm install axios
```

## Paso 2: Copiar Archivos

Copia cada archivo generado a su ubicación correspondiente en tu proyecto:

1. `auth.types.ts` → `src/types/auth.types.ts`
2. `AuthService.ts` → `src/services/AuthService.ts`
3. `api.ts` → `src/services/api.ts` (REEMPLAZAR el existente)
4. `authStore.ts` → `src/store/authStore.ts`
5. `useAuth.ts` → `src/hooks/useAuth.ts`
6. `PrivateRoute.tsx` → `src/components/auth/PrivateRoute.tsx`
7. `LoginPage.tsx` → `src/pages/auth/LoginPage.tsx`
8. `RegisterPage.tsx` → `src/pages/auth/RegisterPage.tsx`
9. `GoogleCallback.tsx` → `src/pages/auth/GoogleCallback.tsx`
10. `UnauthorizedPage.tsx` → `src/pages/auth/UnauthorizedPage.tsx`
11. `router.tsx` → `src/routes/router.tsx`
12. `App.tsx` → `src/App.tsx` (REEMPLAZAR el existente)

## Paso 3: Crear Variables de Entorno

Crea un archivo `.env` en la raíz del proyecto con:

```env
VITE_API_BASE_URL=https://localhost:7000/api
```

## Paso 4: Actualizar main.tsx

Asegúrate de que tu `main.tsx` sea simple:

```tsx
import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.tsx'
import './index.css'

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
)
```

## Paso 5: Verificar Componentes UI

Asegúrate de tener estos componentes de shadcn/ui instalados:

```bash
npx shadcn@latest add button
npx shadcn@latest add input
npx shadcn@latest add label
npx shadcn@latest add card
npx shadcn@latest add alert
```

## Paso 6: Iniciar Backend

Asegúrate de que el backend esté corriendo:

```bash
cd Backend
docker-compose up -d
dotnet run
```

El backend debe estar en `https://localhost:7000`

## Paso 7: Probar el Sistema

1. Inicia el frontend:
```bash
npm run dev
```

2. Abre el navegador en `http://localhost:5173` (o el puerto de Vite)

3. Deberías ser redirigido automáticamente a `/login`

4. Prueba con las credenciales de prueba:
   - Email: `juan.perez@email.com`
   - Password: `Admin123!`

## Características Implementadas

### Autenticación Completa
- Login con email/password
- Registro de nuevos usuarios
- Google OAuth (configurar CLIENT_ID)
- Refresh token automático
- Logout con invalidación de tokens
- Cambio de contraseña

### Protección de Rutas
- PrivateRoute protege rutas autenticadas
- Redirección automática a login si no autenticado
- Soporte para roles (requiredRoles)
- Página de no autorizado

### Manejo de Estado
- Zustand store para estado global
- Persistencia en localStorage
- Sincronización automática

### Seguridad
- Tokens en localStorage
- Interceptores de Axios para refresh automático
- Validación de contraseñas
- Manejo de errores

## Próximos Pasos

Una vez que el sistema de autenticación funcione:

1. Crear módulo de Proyectos
2. Crear módulo de Actividades
3. Crear módulo de Presupuesto
4. Integrar análisis con IA

## Configuración de Google OAuth (Opcional)

Si quieres habilitar Google OAuth:

1. Ve a Google Cloud Console
2. Crea credenciales OAuth 2.0
3. Configura redirect URI: `http://localhost:5173/auth/google/callback`
4. Actualiza `LoginPage.tsx` línea 56 con tu CLIENT_ID:
```tsx
'client_id=TU_GOOGLE_CLIENT_ID&' +
```

## Troubleshooting

### Error de CORS
Si obtienes errores de CORS, asegúrate de que el backend tenga configurado:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("http://localhost:5173")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
```

### Backend no responde
Verifica que Docker esté corriendo:
```bash
docker ps
```

### Tokens no se guardan
Abre DevTools → Application → Local Storage y verifica que se estén guardando:
- `access_token`
- `refresh_token`
- `user`

## Notas Importantes

1. El sistema usa localStorage para tokens (considera httpOnly cookies para producción)
2. Los refresh tokens expiran en 7 días por defecto
3. El access token expira en 60 minutos
4. El interceptor de Axios maneja automáticamente el refresh
5. Los usuarios por defecto tienen roleId=2 (Usuario)

## Estructura del Token JWT

El backend genera tokens JWT con estos claims:
- `nameid`: User ID
- `email`: Email del usuario
- `name`: Nombre completo
- `role_id`: ID del rol
- `role_name`: Nombre del rol
- `provider`: "local" o "google"

## Endpoints Disponibles

```
POST /api/auth/register
POST /api/auth/login
POST /api/auth/google-login
POST /api/auth/refresh-token
POST /api/auth/logout
POST /api/auth/change-password
GET  /api/auth/me
GET  /api/auth/google-auth-url
```

¡Listo! El sistema de autenticación está completo y listo para usar.
