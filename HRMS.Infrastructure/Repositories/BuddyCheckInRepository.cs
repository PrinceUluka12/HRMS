using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class BuddyCheckInRepository(ApplicationDbContext context): GenericRepository<BuddyCheckIn>(context), IBuddyCheckInRepository
{
    public async Task<List<BuddyCheckIn>> GetCheckinsByPairIdAsync(Guid pairId)
    {
        return await context.BuddyCheckIns.Where(e => e.BuddyPairId == pairId).ToListAsync();
    }
}