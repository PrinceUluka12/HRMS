using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.DepartmentAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface IDepartmentRepository : IGenericRepository<Department>
{
    Task<Department?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    Task<object> GetAllDepartmentsWithManagerName(CancellationToken cancellationToken = default);
}