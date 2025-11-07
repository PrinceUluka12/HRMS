using HRMS.Application.Features.Notifications.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Hubs
{
    public interface INotificationClient
    {
        Task ReceiveNotification(NotificationDto notification);
    }
}
