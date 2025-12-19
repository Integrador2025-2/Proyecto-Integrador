# 🔄 Guía Rápida: Cambiar entre Base de Datos Local y Docker

## 📋 Resumen

Este proyecto soporta dos configuraciones de base de datos que puedes cambiar fácilmente:

1. **LOCAL** - SQL Server local con autenticación de Windows (MinCienciasDB)
2. **DOCKER** - SQL Server en contenedor Docker (ProyectoIntegradorDb)

---

## 🚀 Cambio Rápido

### Para usar Base de Datos LOCAL:

**Edita el archivo `.env` y cambia esta línea:**
```env
DB_ENVIRONMENT=local
```

**Configuración completa:**
```env
# Database Configuration
DB_ENVIRONMENT=local

# --- Base de Datos LOCAL ---
DB_LOCAL_SERVER=DESKTOP-8H84J7R
DB_LOCAL_NAME=MinCienciasDB
DB_LOCAL_INTEGRATED_SECURITY=true
```

**Ventajas:**
- ✅ No necesitas Docker corriendo
- ✅ Usa autenticación de Windows
- ✅ Ideal para desarrollo y pruebas de nuevos modelos

---

### Para usar Base de Datos DOCKER:

**Edita el archivo `.env` y cambia esta línea:**
```env
DB_ENVIRONMENT=docker
```

**Configuración completa:**
```env
# Database Configuration
DB_ENVIRONMENT=docker

# --- Base de Datos DOCKER ---
DB_DOCKER_SERVER=localhost,1433
DB_DOCKER_NAME=ProyectoIntegradorDb
DB_DOCKER_USER=sa
DB_DOCKER_PASSWORD=ProyectoIntegrador123!
```

**Antes de ejecutar, asegúrate de:**
```bash
# Levantar los contenedores
docker-compose up -d
```

**Ventajas:**
- ✅ Base de datos compartida con el equipo
- ✅ Fácil de resetear (solo bajar y subir el contenedor)
- ✅ Aislada del sistema

---

## 🔍 Verificar qué Base de Datos estás usando

Cuando ejecutes la aplicación, verás un mensaje en la consola:

**Si estás usando LOCAL:**
```
🔧 Usando Base de Datos LOCAL: MinCienciasDB en DESKTOP-8H84J7R
```

**Si estás usando DOCKER:**
```
🐳 Usando Base de Datos DOCKER: ProyectoIntegradorDb en localhost,1433
```

---

## 🛠️ Workflow Recomendado para Modelado

### Fase 1: Desarrollo del Nuevo Modelo (Local)
```bash
# 1. Cambiar a LOCAL
# Edita .env: DB_ENVIRONMENT=local

# 2. Ejecutar la aplicación
dotnet run

# 3. Crear/modificar modelos en Models/Domain/

# 4. Crear migración
dotnet ef migrations add NuevoModelado

# 5. Aplicar migración
dotnet ef database update
```

### Fase 2: Testing del Nuevo Modelo (Local)
```bash
# Probar funcionalidades con la base de datos local
# Hacer ajustes según sea necesario
```

### Fase 3: Migrar a Docker (cuando esté listo)
```bash
# 1. Cambiar a DOCKER
# Edita .env: DB_ENVIRONMENT=docker

# 2. Levantar Docker
docker-compose up -d

# 3. Aplicar migraciones
dotnet ef database update

# 4. Commitear cambios
git add .
git commit -m "Update database model"
git push origin dev
```

---

## ⚠️ IMPORTANTE

### NO hagas esto:
- ❌ NO cambies de entorno sin aplicar las migraciones pendientes
- ❌ NO borres las migraciones existentes
- ❌ NO edites migraciones que ya están en `dev` o `main`

### SÍ haz esto:
- ✅ Commitea las migraciones al repositorio
- ✅ Comunica al equipo cuando cambies el modelo
- ✅ Prueba en LOCAL antes de migrar a DOCKER
- ✅ Documenta los cambios grandes en el modelo

---

## 🆘 Troubleshooting

### Error: "Cannot open database MinCienciasDB"
**Solución:** Verifica que SQL Server local esté corriendo y que la base de datos exista.

```sql
-- Crear la base de datos si no existe
CREATE DATABASE MinCienciasDB;
```

### Error: "Login failed for user 'sa'"
**Solución:** Verifica que Docker esté corriendo y la contraseña sea correcta.

```bash
# Reiniciar contenedores
docker-compose down
docker-compose up -d
```

### Quiero resetear la base de datos local
```bash
# 1. Eliminar la base de datos en SQL Server Management Studio
# 2. O ejecutar:
dotnet ef database drop
dotnet ef database update
```

### Quiero resetear la base de datos Docker
```bash
# Borrar el contenedor y su volumen
docker-compose down -v
docker-compose up -d
dotnet ef database update
```

---

## 📊 Comparación Rápida

| Característica | LOCAL | DOCKER |
|----------------|-------|--------|
| Requiere Docker | ❌ No | ✅ Sí |
| Autenticación | Windows | Usuario/Contraseña |
| Compartida con equipo | ❌ No | ✅ Sí |
| Fácil de resetear | ⚠️ Manual | ✅ Muy fácil |
| Ideal para | Desarrollo individual | Trabajo en equipo |
| Base de datos | MinCienciasDB | ProyectoIntegradorDb |

---

## 📚 Referencias

- [ENV_SETUP.md](./ENV_SETUP.md) - Documentación completa de variables de entorno
- [MIGRATIONS.md](./docs/MIGRATIONS.md) - Guía de migraciones
- [docker-compose.yml](./docker-compose.yml) - Configuración de Docker
