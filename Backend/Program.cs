using Backend.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add API services
builder.Services.AddControllers();

// Add CQRS services
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddAutoMapper(typeof(Program));

// Add Entity Framework
builder.Services.AddDbContext<Backend.Infrastructure.Context.ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<Backend.Infrastructure.Repositories.IUserRepository, Backend.Infrastructure.Repositories.UserRepository>();

// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Proyecto Integrador API",
        Version = "v1",
        Description = "API para el proyecto integrador",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Tu Nombre",
            Email = "tu.email@ejemplo.com"
        }
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

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers(); // Mapear controladores API

app.Run();
