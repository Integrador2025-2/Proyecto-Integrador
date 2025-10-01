# 🏗️ Arquitectura CQRS - Proyecto Integrador

## 📋 Tabla de Contenidos
- [Introducción](#introducción)
- [¿Qué es CQRS?](#qué-es-cqrs)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Componentes Principales](#componentes-principales)
- [Flujo de Datos](#flujo-de-datos)
- [Tecnologías Utilizadas](#tecnologías-utilizadas)
- [Patrones Implementados](#patrones-implementados)
- [Ventajas de esta Arquitectura](#ventajas-de-esta-arquitectura)
- [Ejemplos de Uso](#ejemplos-de-uso)
- [Mejores Prácticas](#mejores-prácticas)

## 🎯 Introducción

Este proyecto implementa una arquitectura **CQRS (Command Query Responsibility Segregation)** utilizando **.NET 8** y **ASP.NET Core**. La arquitectura separa claramente las operaciones de lectura (Queries) de las operaciones de escritura (Commands), proporcionando una base sólida para aplicaciones escalables y mantenibles.

## 🔍 ¿Qué es CQRS?

**CQRS** es un patrón arquitectónico que separa la responsabilidad de:
- **Commands**: Operaciones que modifican el estado de la aplicación (Create, Update, Delete)
- **Queries**: Operaciones que solo leen datos (Read)

### Beneficios:
- ✅ **Separación clara** de responsabilidades
- ✅ **Escalabilidad** independiente de lectura y escritura
- ✅ **Optimización** específica para cada tipo de operación
- ✅ **Mantenibilidad** mejorada
- ✅ **Testing** más fácil y granular

## 📁 Estructura del Proyecto

```
Backend/
├── 📂 Commands/                    # Comandos (Write Operations)
│   ├── 📂 Users/
│   │   ├── CreateUserCommand.cs
│   │   ├── UpdateUserCommand.cs
│   │   └── DeleteUserCommand.cs
│   └── 📂 Weather/
│       └── CreateWeatherForecastCommand.cs
├── 📂 Queries/                     # Consultas (Read Operations)
│   ├── 📂 Users/
│   │   ├── GetUserByIdQuery.cs
│   │   └── GetAllUsersQuery.cs
│   └── 📂 Weather/
│       ├── GetWeatherForecastByIdQuery.cs
│       └── GetAllWeatherForecastsQuery.cs
├── 📂 Handlers/                    # Manejadores de Commands y Queries
│   ├── 📂 Commands/
│   │   ├── CreateUserCommandHandler.cs
│   │   ├── UpdateUserCommandHandler.cs
│   │   └── DeleteUserCommandHandler.cs
│   └── 📂 Queries/
│       ├── GetUserByIdQueryHandler.cs
│       └── GetAllUsersQueryHandler.cs
├── 📂 Models/                      # Modelos de Datos
│   ├── 📂 Domain/                  # Modelos de dominio (entidades)
│   │   ├── User.cs
│   │   └── WeatherForecast.cs
│   └── 📂 DTOs/                    # Data Transfer Objects
│       ├── UserDto.cs
│       └── WeatherForecastDto.cs
├── 📂 Infrastructure/              # Infraestructura y Acceso a Datos
│   ├── 📂 Repositories/
│   │   ├── IUserRepository.cs
│   │   └── UserRepository.cs
│   └── MappingProfile.cs           # Configuración de AutoMapper
├── 📂 Controllers/                 # Controladores API (Orquestadores)
│   ├── UsersController.cs
│   └── WeatherForecastController.cs
├── 📂 Data/                        # Contexto de Base de Datos (futuro)
├── 📂 Migrations/                  # Migraciones de BD (futuro)
└── Program.cs                      # Configuración de la aplicación
```

## 🧩 Componentes Principales

### 1. **Commands (Comandos)**
**Ubicación**: `Commands/`
**Propósito**: Representan operaciones que modifican el estado de la aplicación.

```csharp
// Ejemplo: CreateUserCommand.cs
public class CreateUserCommand : IRequest<UserDto>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
```

**Características**:
- Implementan `IRequest<T>` de MediatR
- Representan una intención de modificar datos
- Contienen solo los datos necesarios para la operación

### 2. **Queries (Consultas)**
**Ubicación**: `Queries/`
**Propósito**: Representan operaciones que solo leen datos.

```csharp
// Ejemplo: GetUserByIdQuery.cs
public class GetUserByIdQuery : IRequest<UserDto?>
{
    public int Id { get; set; }
}
```

**Características**:
- Implementan `IRequest<T>` de MediatR
- Representan una intención de leer datos
- Pueden incluir filtros y parámetros de búsqueda

### 3. **Handlers (Manejadores)**
**Ubicación**: `Handlers/`
**Propósito**: Contienen la lógica de negocio para procesar Commands y Queries.

```csharp
// Ejemplo: CreateUserCommandHandler.cs
public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // Lógica de negocio aquí
    }
}
```

**Características**:
- Implementan `IRequestHandler<TRequest, TResponse>`
- Contienen la lógica de negocio
- Acceden a repositorios para persistir/obtener datos

### 4. **Models (Modelos)**
**Ubicación**: `Models/`

#### **Domain Models** (`Models/Domain/`)
- Representan las entidades del dominio de negocio
- Contienen la lógica de negocio
- Son independientes de la infraestructura

#### **DTOs** (`Models/DTOs/`)
- Data Transfer Objects para comunicación con la API
- Optimizados para transferencia de datos
- Separados de los modelos de dominio

### 5. **Infrastructure (Infraestructura)**
**Ubicación**: `Infrastructure/`

#### **Repositories**
- Abstraen el acceso a datos
- Implementan el patrón Repository
- Permiten cambiar la implementación de persistencia

#### **AutoMapper**
- Mapeo automático entre objetos
- Configurado en `MappingProfile.cs`
- Reduce código repetitivo

### 6. **Controllers (Controladores)**
**Ubicación**: `Controllers/`
**Propósito**: Actúan como orquestadores, delegando a MediatR.

```csharp
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers()
    {
        var query = new GetAllUsersQuery();
        var users = await _mediator.Send(query);
        return Ok(users);
    }
}
```

## 🔄 Flujo de Datos

### Flujo de un Command (Escritura):
```
1. Cliente → HTTP POST/PUT/DELETE
2. Controller → Recibe request
3. Controller → Crea Command
4. Controller → Envía Command a MediatR
5. MediatR → Busca Handler correspondiente
6. Handler → Ejecuta lógica de negocio
7. Handler → Accede a Repository
8. Repository → Persiste datos
9. Handler → Retorna resultado
10. Controller → Retorna HTTP Response
```

### Flujo de una Query (Lectura):
```
1. Cliente → HTTP GET
2. Controller → Recibe request
3. Controller → Crea Query
4. Controller → Envía Query a MediatR
5. MediatR → Busca Handler correspondiente
6. Handler → Accede a Repository
7. Repository → Obtiene datos
8. Handler → Mapea a DTO
9. Handler → Retorna DTO
10. Controller → Retorna HTTP Response
```

## 🛠️ Tecnologías Utilizadas

| Tecnología | Versión | Propósito |
|------------|---------|-----------|
| **.NET** | 8.0 | Framework principal |
| **ASP.NET Core** | 8.0 | Web API |
| **MediatR** | 12.2.0 | Implementación CQRS |
| **AutoMapper** | 12.0.1 | Mapeo de objetos |
| **FluentValidation** | 11.8.1 | Validación de datos |
| **Swagger** | 6.5.0 | Documentación API |

## 🎨 Patrones Implementados

### 1. **CQRS (Command Query Responsibility Segregation)**
- Separación de comandos y consultas
- Handlers especializados para cada operación

### 2. **Repository Pattern**
- Abstracción del acceso a datos
- Facilita testing y cambios de implementación

### 3. **DTO Pattern**
- Separación entre modelos de dominio y transferencia
- Optimización de transferencia de datos

### 4. **Dependency Injection**
- Inyección de dependencias con .NET DI Container
- Facilita testing y mantenimiento

### 5. **Mediator Pattern**
- Desacoplamiento entre componentes
- Implementado con MediatR

## ✅ Ventajas de esta Arquitectura

### **Escalabilidad**
- Commands y Queries pueden escalarse independientemente
- Optimización específica para cada tipo de operación

### **Mantenibilidad**
- Código organizado y fácil de encontrar
- Responsabilidades claramente definidas
- Fácil agregar nuevas funcionalidades

### **Testing**
- Handlers pueden probarse de forma aislada
- Mocks fáciles de implementar
- Cobertura de testing granular

### **Flexibilidad**
- Fácil cambiar implementación de persistencia
- Agregar validaciones, logging, etc.
- Optimizaciones específicas por operación

## 📝 Ejemplos de Uso

### Crear un Usuario:
```http
POST /api/users
Content-Type: application/json

{
  "firstName": "Juan",
  "lastName": "Pérez",
  "email": "juan.perez@email.com"
}
```

### Obtener Usuarios con Filtros:
```http
GET /api/users?isActive=true&searchTerm=Juan
```

### Obtener Usuario por ID:
```http
GET /api/users/1
```

## 🎯 Mejores Prácticas

### **Naming Conventions**
- Commands: `{Action}{Entity}Command` (ej: `CreateUserCommand`)
- Queries: `{Action}{Entity}Query` (ej: `GetUserByIdQuery`)
- Handlers: `{Command/Query}Handler` (ej: `CreateUserCommandHandler`)

### **Organización de Archivos**
- Un archivo por Command/Query
- Handlers en carpetas separadas
- DTOs agrupados por entidad

### **Validaciones**
- Validaciones en los Handlers
- Uso de FluentValidation para reglas complejas
- Validaciones de entrada en Controllers

### **Manejo de Errores**
- Excepciones específicas del dominio
- Manejo centralizado de errores
- Logging apropiado

### **Performance**
- Queries optimizadas para lectura
- Commands optimizados para escritura
- Caching en Queries cuando sea apropiado

## 🚀 Próximos Pasos

1. **Implementar Base de Datos**: Entity Framework Core
2. **Agregar Validaciones**: FluentValidation
3. **Implementar Logging**: Serilog
4. **Agregar Caching**: Redis o Memory Cache
5. **Implementar Autenticación**: JWT
6. **Agregar Tests**: Unit Tests y Integration Tests
7. **Implementar Event Sourcing**: Para auditoría
8. **Agregar Monitoring**: Application Insights

## 📚 Recursos Adicionales

- [MediatR Documentation](https://github.com/jbogard/MediatR)
- [AutoMapper Documentation](https://docs.automapper.org/)
- [CQRS Pattern](https://docs.microsoft.com/en-us/azure/architecture/patterns/cqrs)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

---

**Autor**: Equipo de Desarrollo  
**Fecha**: 2025  
**Versión**: 1.0



