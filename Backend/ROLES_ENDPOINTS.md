# Endpoints de Roles y Permisos

## ✅ Implementación Completa

Se han creado **8 endpoints** para gestión de roles y asignación de permisos a usuarios.

---

## 📋 Endpoints Disponibles

### 1. **GET /api/roles**
Obtiene todos los roles del sistema.

**Respuesta:**
```json
[
  {
    "id": 1,
    "name": "Admin",
    "description": "Administrador del sistema",
    "permissions": "{\"users\":\"all\",\"projects\":\"all\"}",
    "isActive": true,
    "createdAt": "2025-01-01T00:00:00Z",
    "updatedAt": null
  }
]
```

---

### 2. **GET /api/roles/{id}**
Obtiene un rol específico por ID.

**Ejemplo:**
```bash
curl -H "Authorization: Bearer $TOKEN" \
     http://localhost:5043/api/roles/1
```

---

### 3. **GET /api/roles/nombre/{name}**
Busca un rol por su nombre.

**Ejemplo:**
```bash
curl -H "Authorization: Bearer $TOKEN" \
     http://localhost:5043/api/roles/nombre/Admin
```

---

### 4. **GET /api/roles/{roleId}/usuarios**
Obtiene todos los usuarios que tienen un rol específico.

**Ejemplo:**
```bash
curl -H "Authorization: Bearer $TOKEN" \
     http://localhost:5043/api/roles/1/usuarios
```

**Respuesta:**
```json
[
  {
    "id": 1,
    "fullName": "Usuario Test",
    "email": "test@example.com",
    "isActive": true,
    "roleId": 1,
    "roleName": "Admin",
    "provider": "local"
  }
]
```

---

### 5. **POST /api/roles**
Crea un nuevo rol.

**Body:**
```json
{
  "name": "Editor",
  "description": "Puede editar contenido",
  "permissions": "{\"projects\":\"edit\",\"users\":\"read\"}"
}
```

**Ejemplo curl:**
```bash
curl -X POST http://localhost:5043/api/roles \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Editor",
    "description": "Puede editar contenido",
    "permissions": "{\"projects\":\"edit\",\"users\":\"read\"}"
  }'
```

---

### 6. **PUT /api/roles/{id}**
Actualiza un rol existente.

**Body:**
```json
{
  "id": 3,
  "name": "Editor",
  "description": "Puede editar y crear contenido",
  "permissions": "{\"projects\":\"all\",\"users\":\"read\"}",
  "isActive": true
}
```

**Ejemplo curl:**
```bash
curl -X PUT http://localhost:5043/api/roles/3 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 3,
    "name": "Editor",
    "description": "Puede editar y crear contenido",
    "permissions": "{\"projects\":\"all\",\"users\":\"read\"}",
    "isActive": true
  }'
```

---

### 7. **DELETE /api/roles/{id}**
Elimina un rol (solo si no tiene usuarios asignados).

**Ejemplo:**
```bash
curl -X DELETE http://localhost:5043/api/roles/3 \
  -H "Authorization: Bearer $TOKEN"
```

**Nota:** Si el rol tiene usuarios asignados, retornará un error 409 Conflict.

---

### 8. **POST /api/roles/asignar**
Asigna un rol a un usuario.

**Body:**
```json
{
  "userId": 5,
  "roleId": 2
}
```

**Ejemplo curl:**
```bash
curl -X POST http://localhost:5043/api/roles/asignar \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 5,
    "roleId": 2
  }'
```

**Respuesta:**
```json
{
  "message": "Rol asignado exitosamente"
}
```

---

## 🔐 Permisos (JSON Format)

El campo `permissions` almacena permisos en formato JSON. Ejemplo:

```json
{
  "users": "all",           // all, read, edit, delete, none
  "projects": "edit",
  "activities": "read",
  "reports": "all",
  "roles": "none"
}
```

**Valores posibles:**
- `all` - Todos los permisos (crear, leer, editar, eliminar)
- `edit` - Crear, leer y editar
- `read` - Solo lectura
- `delete` - Puede eliminar
- `none` - Sin acceso

---

## 📊 Casos de Uso

### Crear un rol "Coordinador"
```bash
curl -X POST http://localhost:5043/api/roles \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Coordinador",
    "description": "Coordinador de proyecto",
    "permissions": "{\"projects\":\"edit\",\"activities\":\"all\",\"users\":\"read\",\"reports\":\"read\"}"
  }'
```

### Ver todos los usuarios con rol "Admin"
```bash
curl -H "Authorization: Bearer $TOKEN" \
     http://localhost:5043/api/roles/1/usuarios
```

### Cambiar el rol de un usuario
```bash
curl -X POST http://localhost:5043/api/roles/asignar \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 10,
    "roleId": 3
  }'
```

### Desactivar un rol (sin eliminarlo)
```bash
curl -X PUT http://localhost:5043/api/roles/3 \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "id": 3,
    "name": "Editor",
    "description": "Rol desactivado temporalmente",
    "permissions": "{}",
    "isActive": false
  }'
```

---

## 🛡️ Validaciones Implementadas

1. **Nombre único:** No pueden existir dos roles con el mismo nombre
2. **Usuarios asignados:** No se puede eliminar un rol si tiene usuarios
3. **Roles activos:** Solo se pueden asignar roles activos a usuarios
4. **IDs válidos:** Validación de IDs > 0

---

## 🏗️ Arquitectura CQRS

### Archivos Creados:

**Commands:**
- `Commands/Roles/RoleCommands.cs` - CreateRoleCommand, UpdateRoleCommand, DeleteRoleCommand, AssignRoleToUserCommand

**Queries:**
- `Queries/Roles/RoleQueries.cs` - GetRoleByIdQuery, GetAllRolesQuery, GetRoleByNameQuery, GetUsersByRoleIdQuery

**Handlers:**
- `Handlers/Commands/RoleCommandHandlers.cs` - 4 command handlers
- `Handlers/Queries/RoleQueryHandlers.cs` - 4 query handlers

**Controllers:**
- `Controllers/RolesController.cs` - 8 endpoints REST

**Repository:**
- `Infrastructure/Repositories/IUserRepository.cs` - Agregado método `GetByRoleIdAsync()`
- `Infrastructure/Repositories/UserRepository.cs` - Implementación del método

---

## 🚀 Para Usar

1. **Reinicia el backend** para cargar los nuevos endpoints:
   ```bash
   # Detener el proceso actual
   taskkill /F /IM Backend.exe
   
   # Iniciar nuevamente
   dotnet run --project Backend.csproj
   ```

2. **Obtén un token** (ver `TOKEN_QUICKSTART.md`)

3. **Prueba los endpoints** con curl, Postman o Swagger UI

---

## 📝 Ejemplo Completo: Crear y Asignar un Rol

```bash
# 1. Obtener token
TOKEN=$(curl -s -X POST http://localhost:5043/api/auth/dev-login \
  -H "Content-Type: application/json" \
  -d '{"email":"test@example.com","password":"Test123!"}' \
  | grep -o '"token":"[^"]*' | cut -d'"' -f4)

# 2. Crear un nuevo rol
ROLE_RESPONSE=$(curl -s -X POST http://localhost:5043/api/roles \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Investigador",
    "description": "Investigador de proyecto",
    "permissions": "{\"projects\":\"read\",\"activities\":\"edit\",\"reports\":\"all\"}"
  }')

echo "Rol creado: $ROLE_RESPONSE"

# 3. Asignar rol a usuario (suponiendo userId=2, roleId=3)
curl -X POST http://localhost:5043/api/roles/asignar \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 2,
    "roleId": 3
  }'

# 4. Ver usuarios con ese rol
curl -H "Authorization: Bearer $TOKEN" \
     http://localhost:5043/api/roles/3/usuarios
```

---

## 🔍 Ver en Swagger

Una vez que reinicies el backend, ve a:
http://localhost:5043/swagger

Busca la sección **Roles** y verás los 8 endpoints disponibles.
