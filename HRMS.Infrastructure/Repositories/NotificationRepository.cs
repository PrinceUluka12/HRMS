using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.NotificationAggregate;
using HRMS.Domain.Interfaces;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class NotificationRepository(ApplicationDbContext context): GenericRepository<Notification>(context),INotificationRepository
{
    public async Task<List<Notification>> GetByEmployeeIdIdAsync(Guid EmployeeId, CancellationToken cancellationToken)
    {
        return await context.Notifications.Where(e => e.EmployeeId == EmployeeId).ToListAsync();
    }
}