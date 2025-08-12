using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Enums;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class BuddyPairRepository(ApplicationDbContext context) : GenericRepository<BuddyPair>(context), IBuddyPairRepository
{
    public async Task<List<BuddyPair>> GetAllByEmployee(Guid employeeId)
    {
        return await context.BuddyPairs.Where(e => e.MentorId == employeeId || e.MenteeId == employeeId).ToListAsync();
    }

    public async Task<List<BuddyPair>> GetPendingCheckinsByEmployee(Guid employeeId)
    {
        // Get all active BuddyPairs where employee is mentor or mentee
        var activePairs = await context.BuddyPairs
            .Where(bp => bp.Status == BuddyPairStatus.Active &&
                         (bp.MentorId == employeeId || bp.MenteeId == employeeId))
            .Include(bp => bp.CheckIns)
            .ToListAsync();
        var pendingPairs = new List<BuddyPair>();

        foreach (var pair in activePairs)
        {
            var lastCheckIn = pair.CheckIns
                .OrderByDescending(ci => ci.CheckInDate)
                .FirstOrDefault();

            
            // If no check-in ever or last check-in is older than a threshold (e.g. 7 days ago)
            if (lastCheckIn == null || lastCheckIn.CheckInDate < DateTime.UtcNow.AddDays(-7))
            {
                pendingPairs.Add(pair);
            }
        }

        return pendingPairs;
    }
}