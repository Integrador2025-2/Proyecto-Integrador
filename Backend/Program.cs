var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// Add API services
builder.Services.AddControllers();

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
