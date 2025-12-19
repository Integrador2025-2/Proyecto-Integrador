using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Backend.Services;

public class ResendEmailService : IEmailService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ResendEmailService> _logger;

    public ResendEmailService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<ResendEmailService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody, string? fromEmail = null)
    {
        var apiKey = _configuration["Resend:ApiKey"] ?? string.Empty;
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Resend API key no configurada (Resend:ApiKey)");
        }

        // Para pruebas, usar el dominio de sandbox de Resend si el dominio no está verificado
        var configuredFrom = fromEmail ?? _configuration["Email:From"] ?? "onboarding@resend.dev";
        var from = configuredFrom;
        
        // Si el email no es del dominio de sandbox de Resend, intentar usar el sandbox
        // Esto es útil durante desarrollo cuando el dominio no está verificado aún
        if (!from.Contains("@resend.dev") && !from.Contains("@resend.com"))
        {
            _logger.LogWarning("Usando dominio de sandbox de Resend para pruebas. Configura un dominio verificado para producción. Remitente configurado: {From}", configuredFrom);
            from = "onboarding@resend.dev";
        }

        var client = _httpClientFactory.CreateClient();
        client.BaseAddress = new Uri("https://api.resend.com/");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var payload = new
        {
            from,
            to = new[] { toEmail },
            subject,
            html = htmlBody
        };

        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        var content = new StringContent(JsonSerializer.Serialize(payload, jsonOptions), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("emails", content);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            _logger.LogError("Error enviando email via Resend: {Status} {Body}", response.StatusCode, error);
            throw new InvalidOperationException("Fallo al enviar el correo de 2FA");
        }
    }
}


