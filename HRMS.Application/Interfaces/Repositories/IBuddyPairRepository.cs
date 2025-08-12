using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.OnboardingAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface IBuddyPairRepository : IGenericRepository<BuddyPair>
{
    Task<List<BuddyPair>> GetAllByEmployee(Guid employeeId);
    Task<List<BuddyPair>> GetPendingCheckinsByEmployee(Guid employeeId);
}