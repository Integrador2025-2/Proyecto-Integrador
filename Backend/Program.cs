using Backend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add API services
builder.Services.AddControllers();

// Add CQRS services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddAutoMapper(typeof(Program));

// Register HttpClient factory for external HTTP calls (n8n webhooks, etc.)
builder.Services.AddHttpClient();

// Add Entity Framework
builder.Services.AddDbContext<Backend.Infrastructure.Context.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), 
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));

// Add repositories
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IUserRepository, Backend.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IRoleRepository, Backend.Infrastructure.Repositories.RoleRepository>();
// Register new repositories
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IEntidadRepository, Backend.Infrastructure.Repositories.EntidadRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IActXEntidadRepository, Backend.Infrastructure.Repositories.ActXEntidadRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IProyectoRepository, Backend.Infrastructure.Repositories.ProyectoRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IActividadRepository, Backend.Infrastructure.Repositories.ActividadRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IRubroRepository, Backend.Infrastructure.Repositories.RubroRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IServiciosTecnologicosRepository, Backend.Infrastructure.Repositories.ServiciosTecnologicosRepository>();
// Repositories added for rubro-associated entities
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IMaterialesInsumosRepository, Backend.Infrastructure.Repositories.MaterialesInsumosRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.ICapacitacionEventosRepository, Backend.Infrastructure.Repositories.CapacitacionEventosRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IGastosViajeRepository, Backend.Infrastructure.Repositories.GastosViajeRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.ITalentoHumanoRepository, Backend.Infrastructure.Repositories.TalentoHumanoRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IEquiposSoftwareRepository, Backend.Infrastructure.Repositories.EquiposSoftwareRepository>();
// New repositories for CadenaDeValor and Tarea
builder.Services.AddScoped<Backend.Infrastructure.Repositories.ICadenaDeValorRepository, Backend.Infrastructure.Repositories.CadenaDeValorRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.ITareaRepository, Backend.Infrastructure.Repositories.TareaRepository>();

// Add services
builder.Services.AddScoped<Backend.Services.IAuthService, Backend.Services.AuthService>();

// Add Redis connection multiplexer
builder.Services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
    StackExchange.Redis.ConnectionMultiplexer.Connect(
        builder.Configuration["Redis:ConnectionString"] ?? "localhost:6379"
    )
);

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Proyecto Integrador API",
        Version = "v1",
        Description = "API para el proyecto integrador con autenticación JWT y Google OAuth",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Tu Nombre",
            Email = "tu.email@ejemplo.com"
        }
    });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add JWT Authentication (and optional Google if configured)
var authBuilder = builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
});

authBuilder.AddJwtBearer(options =>
{
    var configuredSecret = builder.Configuration["JwtSettings:SecretKey"];
    var jwtSecret = string.IsNullOrWhiteSpace(configuredSecret) ? "your-super-secret-key-that-is-at-least-32-characters-long" : configuredSecret!;

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = string.IsNullOrWhiteSpace(builder.Configuration["JwtSettings:Issuer"]) ? "ProyectoIntegrador" : builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = string.IsNullOrWhiteSpace(builder.Configuration["JwtSettings:Audience"]) ? "ProyectoIntegrador" : builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret))
    };
});

// Register Google only when credentials are provided (avoids runtime validation error when empty)
var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
{
    authBuilder.AddGoogle(options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
    });
}

// Add Authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Proyecto Integrador API v1");
        c.RoutePrefix = string.Empty; // Para que Swagger esté en la raíz
    });
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers(); // Mapear controladores API

// Ensure the target database exists (useful when SQL Server runs in Docker and DB not yet created)
try
{
    using (var scope = app.Services.CreateScope())
    {
        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var connString = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrWhiteSpace(connString))
        {
            try
            {
                var sqlBuilder = new SqlConnectionStringBuilder(connString);
                var targetDb = string.IsNullOrWhiteSpace(sqlBuilder.InitialCatalog) ? "ProyectoIntegradorDb" : sqlBuilder.InitialCatalog;

                // Connect to master to create the DB if it doesn't exist
                var masterBuilder = new SqlConnectionStringBuilder(connString)
                {
                    InitialCatalog = "master"
                };

                app.Logger.LogInformation("Ensuring database '{Database}' exists on server {DataSource}", targetDb, masterBuilder.DataSource);

                using (var conn = new SqlConnection(masterBuilder.ConnectionString))
                {
                    await conn.OpenAsync();
                    using (var cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = $"IF DB_ID(N'{targetDb}') IS NULL CREATE DATABASE [{targetDb}];";
                        cmd.CommandTimeout = 30;
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                // Apply EF Core migrations (if any)
                try
                {
                    var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    if (db != null)
                    {
                        app.Logger.LogInformation("Applying EF Core migrations (if any). This may take a moment...");
                        await db.Database.MigrateAsync();
                    }
                }
                catch (Exception migEx)
                {
                    app.Logger.LogWarning(migEx, "Failed to apply migrations automatically.");
                }
            }
            catch (Exception ex)
            {
                app.Logger.LogWarning(ex, "Error while ensuring/creating the database.\nMake sure SQL Server container is running and accessible.");
            }
        }
    }
}
catch (Exception outerEx)
{
    app.Logger.LogWarning(outerEx, "Unexpected error during database ensure/migrate step.");
}

await app.RunAsync();
