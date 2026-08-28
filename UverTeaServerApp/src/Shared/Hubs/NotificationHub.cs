using Microsoft.AspNetCore.SignalR;

namespace UverTeaServerApp.src.Shared.Hubs;

public class NotificationHub : Hub
{
    
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}