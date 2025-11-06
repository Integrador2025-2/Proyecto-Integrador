# 🔐 Configuración de Variables de Entorno

Este proyecto usa un archivo `.env` para manejar información sensible y configuraciones específicas del entorno.

## 📋 Configuración Inicial

### 1. Copiar el archivo de ejemplo

```bash
cp .env.example .env
```

### 2. Editar el archivo `.env`

Abre el archivo `.env` y configura tus valores:

```env
# Database Configuration
DB_SERVER=localhost,1433
DB_NAME=ProyectoIntegradorDb
DB_USER=sa
DB_PASSWORD=TuPasswordSegura123!

# JWT Configuration
JWT_SECRET_KEY=TuClaveSecretaSuperSegura_AlMenos32Caracteres
JWT_ISSUER=ProyectoIntegrador
JWT_AUDIENCE=ProyectoIntegrador
JWT_EXPIRATION_MINUTES=60

# Redis Configuration
REDIS_CONNECTION_STRING=localhost:6379

# RAG Service
RAG_SERVICE_BASE_URL=http://localhost:8001

# Email Configuration (SMTP)
EMAIL_FROM=tu-email@gmail.com
SMTP_HOST=smtp.gmail.com
SMTP_PORT=587
SMTP_USERNAME=tu-email@gmail.com
SMTP_PASSWORD=tu-app-password

# Resend API
RESEND_API_KEY=tu-resend-api-key

# Google OAuth (Opcional)
GOOGLE_CLIENT_ID=
GOOGLE_CLIENT_SECRET=
```

## 🔒 Seguridad

### ⚠️ IMPORTANTE

- **NUNCA** commitees el archivo `.env` al repositorio
- **SIEMPRE** usa `.env.example` como plantilla
- El `.env` contiene credenciales sensibles

### ✅ Archivos en Git

- ✅ `.env.example` - **SÍ** debe estar en Git (plantilla sin datos reales)
- ❌ `.env` - **NO** debe estar en Git (ignorado por `.gitignore`)

## 📝 Variables de Entorno Disponibles

### Base de Datos - Configuración de Entorno

**IMPORTANTE**: El proyecto soporta dos modos de base de datos:

#### 🔧 Modo LOCAL (SQL Server local con Windows Authentication)
```env
DB_ENVIRONMENT=local
DB_LOCAL_SERVER=DESKTOP-8H84J7R
DB_LOCAL_NAME=MinCienciasDB
DB_LOCAL_INTEGRATED_SECURITY=true
```

#### 🐳 Modo DOCKER (SQL Server en contenedor)
```env
DB_ENVIRONMENT=docker
DB_DOCKER_SERVER=localhost,1433
DB_DOCKER_NAME=ProyectoIntegradorDb
DB_DOCKER_USER=sa
DB_DOCKER_PASSWORD=ProyectoIntegrador123!
```

#### 🔄 Cambiar entre Local y Docker

Para cambiar de base de datos, simplemente edita el archivo `.env`:

**Para usar Base de Datos LOCAL:**
```env
DB_ENVIRONMENT=local
```

**Para usar Base de Datos DOCKER:**
```env
DB_ENVIRONMENT=docker
```

| Variable | Descripción | Valores posibles |
|----------|-------------|------------------|
| `DB_ENVIRONMENT` | Define qué base de datos usar | `local` o `docker` |

### Base de Datos LOCAL

| Variable | Descripción | Ejemplo |
|----------|-------------|---------|
| `DB_LOCAL_SERVER` | Nombre del servidor SQL Server local | `DESKTOP-8H84J7R` |
| `DB_LOCAL_NAME` | Nombre de la base de datos | `MinCienciasDB` |
| `DB_LOCAL_INTEGRATED_SECURITY` | Usar autenticación de Windows | `true` |

### Base de Datos DOCKER

| Variable | Descripción | Ejemplo |
|----------|-------------|---------|
| `DB_DOCKER_SERVER` | Servidor de base de datos | `localhost,1433` |
| `DB_DOCKER_NAME` | Nombre de la base de datos | `ProyectoIntegradorDb` |
| `DB_DOCKER_USER` | Usuario de la base de datos | `sa` |
| `DB_DOCKER_PASSWORD` | Contraseña de la base de datos | `Password123!` |

### JWT (Autenticación)

| Variable | Descripción | Ejemplo |
|----------|-------------|---------|
| `JWT_SECRET_KEY` | Clave secreta para firmar tokens | Mínimo 32 caracteres |
| `JWT_ISSUER` | Emisor del token | `ProyectoIntegrador` |
| `JWT_AUDIENCE` | Audiencia del token | `ProyectoIntegrador` |
| `JWT_EXPIRATION_MINUTES` | Tiempo de expiración en minutos | `60` |

### Redis

| Variable | Descripción | Ejemplo |
|----------|-------------|---------|
| `REDIS_CONNECTION_STRING` | Cadena de conexión a Redis | `localhost:6379` |

### Email (SMTP)

| Variable | Descripción | Ejemplo |
|----------|-------------|---------|
| `EMAIL_FROM` | Email del remitente | `tu-email@gmail.com` |
| `SMTP_HOST` | Servidor SMTP | `smtp.gmail.com` |
| `SMTP_PORT` | Puerto SMTP | `587` |
| `SMTP_USERNAME` | Usuario SMTP | `tu-email@gmail.com` |
| `SMTP_PASSWORD` | Contraseña de aplicación Gmail | Ver instrucciones abajo |

### Google OAuth (Opcional)

| Variable | Descripción |
|----------|-------------|
| `GOOGLE_CLIENT_ID` | Client ID de Google OAuth |
| `GOOGLE_CLIENT_SECRET` | Client Secret de Google OAuth |

## 📧 Configurar Contraseña de Aplicación Gmail

Para usar Gmail como servidor SMTP:

1. Ir a [Google Account Security](https://myaccount.google.com/security)
2. Activar "Verificación en 2 pasos"
3. Ir a "Contraseñas de aplicación"
4. Seleccionar "Correo" y "Otro dispositivo"
5. Copiar la contraseña generada
6. Usar esa contraseña en `SMTP_PASSWORD`

## 🚀 Ejecución

El archivo `.env` se carga automáticamente al iniciar la aplicación:

```bash
dotnet run
```

## 🔄 Entornos Diferentes

Puedes crear múltiples archivos para diferentes entornos:

```bash
.env.development
.env.staging
.env.production
```

Y cargar el apropiado según el entorno.

## 🆘 Troubleshooting

### Error: "No se puede conectar a la base de datos"

1. Verifica que Docker esté corriendo
2. Verifica las credenciales en `.env`
3. Ejecuta `docker-compose up -d`

### Error: "JWT Secret Key muy corto"

La clave debe tener al menos 32 caracteres. Genera una nueva:

```bash
openssl rand -base64 32
```

### Error: "No se encuentra el archivo .env"

Asegúrate de haber copiado `.env.example` a `.env`:

```bash
cp .env.example .env
```

## 📚 Referencias

- [DotNetEnv Documentation](https://github.com/tonerdo/dotnet-env)
- [Best Practices for .env files](https://12factor.net/config)
