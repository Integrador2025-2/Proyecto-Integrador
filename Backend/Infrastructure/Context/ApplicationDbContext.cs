using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Entidades del sistema
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    // Entidades principales del proyecto
    public DbSet<Proyecto> Proyectos { get; set; }
    public DbSet<Objetivo> Objetivos { get; set; }
    public DbSet<CadenaDeValor> CadenasDeValor { get; set; }
    public DbSet<Actividad> Actividades { get; set; }
    public DbSet<Tarea> Tareas { get; set; }
    public DbSet<CronogramaTarea> CronogramaTareas { get; set; }
    
    // Recursos
    public DbSet<Recurso> Recursos { get; set; }
    public DbSet<RecursoEspecifico> RecursosEspecificos { get; set; }
    public DbSet<Rubro> Rubros { get; set; }
    
    // Tipos específicos de recursos
    public DbSet<TalentoHumano> TalentoHumano { get; set; }
    public DbSet<Contratacion> Contrataciones { get; set; }
    public DbSet<RemuneracionPorAnio> RemuneracionesPorAnio { get; set; }
    public DbSet<TalentoHumanoTarea> TalentoHumanoTareas { get; set; }
    public DbSet<EquiposSoftware> EquiposSoftware { get; set; }
    public DbSet<ServiciosTecnologicos> ServiciosTecnologicos { get; set; }
    public DbSet<MaterialesInsumos> MaterialesInsumos { get; set; }
    public DbSet<CapacitacionEventos> CapacitacionEventos { get; set; }
    public DbSet<GastosViaje> GastosViaje { get; set; }
    public DbSet<Infraestructura> Infraestructura { get; set; }
    public DbSet<Administrativos> Administrativos { get; set; }
    public DbSet<ProteccionConocimientoDivulgacion> ProteccionConocimientoDivulgacion { get; set; }
    public DbSet<SeguimientoEvaluacion> SeguimientoEvaluacion { get; set; }
    public DbSet<Divulgacion> Divulgacion { get; set; }
    public DbSet<Otros> Otros { get; set; }
    
    // Entidades participantes
    public DbSet<Entidad> Entidades { get; set; }
    public DbSet<ActXEntidad> ActXEntidades { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración básica de las tablas principales
        // TODO: Completar las configuraciones de relaciones y constraints después de crear la migración inicial
        
        // Datos de prueba
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Roles por defecto
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = 1,
                Name = "Administrador",
                Description = "Rol con permisos completos del sistema",
                Permissions = "[\"users.create\", \"users.read\", \"users.update\", \"users.delete\"]",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Role
            {
                Id = 2,
                Name = "Usuario",
                Description = "Rol con permisos básicos del sistema",
                Permissions = "[\"users.read\"]",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        );

        // Usuarios de prueba (contraseñas: "Admin123!" y "User123!")
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FirstName = "Juan",
                LastName = "Pérez",
                Email = "juan.perez@email.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                RoleId = 1,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = 2,
                FirstName = "María",
                LastName = "González",
                Email = "maria.gonzalez@email.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                RoleId = 2,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            }
        );
    }
}




