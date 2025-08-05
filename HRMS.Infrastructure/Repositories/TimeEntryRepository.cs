using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.TimeTrackingAggregate;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class TimeEntryRepository(ApplicationDbContext context)
    : GenericRepository<TimeEntry>(context), ITimeEntryRepository
{
    public async Task<IEnumerable<TimeEntry>> GetEntryByEmployeeId(Guid EmployeeId , CancellationToken cancellationToken = default)
    {
        return await context.TimeEntries
            .Where(e => e.EmployeeId == EmployeeId)
            .Include(e => e.Locations)
            .ToListAsync(cancellationToken);
    }
}