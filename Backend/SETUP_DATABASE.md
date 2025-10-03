# 🗄️ Configuración de Base de Datos - Proyecto Integrador

## 🚨 Problema Detectado
El sistema no puede encontrar SQL Server LocalDB. Tienes varias opciones para resolver esto:

## 📋 Opciones Disponibles

### 🥇 **Opción 1: SQLite (Recomendada para desarrollo)**
**Ventajas**: Fácil instalación, no requiere servidor, perfecta para desarrollo
**Desventajas**: Limitaciones para producción

### 🥈 **Opción 2: SQL Server Express**
**Ventajas**: SQL Server completo, ideal para producción
**Desventajas**: Requiere instalación

### 🥉 **Opción 3: SQL Server Developer Edition**
**Ventajas**: SQL Server completo, gratuito para desarrollo
**Desventajas**: Requiere instalación

## 🔧 Soluciones

### **Solución 1: Cambiar a SQLite (Más Fácil)**

1. **Agregar paquete SQLite**:
```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
```

2. **Cambiar connection string en appsettings.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=ProyectoIntegrador.db"
  }
}
```

3. **Actualizar Program.cs**:
```csharp
// Cambiar de UseSqlServer a UseSqlite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### **Solución 2: Instalar SQL Server Express**

1. **Descargar SQL Server Express**:
   - Ve a: https://www.microsoft.com/en-us/sql-server/sql-server-downloads
   - Descarga "Express" (gratuito)
   - Instala con configuración por defecto

2. **Verificar instalación**:
```bash
# Verificar que SQL Server está corriendo
sqlcmd -S "(localdb)\mssqllocaldb" -Q "SELECT @@VERSION"
```

3. **Crear base de datos manualmente** (opcional):
```sql
CREATE DATABASE ProyectoIntegradorDB;
```

### **Solución 3: Usar Docker (Avanzado)**

1. **Instalar Docker Desktop**
2. **Ejecutar SQL Server en contenedor**:
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword123!" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
```

3. **Actualizar connection string**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ProyectoIntegradorDB;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true"
  }
}
```

## 🎯 Recomendación

**Para desarrollo**: Usa **SQLite** (Opción 1)
**Para producción**: Usa **SQL Server Express** (Opción 2)

## 📝 Pasos Siguientes

Una vez que elijas una opción, ejecuta:

```bash
# Restaurar paquetes
dotnet restore

# Crear migraciones (si es necesario)
dotnet ef migrations add InitialCreate

# Aplicar migraciones
dotnet ef database update

# Ejecutar aplicación
dotnet run
```

## 🔍 Verificación

Para verificar que la base de datos funciona:

1. **Ejecutar la aplicación**:
```bash
dotnet run
```

2. **Probar endpoints**:
   - GET `https://localhost:5001/api/users`
   - POST `https://localhost:5001/api/users`

3. **Verificar en Swagger**:
   - Ve a `https://localhost:5001`
   - Prueba los endpoints

## 🆘 Si Tienes Problemas

1. **Verificar connection string**
2. **Verificar que el servidor esté corriendo**
3. **Verificar permisos de usuario**
4. **Revisar logs de la aplicación**

## 📞 Soporte

Si necesitas ayuda con alguna opción, dime cuál prefieres y te ayudo a implementarla paso a paso.





