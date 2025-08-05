using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Departments.Dtos;
using HRMS.Domain.Aggregates.DepartmentAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken = default);

    Task<Department> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<List<DepartmentWithManagerView>> GetWithManagerViewAsync(CancellationToken cancellationToken = default);
    Task<DepartmentWithManagerView> GetWithManagerByIdViewAsync(Guid Id,CancellationToken cancellationToken = default);
}