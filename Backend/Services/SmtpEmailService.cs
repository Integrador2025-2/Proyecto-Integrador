using System.Net;
using System.Net.Mail;

namespace Backend.Services;

public class SmtpEmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SmtpEmailService> _logger;

    public SmtpEmailService(IConfiguration configuration, ILogger<SmtpEmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody, string? fromEmail = null)
    {
        var smtpHost = _configuration["Email:Smtp:Host"] ?? "smtp.gmail.com";
        var smtpPort = int.Parse(_configuration["Email:Smtp:Port"] ?? "587");
        var smtpUser = _configuration["Email:Smtp:Username"] ?? string.Empty;
        var smtpPasswordRaw = _configuration["Email:Smtp:Password"] ?? string.Empty;
        
        // Eliminar espacios de la contraseña (las contraseñas de aplicación de Google vienen con espacios)
        var smtpPassword = smtpPasswordRaw.Replace(" ", string.Empty);
        var from = fromEmail ?? _configuration["Email:From"] ?? smtpUser;

        if (string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPassword))
        {
            throw new InvalidOperationException("Email SMTP credentials no configuradas (Email:Smtp:Username y Email:Smtp:Password)");
        }

        _logger.LogInformation("Intentando enviar email 2FA a {ToEmail} desde {FromEmail} via {Host}:{Port}", 
            toEmail, from, smtpHost, smtpPort);

        try
        {
            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(smtpUser, smtpPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 30000
            };

            using var message = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = htmlBody,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(toEmail));

            await client.SendMailAsync(message);
            _logger.LogInformation("Email 2FA enviado exitosamente a {ToEmail}", toEmail);
        }
        catch (SmtpException smtpEx)
        {
            _logger.LogError(smtpEx, "Error SMTP enviando email 2FA a {ToEmail}. StatusCode: {StatusCode}, Mensaje: {Message}", 
                toEmail, smtpEx.StatusCode, smtpEx.Message);
            
            // Mensajes de error más descriptivos basados en el código y mensaje
            string errorMessage;
            if (smtpEx.Message.Contains("Authentication", StringComparison.OrdinalIgnoreCase) || 
                smtpEx.Message.Contains("5.7.0", StringComparison.OrdinalIgnoreCase) ||
                smtpEx.Message.Contains("authentication required", StringComparison.OrdinalIgnoreCase))
            {
                errorMessage = "Autenticación fallida. Verifica Username y Password (contraseña de aplicación sin espacios).";
            }
            else
            {
                errorMessage = smtpEx.StatusCode switch
                {
                    SmtpStatusCode.GeneralFailure => "Error general de conexión SMTP. Verifica Host y Puerto.",
                    SmtpStatusCode.ClientNotPermitted => "Cliente no autorizado. Verifica que la contraseña de aplicación sea correcta.",
                    SmtpStatusCode.MustIssueStartTlsFirst => "Se requiere TLS/SSL. El servidor requiere conexión segura.",
                    _ => $"Error SMTP: {smtpEx.Message}"
                };
            }
            
            throw new InvalidOperationException($"Fallo al enviar el correo de 2FA: {errorMessage}", smtpEx);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado enviando email 2FA a {ToEmail} via SMTP", toEmail);
            throw new InvalidOperationException($"Fallo al enviar el correo de 2FA: {ex.Message}", ex);
        }
    }
}

