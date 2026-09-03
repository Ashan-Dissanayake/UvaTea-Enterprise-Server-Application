using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace UverTeaServerApp.Shared.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
        {
            _logger.LogWarning("Recipient email is empty. Skipping email dispatch.");
            return;
        }

        var emailSettings = _configuration.GetSection("EmailSettings");
        var senderName = emailSettings["SenderName"] ?? "Uva Tea Factory";
        var senderEmail = emailSettings["SenderEmail"];
        var server = emailSettings["Server"];
        var portStr = emailSettings["Port"];
        var username = emailSettings["Username"];
        var password = emailSettings["Password"];

        if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(senderEmail))
        {
            _logger.LogWarning("Email settings (Server or SenderEmail) are not fully configured in appsettings.json. Skipping email to {Recipient}.", toEmail);
            return;
        }

        try
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(senderName, senderEmail));
            mimeMessage.To.Add(MailboxAddress.Parse(toEmail));
            mimeMessage.Subject = subject;

            var builder = new BodyBuilder
            {
                HtmlBody = message
            };
            mimeMessage.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            var port = int.TryParse(portStr, out int p) ? p : 587;

            await client.ConnectAsync(server, port, SecureSocketOptions.StartTls);

            if (!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                await client.AuthenticateAsync(username, password);
            }

            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);

            _logger.LogInformation("Email sent successfully to {Recipient} with subject '{Subject}'", toEmail, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}. Error: {Message}", toEmail, ex.Message);
        }
    }
}