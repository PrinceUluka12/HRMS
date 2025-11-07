using HRMS.Application.Features.Notifications.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Interfaces
{
    public interface INotificationPublisher
    {
        Task PublishToEmployeeAsync(Guid employeeId, NotificationDto dto);
        Task BroadcastAsync(NotificationDto dto);
    }
}
