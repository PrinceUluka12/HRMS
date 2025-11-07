using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.NotificationAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class NotificationRepository(ApplicationDbContext _db) : GenericRepository<Notification>(_db), INotificationRepository
{
    public async Task<IEnumerable<Notification>> GetByEmployeeAsync(Guid employeeId, int take = 50, int skip = 0)
    {
        // Return notifications targeted to employee OR broadcast messages (EmployeeId == null), newest first
        return await _db.Notifications
        .Where(n => n.EmployeeId == null || n.EmployeeId == employeeId)
        .OrderByDescending(n => n.CreatedAt)
        .Skip(skip)
        .Take(take)
        .ToListAsync();
    }

    public async Task<IEnumerable<Notification>> GetUnreadByEmployeeAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
        return await _db.Notifications
            .Where(n => n.EmployeeId == employeeId && n.Status == NotificationStatus.Unread)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetUnreadCountAsync(Guid employeeId)
    {
        return await _db.Notifications.CountAsync(n => (n.EmployeeId == null || n.EmployeeId == employeeId) && n.Status == NotificationStatus.Unread);
    }
}