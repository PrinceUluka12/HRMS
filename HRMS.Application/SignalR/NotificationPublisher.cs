using HRMS.Application.Features.Notifications.Dtos;
using HRMS.Application.Hubs;
using HRMS.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace HRMS.Application.SignalR
{
    public class NotificationPublisher : INotificationPublisher
    {
        private readonly IHubContext<NotificationHub, INotificationClient> _hub;

        public NotificationPublisher(IHubContext<NotificationHub, INotificationClient> hub)
        {
            _hub = hub ?? throw new ArgumentNullException(nameof(hub));
        }

        /// <summary>
        /// Sends a notification to a specific employee.
        /// </summary>
        /// <param name="employeeId">Employee GUID</param>
        /// <param name="dto">Notification DTO</param>
        public async Task PublishToEmployeeAsync(Guid employeeId, NotificationDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            // SignalR sends to Users by their User Identifier (make sure IUserIdProvider maps claim to employeeId)
            await _hub.Clients.User(employeeId.ToString()).ReceiveNotification(dto);
        }

        /// <summary>
        /// Broadcasts a notification to all connected clients.
        /// </summary>
        /// <param name="dto">Notification DTO</param>
        public async Task BroadcastAsync(NotificationDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            await _hub.Clients.All.ReceiveNotification(dto);
        }
    }
}
