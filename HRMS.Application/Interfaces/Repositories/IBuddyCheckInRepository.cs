using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.BuddyPair.Dtos;
using HRMS.Domain.Aggregates.OnboardingAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface IBuddyCheckInRepository : IGenericRepository<BuddyCheckIn>
{
    Task<List<BuddyCheckIn>> GetCheckinsByPairIdAsync(Guid pairId);
}