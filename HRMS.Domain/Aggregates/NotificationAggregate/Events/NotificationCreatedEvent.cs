using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Domain.Aggregates.NotificationAggregate.Events
{
    public sealed class NotificationCreatedEvent
    {
        public Guid NotificationId { get; }
        public Guid? EmployeeId { get; }


        public NotificationCreatedEvent(Guid notificationId, Guid? employeeId)
        {
            NotificationId = notificationId;
            EmployeeId = employeeId;
        }
    }
}
