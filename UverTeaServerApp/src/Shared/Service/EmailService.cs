using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace UverTeaServerApp.src.Shared.Service;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var emailSettings = _configuration.GetSection("EmailSettings");
        
        var mimeMessage = new MimeMessage();
        mimeMessage.From.Add(new MailboxAddress(
            emailSettings["SenderName"], 
            emailSettings["SenderEmail"]
        ));
        mimeMessage.To.Add(MailboxAddress.Parse(toEmail));
        mimeMessage.Subject = subject;

        var builder = new BodyBuilder
        {
            HtmlBody = message // HTML format for email 
        };
        mimeMessage.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        
        //  Connect with Secure Options Development / Production 
        await client.ConnectAsync(
            emailSettings["Server"], 
            int.Parse(emailSettings["Port"] ?? "587"), 
            SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(
            emailSettings["Username"], 
            emailSettings["Password"]
        );

        await client.SendAsync(mimeMessage);
        await client.DisconnectAsync(true);
    }
}