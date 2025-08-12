using HRMS.Application.Interfaces;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class OnboardingWorkflowRepository(ApplicationDbContext context):GenericRepository<OnboardingWorkflow>(context),  IOnboardingWorkflowRepository
{
    public async Task<List<OnboardingWorkflow>> GetByDepartment(Guid departmentId)
    {
        return await context.OnboardingWorkflows.Where(o => o.DepartmentId == departmentId && o.IsActive).ToListAsync();
    }
}