using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    public DbSet<Proyecto> Proyectos { get; set; }
    public DbSet<Actividad> Actividades { get; set; }
    public DbSet<Rubro> Rubros { get; set; }
    public DbSet<TalentoHumano> TalentoHumano { get; set; }
    public DbSet<ServiciosTecnologicos> ServiciosTecnologicos { get; set; }
    public DbSet<EquiposSoftware> EquiposSoftware { get; set; }
    public DbSet<MaterialesInsumos> MaterialesInsumos { get; set; }
    public DbSet<CapacitacionEventos> CapacitacionEventos { get; set; }
    public DbSet<GastosViaje> GastosViaje { get; set; }
    public DbSet<ActXEntidad> ActXEntidades { get; set; }
    public DbSet<Entidad> Entidades { get; set; }
    public DbSet<CadenaDeValor> CadenasDeValor { get; set; }
    public DbSet<Tarea> Tareas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Role
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Description)
                .HasMaxLength(255);
            entity.Property(e => e.Permissions)
                .HasMaxLength(1000);
            entity.Property(e => e.CreatedAt)
                .IsRequired();
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            entity.HasIndex(e => e.Name)
                .IsUnique();
        });

        // Ensure entities map to the expected table names (existing DB uses pluralized names)
        modelBuilder.Entity<Entidad>(entity =>
        {
            entity.ToTable("Entidades");
            entity.HasKey(e => e.EntidadId);
            // The existing DB column is named Id_Entidad
            entity.Property(e => e.EntidadId)
                .HasColumnName("Id_Entidad")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(255);
        });

        // Configuración de User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255);
            entity.Property(e => e.GoogleId)
                .HasMaxLength(100);
            entity.Property(e => e.ProfilePictureUrl)
                .HasMaxLength(500);
            entity.Property(e => e.Provider)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("local");
            entity.HasIndex(e => e.Email)
                .IsUnique();
            entity.HasIndex(e => e.GoogleId)
                .IsUnique()
                .HasFilter("[GoogleId] IS NOT NULL");
            entity.Property(e => e.CreatedAt)
                .IsRequired();
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
            
            // Relación con Role
            entity.HasOne(e => e.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configuración de WeatherForecast
        modelBuilder.Entity<WeatherForecast>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Date)
                .IsRequired();
            entity.Property(e => e.TemperatureC)
                .IsRequired();
            entity.Property(e => e.Summary)
                .HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .IsRequired();
        });

        // Configuracion de ActXEntidad (tabla intermedia entre Actividad y Entidad)
        modelBuilder.Entity<ActXEntidad>(entity =>
        {
            entity.ToTable("ActxEntidades");
            entity.HasKey(e => e.ActXEntidadId);

            // Map CLR properties to existing DB column names
            entity.Property(e => e.ActXEntidadId)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.ActividadId)
                .HasColumnName("act_id");

            entity.Property(e => e.EntidadId)
                .HasColumnName("entidad_id");

            entity.Property(e => e.Efectivo).HasPrecision(18, 2);
            entity.Property(e => e.Especie).HasPrecision(18, 2);
            
            // The database has a column 'rubro_id' but we don't model a relationship here.

            entity.HasOne(e => e.Actividad)
                .WithMany(a => a.ActXEntidades)
                .HasForeignKey(e => e.ActividadId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Entidad)
                .WithMany(en => en.ActXEntidades)
                .HasForeignKey(e => e.EntidadId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de CadenaDeValor
        modelBuilder.Entity<CadenaDeValor>(entity =>
        {
            entity.ToTable("CadenasDeValor");
            entity.HasKey(e => e.CadenaDeValorId);
            entity.Property(e => e.CadenaDeValorId).HasColumnName("CadenaDeValorId").ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(255);
            entity.Property(e => e.ObjetivoEspecifico).HasMaxLength(1000);
            entity.Property(e => e.Producto).HasMaxLength(255);
        });

        // Configuración de Tarea
        modelBuilder.Entity<Tarea>(entity =>
        {
            entity.ToTable("Tareas");
            entity.HasKey(e => e.TareaId);
            entity.Property(e => e.TareaId).HasColumnName("TareaId").ValueGeneratedOnAdd();
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Descripcion).HasMaxLength(1000);
            entity.Property(e => e.Periodo).HasMaxLength(100);
            entity.Property(e => e.Monto).HasPrecision(18,2);

            entity.HasOne(e => e.Actividad)
                .WithMany() // Actividad puede tener muchas tareas; no modificar Actividad class
                .HasForeignKey(e => e.ActividadId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Relación Actividad -> CadenaDeValor (actividad incluye una CadenaDeValorId)
        modelBuilder.Entity<Actividad>(entity =>
        {
            entity.HasOne<CadenaDeValor>()
                .WithMany(c => c.Actividades)
                .HasForeignKey("CadenaDeValorId")
                .OnDelete(DeleteBehavior.SetNull);
        });

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
                Permissions = "[\"users.create\", \"users.read\", \"users.update\", \"users.delete\", \"weather.create\", \"weather.read\", \"weather.update\", \"weather.delete\"]",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new Role
            {
                Id = 2,
                Name = "Usuario",
                Description = "Rol con permisos básicos del sistema",
                Permissions = "[\"users.read\", \"weather.read\"]",
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
                RoleId = 1, // Administrador
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
                RoleId = 2, // Usuario
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = 3,
                FirstName = "Carlos",
                LastName = "López",
                Email = "carlos.lopez@email.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("User123!"),
                RoleId = 2, // Usuario
                CreatedAt = DateTime.UtcNow,
                IsActive = false
            }
        );

        // Pronósticos de prueba
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
        var random = new Random(42); // Semilla fija para datos consistentes

        var weatherForecasts = new List<WeatherForecast>();
        for (int i = 1; i <= 10; i++)
        {
            weatherForecasts.Add(new WeatherForecast
            {
                Id = i,
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(i)),
                TemperatureC = random.Next(-20, 55),
                Summary = summaries[random.Next(summaries.Length)],
                CreatedAt = DateTime.UtcNow
            });
        }

        modelBuilder.Entity<WeatherForecast>().HasData(weatherForecasts.ToArray());
    }
}




