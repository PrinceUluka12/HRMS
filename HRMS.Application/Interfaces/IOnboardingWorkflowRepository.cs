using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.OnboardingAggregate;

namespace HRMS.Application.Interfaces;

public interface IOnboardingWorkflowRepository : IGenericRepository<OnboardingWorkflow>
{
    Task<List<OnboardingWorkflow>> GetByDepartment(Guid departmentId);
}