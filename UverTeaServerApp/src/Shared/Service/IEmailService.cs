namespace UverTeaServerApp.src.Shared.Service;

public interface IEmailService
{
    Task SendEmailAsync(string toEmail, string subject, string message);
}
