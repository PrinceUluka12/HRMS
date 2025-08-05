using AutoMapper;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class OnboardingRepository(ApplicationDbContext context)
    : GenericRepository<Onboarding>(context), IOnboardingRepository
{
    public async Task<Onboarding> GetByEmployeeIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Onboardings.Include(e => e.Stages).ThenInclude(e => e.Tasks)
            .FirstOrDefaultAsync(e => e.EmployeeId == id, cancellationToken);
    }

    public async Task<List<Onboarding>> GetAllDataAsync(CancellationToken cancellationToken = default)
    {
        return await context.Onboardings.Include(e => e.Stages).ThenInclude(q => q.Tasks)
            .ToListAsync(cancellationToken);
    }
}