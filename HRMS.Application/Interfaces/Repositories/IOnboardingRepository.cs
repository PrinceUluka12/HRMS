using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Domain.Aggregates.OnboardingAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface IOnboardingRepository :IGenericRepository<Onboarding>
{
    Task<Onboarding> GetByEmployeeIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Onboarding>> GetAllDataAsync(CancellationToken cancellationToken = default);
    
    //Task<List<OnboardingTask>> GetAllTasksAsync(CancellationToken cancellationToken = default);
}