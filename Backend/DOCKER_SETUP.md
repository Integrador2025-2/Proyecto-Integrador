# 🐳 Configuración Docker para Base de Datos

## 📋 Pasos para Configurar Docker + SQL Server

### 🚀 **Paso 1: Instalar Docker Desktop**

1. **Descargar Docker Desktop**:
   - Ve a: https://www.docker.com/products/docker-desktop/
   - Descarga "Docker Desktop for Windows"
   - Instala siguiendo el asistente

2. **Verificar instalación**:
```bash
docker --version
docker-compose --version
```

### 🐳 **Paso 2: Configurar SQL Server en Docker**

He creado los archivos necesarios para ti:

#### **docker-compose.yml** (ya creado):
```yaml
version: '3.8'
services:
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: proyecto-integrador-db
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=ProyectoIntegrador123!
      - MSSQL_PID=Express
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql
    restart: unless-stopped
    healthcheck:
      test: ["CMD-SHELL", "/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ProyectoIntegrador123! -Q 'SELECT 1'"]
      interval: 30s
      timeout: 10s
      retries: 5

volumes:
  sqlserver_data:
```

#### **Connection String actualizada**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ProyectoIntegradorDB;User Id=sa;Password=ProyectoIntegrador123!;TrustServerCertificate=true;MultipleActiveResultSets=true"
  }
}
```

### 🔧 **Paso 3: Comandos para Ejecutar**

1. **Iniciar SQL Server**:
```bash
docker-compose up -d
```

2. **Verificar que está corriendo**:
```bash
docker ps
```

3. **Aplicar migraciones**:
```bash
dotnet ef database update
```

4. **Ejecutar aplicación**:
```bash
dotnet run
```

### 🎯 **Paso 4: Verificar Funcionamiento**

1. **Probar conexión**:
```bash
# Conectar a SQL Server desde línea de comandos
docker exec -it proyecto-integrador-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ProyectoIntegrador123!
```

2. **Probar API**:
   - Ve a: `https://localhost:5001`
   - Prueba los endpoints de usuarios

### 🛠️ **Comandos Útiles**

```bash
# Ver logs del contenedor
docker logs proyecto-integrador-db

# Parar el contenedor
docker-compose down

# Reiniciar el contenedor
docker-compose restart

# Ver estado de contenedores
docker ps -a

# Eliminar todo (cuidado, borra datos)
docker-compose down -v
```

### 🔍 **Troubleshooting**

#### **Error: "Port 1433 is already in use"**
```bash
# Ver qué está usando el puerto
netstat -ano | findstr :1433

# Cambiar puerto en docker-compose.yml
ports:
  - "1434:1433"  # Usar puerto 1434 en lugar de 1433
```

#### **Error: "Cannot connect to SQL Server"**
```bash
# Verificar que el contenedor está corriendo
docker ps

# Ver logs para errores
docker logs proyecto-integrador-db

# Reiniciar el contenedor
docker-compose restart
```

#### **Error: "Database does not exist"**
```bash
# Aplicar migraciones
dotnet ef database update

# O crear la base de datos manualmente
docker exec -it proyecto-integrador-db /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P ProyectoIntegrador123! -Q "CREATE DATABASE ProyectoIntegradorDB"
```

### 📊 **Ventajas de Docker**

- ✅ **Portable**: Funciona en cualquier máquina
- ✅ **Aislado**: No interfiere con otras instalaciones
- ✅ **Fácil de limpiar**: `docker-compose down -v`
- ✅ **Versionado**: Puedes usar diferentes versiones de SQL Server
- ✅ **Desarrollo**: Perfecto para desarrollo y testing

### 🎯 **Próximos Pasos**

1. **Instalar Docker Desktop**
2. **Ejecutar**: `docker-compose up -d`
3. **Aplicar migraciones**: `dotnet ef database update`
4. **Probar aplicación**: `dotnet run`

### 🆘 **Si Tienes Problemas**

1. **Verificar que Docker Desktop esté corriendo**
2. **Verificar que el puerto 1433 esté libre**
3. **Revisar logs del contenedor**
4. **Verificar connection string**

---

**¿Necesitas ayuda con algún paso específico?** Te guío paso a paso.


