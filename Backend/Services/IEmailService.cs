namespace Backend.Services;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string htmlBody, string? fromEmail = null);
}


