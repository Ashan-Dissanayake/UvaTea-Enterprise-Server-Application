using MediatR;
using Microsoft.AspNetCore.SignalR;
using UverTeaServerApp.src.Shared.Hubs;

namespace UverTeaServerApp.src.Feature.EmployeeModule.Events;

public class EmployeeCreatedNotificationHandler : INotificationHandler<EmployeeCreatedEvent>
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public EmployeeCreatedNotificationHandler(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(EmployeeCreatedEvent notification, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveNotification", new
        {
            Title = "New Employee Registered",
            Message = $"{notification.FirstName} {notification.LastName} has joined the team.",
            Timestamp = DateTime.UtcNow
        }, cancellationToken);
    }
}