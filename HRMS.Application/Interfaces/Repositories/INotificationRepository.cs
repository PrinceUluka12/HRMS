using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.NotificationAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<List<Notification>> GetByEmployeeIdIdAsync(Guid EmployeeId, CancellationToken cancellationToken);
}