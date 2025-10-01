using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Context;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
            entity.HasIndex(e => e.Email)
                .IsUnique();
            entity.Property(e => e.CreatedAt)
                .IsRequired();
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
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

        // Datos de prueba
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Usuarios de prueba
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                FirstName = "Juan",
                LastName = "Pérez",
                Email = "juan.perez@email.com",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = 2,
                FirstName = "María",
                LastName = "González",
                Email = "maria.gonzalez@email.com",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            },
            new User
            {
                Id = 3,
                FirstName = "Carlos",
                LastName = "López",
                Email = "carlos.lopez@email.com",
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


