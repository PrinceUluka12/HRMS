using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Features.Employees.Queries.GetEmployeeList;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Interfaces;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class EmployeeRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
    : GenericRepository<Employee>(context), IEmployeeRepository
{
    public async Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Employees
            /*.Include(e => e.Department)
            .Include(e => e.Position)
            .Include(e => e.Skills)
            .Include(e => e.Certifications)*/
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<List<Employee>> GetListWithDepAndPosAsync(CancellationToken cancellationToken = default)
    {
      

        return await context.Employees
            .Include(e => e.Department)
            .Include(e => e.Position)
            .ToListAsync(cancellationToken);
        ;
    }

    public async Task<Employee?> GetByIdWithPositionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Employees
            .Include(e => e.Position)
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Employee?> GetByAzureAdIdAsync(Guid azureAdId, CancellationToken cancellationToken = default)
    {
        return await context.Employees.Include(p => p.Position).Include(d => d.Department)
            .FirstOrDefaultAsync(e => e.AzureAdId == azureAdId, cancellationToken);
    }


    // Other repository methods...
}