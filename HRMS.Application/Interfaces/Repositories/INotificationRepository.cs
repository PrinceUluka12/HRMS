using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.NotificationAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface INotificationRepository : IGenericRepository<Notification>
{
    Task<IEnumerable<Notification>> GetByEmployeeAsync(Guid employeeId, int take = 50, int skip = 0);
    Task<int> GetUnreadCountAsync(Guid employeeId);
    Task<IEnumerable<Notification>> GetUnreadByEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default);
}