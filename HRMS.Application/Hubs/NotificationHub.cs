using HRMS.Application.Features.Notifications.Queries;
using HRMS.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Hubs;

public class NotificationHub(ILogger<NotificationHub> _logger, IMediator _mediator) : Hub<INotificationClient>
{

    public override async Task OnConnectedAsync()
    {
        var userIdStr = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userIdStr) && Guid.TryParse(userIdStr, out var userId))
        {
            // Fetch unread notifications from DB for this employee
            var query = new GetUnreadNotificationsByEmployeeQuery(userId);
            var notifications = await _mediator.Send(query);

            foreach (var n in notifications)
            {
                await Clients.Caller.ReceiveNotification(n);
            }
        }

        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogWarning("Client disconnected: {ConnectionId}", Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}