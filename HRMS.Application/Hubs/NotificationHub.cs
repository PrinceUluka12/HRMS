using Microsoft.AspNetCore.SignalR;

namespace HRMS.Application.Hubs;

public class NotificationHub : Hub
{
    public async Task SendNotification(Guid userId, string message)
    {
        await Clients.User(userId.ToString()).SendAsync("ReceiveNotification", message);
    }
}