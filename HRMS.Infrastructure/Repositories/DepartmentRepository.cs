using HRMS.Application.Features.Departments.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.DepartmentAggregate;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class DepartmentRepository(ApplicationDbContext context)
    : GenericRepository<Department>(context), IDepartmentRepository
{
    public async Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await context.Departments
            .FirstOrDefaultAsync(d => d.Name == name, cancellationToken);
    }

    public async Task<Department?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await context.Departments
            .FirstOrDefaultAsync(d => d.Code == code, cancellationToken);
    }

    public async Task<List<DepartmentWithManagerView>> GetWithManagerViewAsync(
        CancellationToken cancellationToken = default)
    {
        return  await context.Set<DepartmentWithManagerView>().ToListAsync();
    }

    public async Task<DepartmentWithManagerView> GetWithManagerByIdViewAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        return await context.Set<DepartmentWithManagerView>().FirstOrDefaultAsync(e => e.DepartmentId == Id);
    }
}