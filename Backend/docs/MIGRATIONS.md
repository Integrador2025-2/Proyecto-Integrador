# Migraciones y preparación de la base de datos

Este documento explica cómo levantar la base de datos (Docker) y aplicar las migraciones de Entity Framework Core de forma segura para que otros desarrolladores puedan arrancar el backend sin conflictos.

Recomendación principal:
- Mantener la carpeta `Migrations/` en el repositorio. NO la ignores en `.gitignore`.
- Proveer scripts que automaticen: levantar containers, esperar a que SQL esté listo y ejecutar `dotnet ef database update` con reintentos.

---

Requisitos previos
- Docker & docker-compose instalados y en el PATH.
- .NET SDK instalado (la misma versión usada por el proyecto, p. ej. .NET 8).
- Desde la raíz del repo, el servicio `proyecto-integrador-db` está definido en `docker-compose.yml`.

Comandos rápidos (manual)
1. Levantar containers:

```bash
docker-compose up -d
```

2. Ir a la carpeta `Backend` y aplicar migraciones:

```bash
cd Backend
dotnet ef database update
```

Si `dotnet ef database update` falla por que la BD no está lista, repite el comando hasta que funcione, o usa el script que proveemos abajo.

Scripts automáticos

Hemos añadido dos scripts en `scripts/`:

- `scripts/ensure-db.sh` : script para ambientes bash (Linux, macOS, Git Bash / WSL en Windows).
- `scripts/ensure-db.ps1` : script PowerShell para Windows.

Uso (bash)
```bash
# desde la raíz del repo
./scripts/ensure-db.sh
```

Uso (PowerShell)
```powershell
# desde la raíz del repo
.\scripts\ensure-db.ps1
```

Qué hacen los scripts
- Ejecutan `docker-compose up -d` para levantar los servicios definidos.
- Intentan ejecutar `dotnet ef database update` dentro de `Backend/` con reintentos y espera entre intentos.
- Si aplican correctamente las migraciones, informan y salen con código 0.
- Si fallan después del número máximo de reintentos, salen con código distinto de 0 e imprimen la última salida de error.

Generar script SQL idempotente para despliegue

Si necesitas un script SQL legible para aplicar en producción o entregarlo a un DBA, puedes generarlo con:

```bash
cd Backend
dotnet ef migrations script --idempotent -o docs/all_migrations.sql
```

Esto generará `docs/all_migrations.sql` que puedes revisar y aplicar manualmente.

Notas sobre conflictos habituales
- Si la BD ya tiene objetos creados (nombres de tablas/constraints distintos) puede que `dotnet ef database update` falle. En esos casos:
  - Revisar los archivos en `Migrations/` y el snapshot `ApplicationDbContextModelSnapshot.cs`.
  - Ejecutar el script idempotente para intentar aplicar y ver errores SQL detallados.
  - Como último recurso, y solo si se confirma que la estructura ya coincide, se puede insertar manualmente una fila en `__EFMigrationsHistory` para marcar migraciones como aplicadas. Esto debe documentarse.

---

Si quieres, puedo:
- Añadir la generación automatizada del `docs/all_migrations.sql` a un job de CI.
- Generar el `all_migrations.sql` ahora (requiere que `dotnet ef` esté disponible en este entorno).
- Ajustar las migraciones problemáticas del repo para que sean idempotentes (ya se hicieron algunas ediciones manuales en este repo). 
