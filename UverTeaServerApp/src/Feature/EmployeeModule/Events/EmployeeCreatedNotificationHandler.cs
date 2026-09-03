using MediatR;
using Microsoft.AspNetCore.SignalR;
using UverTeaServerApp.Shared.Hubs;
using UverTeaServerApp.Shared.Services;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Events;

public class EmployeeCreatedNotificationHandler : INotificationHandler<EmployeeCreatedEvent>
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private readonly IEmailService _emailService;

    public EmployeeCreatedNotificationHandler(
        IHubContext<NotificationHub> hubContext, 
        IEmailService emailService)
    {
        _hubContext = hubContext;
        _emailService = emailService;
    }

    public async Task Handle(EmployeeCreatedEvent notification, CancellationToken cancellationToken)
    {
        // 1. Send Real-time Notification via SignalR
        var displayName = !string.IsNullOrWhiteSpace(notification.Callingname) 
            ? notification.Callingname 
            : notification.Fullname;

        await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
        {
            Title = "New Employee Registered",
            Message = $"{displayName} has joined the team.",
            Timestamp = DateTime.UtcNow
        }, cancellationToken);

        // 2. Dispatch Welcome Email if recipient email is present
        if (!string.IsNullOrWhiteSpace(notification.Email))
        {
            var emailSubject = "Welcome to Uva Tea Factory!";
            var emailBody = $@"
                <div style='font-family: Arial, sans-serif; padding: 20px;'>
                    <h2 style='color: #2e7d32;'>Welcome to Uva Tea Factory, {displayName}!</h2>
                    <p>Your employee profile has been successfully registered in the system.</p>
                    <p>Best regards,<br/><strong>Uva Tea Management</strong></p>
                </div>";

            await _emailService.SendEmailAsync(notification.Email, emailSubject, emailBody);
        }
    }
}
