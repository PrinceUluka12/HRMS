using HRMS.Application.Common.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.DepartmentAggregate;
using HRMS.Domain.Interfaces;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class DepartmentRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) :GenericRepository<Department>(context), IDepartmentRepository
{
    
    public async Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<object> GetAllDepartmentsWithManagerName(CancellationToken cancellationToken = default)
    {
        var departmentsWithManagers = await context.Departments
            .Include(d => d.Manager) // Assuming you have a navigation property
            .Select(d => new
            {
                DepartmentId = d.Id,
                DepartmentName = d.Name,
                Code = d.Code,
                Description = d.Description,
                ManagerId = d.ManagerId,
                Manager = d.Manager == null ? null : new
                {
                    d.Manager.Id,
                    FullName = d.Manager.Name.FirstName + " " + d.Manager.Name.LastName,
                    d.Manager.Email,
                    d.Manager.JobTitle
                }
            })
            .ToListAsync();
        
        return departmentsWithManagers;
    }
    
}