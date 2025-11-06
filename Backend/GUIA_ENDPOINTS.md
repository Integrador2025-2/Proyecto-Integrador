# 🚀 Guía para Probar los Endpoints del Backend

## ✅ Estado del Backend
- **URL Base**: `http://localhost:5043`
- **Swagger UI**: `http://localhost:5043` (abre automáticamente)
- **Base de Datos**: MinCienciasDB (SQL Server Local)
- **Estado**: ✅ Corriendo correctamente

---

## 📋 Endpoints Disponibles

### 🔐 Autenticación (Sin autorización requerida)

#### 1. **POST /api/auth/register**
Registrar un nuevo usuario
```json
{
  "email": "usuario@ejemplo.com",
  "password": "Password123!",
  "firstName": "Juan",
  "lastName": "Pérez"
}
```

#### 2. **POST /api/auth/login**
Iniciar sesión y obtener token JWT
```json
{
  "email": "usuario@ejemplo.com",
  "password": "Password123!"
}
```
**Respuesta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "userId": 1,
  "email": "usuario@ejemplo.com"
}
```

---

## 🔑 Usar el Token JWT en Swagger

1. Haz login y copia el `token` de la respuesta
2. Click en el botón **"Authorize"** en la parte superior derecha de Swagger
3. Ingresa: `Bearer TU_TOKEN_AQUI` (reemplaza TU_TOKEN_AQUI con el token copiado)
4. Click en **"Authorize"**
5. Ahora puedes probar todos los endpoints protegidos

---

## 📦 Endpoints de Gestión de Proyectos (Requieren autenticación)

### 📁 Proyectos
- **GET** `/api/proyectos` - Listar todos los proyectos
- **GET** `/api/proyectos/{id}` - Obtener proyecto por ID
- **GET** `/api/proyectos/usuario/{usuarioId}` - Proyectos de un usuario
- **POST** `/api/proyectos` - Crear nuevo proyecto
- **PUT** `/api/proyectos/{id}` - Actualizar proyecto
- **DELETE** `/api/proyectos/{id}` - Eliminar proyecto

**Ejemplo - Crear Proyecto:**
```json
{
  "usuarioId": 1,
  "nombre": "Proyecto de Investigación 2025",
  "descripcion": "Investigación sobre IA aplicada",
  "estado": "En Progreso",
  "fechaInicio": "2025-01-01",
  "fechaFin": "2025-12-31"
}
```

---

### 🎯 Objetivos
- **GET** `/api/objetivos` - Listar todos los objetivos
- **GET** `/api/objetivos/{id}` - Obtener objetivo por ID
- **GET** `/api/objetivos/proyecto/{proyectoId}` - Objetivos de un proyecto
- **POST** `/api/objetivos` - Crear nuevo objetivo
- **PUT** `/api/objetivos/{id}` - Actualizar objetivo
- **DELETE** `/api/objetivos/{id}` - Eliminar objetivo

**Ejemplo - Crear Objetivo:**
```json
{
  "proyectoId": 1,
  "nombre": "Objetivo General",
  "descripcion": "Desarrollar un sistema de IA",
  "resultadoEsperado": "Sistema funcional implementado"
}
```

---

### 🔗 Cadenas de Valor
- **GET** `/api/cadenasdevalor` - Listar todas las cadenas
- **GET** `/api/cadenasdevalor/{id}` - Obtener cadena por ID
- **GET** `/api/cadenasdevalor/objetivo/{objetivoId}` - Cadenas de un objetivo
- **POST** `/api/cadenasdevalor` - Crear nueva cadena
- **PUT** `/api/cadenasdevalor/{id}` - Actualizar cadena
- **DELETE** `/api/cadenasdevalor/{id}` - Eliminar cadena

**Ejemplo - Crear Cadena de Valor:**
```json
{
  "objetivoId": 1,
  "nombre": "Cadena de Investigación",
  "objetivoEspecifico": "Implementar módulo de ML"
}
```

---

### ⚡ Actividades
- **GET** `/api/actividades` - Listar todas las actividades
- **GET** `/api/actividades/{id}` - Obtener actividad por ID
- **GET** `/api/actividades/cadena/{cadenaDeValorId}` - Actividades de una cadena
- **POST** `/api/actividades` - Crear nueva actividad
- **PUT** `/api/actividades/{id}` - Actualizar actividad
- **DELETE** `/api/actividades/{id}` - Eliminar actividad

**Ejemplo - Crear Actividad:**
```json
{
  "cadenaDeValorId": 1,
  "nombre": "Recolección de datos",
  "descripcion": "Recopilar dataset de prueba",
  "duracionAnios": 1,
  "valorUnitario": 5000000
}
```

---

### ✅ Tareas
- **GET** `/api/tareas` - Listar todas las tareas
- **GET** `/api/tareas/{id}` - Obtener tarea por ID
- **GET** `/api/tareas/actividad/{actividadId}` - Tareas de una actividad
- **POST** `/api/tareas` - Crear nueva tarea
- **PUT** `/api/tareas/{id}` - Actualizar tarea
- **DELETE** `/api/tareas/{id}` - Eliminar tarea

**Ejemplo - Crear Tarea:**
```json
{
  "actividadId": 1,
  "nombre": "Análisis de datos",
  "descripcion": "Procesar y limpiar datos",
  "periodo": "2025-Q1",
  "monto": 2000000
}
```

---

### 💰 Recursos
- **GET** `/api/recursos` - Listar todos los recursos
- **GET** `/api/recursos/{id}` - Obtener recurso por ID
- **GET** `/api/recursos/actividad/{actividadId}` - Recursos de una actividad
- **POST** `/api/recursos` - Crear nuevo recurso
- **PUT** `/api/recursos/{id}` - Actualizar recurso
- **DELETE** `/api/recursos/{id}` - Eliminar recurso

**Ejemplo - Crear Recurso:**
```json
{
  "actividadId": 1,
  "entidadId": 1,
  "rubroId": 1,
  "tipoRecurso": "Talento Humano",
  "montoEfectivo": 10000000,
  "montoEspecie": 0,
  "descripcion": "Investigadores senior"
}
```

---

### 🏢 Entidades
- **GET** `/api/entidades` - Listar todas las entidades
- **GET** `/api/entidades/{id}` - Obtener entidad por ID
- **POST** `/api/entidades` - Crear nueva entidad
- **PUT** `/api/entidades/{id}` - Actualizar entidad
- **DELETE** `/api/entidades/{id}` - Eliminar entidad

**Ejemplo - Crear Entidad:**
```json
{
  "nombre": "Universidad Nacional"
}
```

---

### 🔄 Relaciones Actividad-Entidad
- **GET** `/api/actxentidad` - Listar todas las relaciones
- **GET** `/api/actxentidad/{id}` - Obtener relación por ID
- **GET** `/api/actxentidad/actividad/{actividadId}` - Relaciones de una actividad
- **GET** `/api/actxentidad/entidad/{entidadId}` - Relaciones de una entidad
- **POST** `/api/actxentidad` - Crear nueva relación
- **PUT** `/api/actxentidad/{id}` - Actualizar relación
- **DELETE** `/api/actxentidad/{id}` - Eliminar relación

**Ejemplo - Crear Relación:**
```json
{
  "actividadId": 1,
  "entidadId": 1,
  "efectivo": 5000000,
  "especie": 2000000,
  "totalContribucion": 7000000
}
```

---

### 📅 Cronogramas de Tareas
- **GET** `/api/cronogramatareas` - Listar todos los cronogramas
- **GET** `/api/cronogramatareas/{id}` - Obtener cronograma por ID
- **GET** `/api/cronogramatareas/tarea/{tareaId}` - Cronogramas de una tarea
- **POST** `/api/cronogramatareas` - Crear nuevo cronograma
- **PUT** `/api/cronogramatareas/{id}` - Actualizar cronograma
- **DELETE** `/api/cronogramatareas/{id}` - Eliminar cronograma

**Ejemplo - Crear Cronograma:**
```json
{
  "tareaId": 1,
  "duracionMeses": 3,
  "duracionDias": 90,
  "fechaInicio": "2025-01-01T00:00:00",
  "fechaFin": "2025-03-31T23:59:59"
}
```

---

## 🧪 Flujo de Prueba Recomendado

### 1️⃣ **Autenticación**
```bash
POST /api/auth/register  # Registrar usuario
POST /api/auth/login     # Obtener token JWT
```

### 2️⃣ **Configurar Token en Swagger**
- Click en "Authorize"
- Ingresar: `Bearer {tu-token}`

### 3️⃣ **Crear Estructura de Proyecto**
```bash
POST /api/proyectos      # Crear proyecto
POST /api/objetivos      # Crear objetivo para el proyecto
POST /api/cadenasdevalor # Crear cadena de valor para el objetivo
POST /api/actividades    # Crear actividad para la cadena
POST /api/tareas         # Crear tarea para la actividad
```

### 4️⃣ **Crear Recursos y Entidades**
```bash
POST /api/entidades      # Crear entidad participante
POST /api/recursos       # Crear recurso para actividad
POST /api/actxentidad    # Relacionar actividad con entidad
```

### 5️⃣ **Cronograma**
```bash
POST /api/cronogramatareas  # Crear cronograma para tarea
```

### 6️⃣ **Consultar Datos**
```bash
GET /api/proyectos/usuario/{usuarioId}           # Ver tus proyectos
GET /api/objetivos/proyecto/{proyectoId}         # Ver objetivos
GET /api/cadenasdevalor/objetivo/{objetivoId}    # Ver cadenas
GET /api/actividades/cadena/{cadenaDeValorId}    # Ver actividades
GET /api/tareas/actividad/{actividadId}          # Ver tareas
```

---

## ⚠️ Advertencias Actuales

El backend tiene algunas advertencias sobre propiedades `decimal` sin tipo especificado:
- ActXEntidad: Efectivo, Especie, TotalContribucion
- Actividad: ValorUnitario
- Contratacion: Iva, ValorMensual
- GastosViaje: Costo
- Recurso: MontoEfectivo, MontoEspecie
- Y otras...

**Estas advertencias NO afectan la funcionalidad actual**, pero es recomendable configurar la precisión en `ApplicationDbContext.OnModelCreating()` más adelante.

---

## 🛠️ Comandos Útiles

### Detener el backend
```bash
Ctrl + C en la terminal donde está corriendo
```

### Reiniciar el backend
```bash
dotnet run
```

### Ver logs en tiempo real
Los logs se muestran en la terminal donde ejecutaste `dotnet run`

### Verificar base de datos
```bash
dotnet ef database update  # Aplicar migraciones
```

---

## 📊 Herramientas Alternativas para Probar APIs

Si prefieres no usar Swagger, puedes usar:

### **Postman**
1. Importar la colección desde Swagger
2. Configurar header `Authorization: Bearer {token}`

### **cURL** (desde terminal)
```bash
# Login
curl -X POST http://localhost:5043/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"usuario@ejemplo.com","password":"Password123!"}'

# Usar token en solicitudes
curl -X GET http://localhost:5043/api/proyectos \
  -H "Authorization: Bearer {tu-token}"
```

### **Thunder Client** (extensión de VS Code)
- Instalar extensión "Thunder Client"
- Crear requests directamente en VS Code

---

## ✅ Checklist de Verificación

- [x] Backend compilado sin errores
- [x] Base de datos MinCienciasDB conectada
- [x] Migraciones aplicadas
- [x] Servidor corriendo en http://localhost:5043
- [x] Swagger UI accesible
- [x] Todos los endpoints creados y registrados
- [x] Autenticación JWT funcionando
- [x] Repositories inyectados en DI
- [x] Handlers CQRS configurados
- [x] Controllers mapeados

---

## 🎯 Próximos Pasos Recomendados

1. ✅ **Probar endpoints básicos** - Registro, login, crear proyecto
2. ⏳ **Configurar precisión de decimales** - Evitar advertencias
3. ⏳ **Crear seeds de datos** - Datos de prueba automáticos
4. ⏳ **Implementar validaciones** - FluentValidation en Commands
5. ⏳ **Agregar AutoMapper** - Mapeo automático de DTOs
6. ⏳ **Manejo de errores global** - Middleware de excepciones
7. ⏳ **Logging mejorado** - Serilog o similar
8. ⏳ **Unit tests** - Pruebas automatizadas

---

**¡El backend está listo para usar! 🚀**
Abre Swagger UI en http://localhost:5043 y comienza a probar los endpoints.
