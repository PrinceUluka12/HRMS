using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.TimeTrackingAggregate;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class TimeEntryRepository(ApplicationDbContext context)
    : GenericRepository<TimeEntry>(context), ITimeEntryRepository
{
    public async Task<IEnumerable<TimeEntry>> GetEntryByEmployeeId(Guid EmployeeId)
    {
        var data = await context.TimeEntries.Where(e => e.EmployeeId == EmployeeId).Include(loc => loc.Locations).ToListAsync();
        return data;
    }
}