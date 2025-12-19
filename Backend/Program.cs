using Backend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using DotNetEnv;

// Cargar variables de entorno desde .env
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Construir cadena de conexión según el entorno configurado
var dbEnvironment = Environment.GetEnvironmentVariable("DB_ENVIRONMENT") ?? "docker";
string connectionString;

if (dbEnvironment.ToLower() == "local")
{
    // Configuración para SQL Server LOCAL con Windows Authentication
    var server = Environment.GetEnvironmentVariable("DB_LOCAL_SERVER") ?? "DESKTOP-8H84J7R";
    var database = Environment.GetEnvironmentVariable("DB_LOCAL_NAME") ?? "MinCienciasDB";
    connectionString = $"Server={server};Database={database};Integrated Security=true;TrustServerCertificate=True;";
    Console.WriteLine($"🔧 Usando Base de Datos LOCAL: {database} en {server}");
}
else
{
    // Configuración para SQL Server en DOCKER
    var server = Environment.GetEnvironmentVariable("DB_DOCKER_SERVER") ?? "localhost,1433";
    var database = Environment.GetEnvironmentVariable("DB_DOCKER_NAME") ?? "ProyectoIntegradorDb";
    var user = Environment.GetEnvironmentVariable("DB_DOCKER_USER") ?? "sa";
    var password = Environment.GetEnvironmentVariable("DB_DOCKER_PASSWORD") ?? "ProyectoIntegrador123!";
    connectionString = $"Server={server};Database={database};User Id={user};Password={password};TrustServerCertificate=True;MultipleActiveResultSets=True;";
    Console.WriteLine($"🐳 Usando Base de Datos DOCKER: {database} en {server}");
}

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
    options.UseSqlServer(connectionString, 
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null)));

// Add repositories
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IUserRepository, Backend.Infrastructure.Repositories.UserRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IRoleRepository, Backend.Infrastructure.Repositories.RoleRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IProyectoRepository, Backend.Infrastructure.Repositories.ProyectoRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IObjetivoRepository, Backend.Infrastructure.Repositories.ObjetivoRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.ICadenaDeValorRepository, Backend.Infrastructure.Repositories.CadenaDeValorRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IActividadRepository, Backend.Infrastructure.Repositories.ActividadRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.ITareaRepository, Backend.Infrastructure.Repositories.TareaRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IRecursoRepository, Backend.Infrastructure.Repositories.RecursoRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IEntidadRepository, Backend.Infrastructure.Repositories.EntidadRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IActXEntidadRepository, Backend.Infrastructure.Repositories.ActXEntidadRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.ICronogramaTareaRepository, Backend.Infrastructure.Repositories.CronogramaTareaRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IRecursoEspecificoRepository, Backend.Infrastructure.Repositories.RecursoEspecificoRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IRubroRepository, Backend.Infrastructure.Repositories.RubroRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IGastosViajeRepository, Backend.Infrastructure.Repositories.GastosViajeRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.ICapacitacionEventosRepository, Backend.Infrastructure.Repositories.CapacitacionEventosRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IMaterialesInsumosRepository, Backend.Infrastructure.Repositories.MaterialesInsumosRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IServiciosTecnologicosRepository, Backend.Infrastructure.Repositories.ServiciosTecnologicosRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IEquiposSoftwareRepository, Backend.Infrastructure.Repositories.EquiposSoftwareRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IDivulgacionRepository, Backend.Infrastructure.Repositories.DivulgacionRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.ISeguimientoEvaluacionRepository, Backend.Infrastructure.Repositories.SeguimientoEvaluacionRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IProteccionConocimientoDivulgacionRepository, Backend.Infrastructure.Repositories.ProteccionConocimientoDivulgacionRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IAdministrativosRepository, Backend.Infrastructure.Repositories.AdministrativosRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.ITalentoHumanoRepository, Backend.Infrastructure.Repositories.TalentoHumanoRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.ITalentoHumanoTareaRepository, Backend.Infrastructure.Repositories.TalentoHumanoTareaRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IRemuneracionPorAnioRepository, Backend.Infrastructure.Repositories.RemuneracionPorAnioRepository>();
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IContratacionRepository, Backend.Infrastructure.Repositories.ContratacionRepository>();
// TODO: Add more repositories as needed (specific resource types, etc.)

// Add services
builder.Services.AddScoped<Backend.Services.IAuthService, Backend.Services.AuthService>();
// Usar SMTP para permitir enviar a cualquier email (Gmail con contraseña de aplicación)
builder.Services.AddScoped<Backend.Services.IEmailService, Backend.Services.SmtpEmailService>();

// Add Redis connection multiplexer
builder.Services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
    StackExchange.Redis.ConnectionMultiplexer.Connect(
        Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING") ?? "localhost:6379"
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
    var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? "your-super-secret-key-that-is-at-least-32-characters-long";

    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "ProyectoIntegrador",
        ValidAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "ProyectoIntegrador",
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSecret))
    };
});

// Register Google only when credentials are provided (avoids runtime validation error when empty)
var googleClientId = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID");
var googleClientSecret = Environment.GetEnvironmentVariable("GOOGLE_CLIENT_SECRET");
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

// ============================================
// AGREGAR CORS - PERMITIR FRONTEND
// ============================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:3000",      // Vite dev server
            "http://localhost:3001",      // Vite dev server (puerto alternativo 2)
            "http://localhost:5173",      // Vite dev server (puerto alternativo)
            "https://localhost:3000",
            "https://localhost:3001",
            "https://localhost:5173"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

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

// Enable CORS for frontend before authentication/authorization
app.UseCors("AllowFrontend");

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
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            try
            {
                var sqlBuilder = new SqlConnectionStringBuilder(connectionString);
                var targetDb = string.IsNullOrWhiteSpace(sqlBuilder.InitialCatalog) ? "ProyectoIntegradorDb" : sqlBuilder.InitialCatalog;

                // Connect to master to create the DB if it doesn't exist
                var masterBuilder = new SqlConnectionStringBuilder(connectionString)
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
