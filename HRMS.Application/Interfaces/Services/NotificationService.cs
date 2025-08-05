using HRMS.Application.Hubs;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Aggregates.NotificationAggregate;
using Microsoft.AspNetCore.SignalR;

namespace HRMS.Application.Interfaces.Services;

public class NotificationService(IHubContext<NotificationHub> hub, INotificationRepository notificationRepository)
    : INotificationService
{
    public async Task BroadcastAsync(string message, Guid employeeId)
    {
        await SaveNotificationAsync(message,employeeId);
        await hub.Clients.All.SendAsync("ReceiveNotification", message);
    }

    public async Task NotifyUserAsync(Guid employeeId, string message)
    {
        await SaveNotificationAsync(message, employeeId);
        await hub.Clients.User(employeeId.ToString()).SendAsync("ReceiveNotification", message);
    }
    private async Task SaveNotificationAsync(string message,Guid employeeId)
    {
        var data = new Notification
        {
            Message = message,
            Timestamp = DateTime.UtcNow,
            EmployeeId = employeeId
            
        };

        await notificationRepository.AddAsync(data);
    }
}