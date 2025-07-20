using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.EmployeeAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface IEmployeeRepository : IGenericRepository<Employee>
{
    Task<Employee?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Employee?> GetByIdWithPositionAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Employee?> GetByAzureAdIdAsync(string azureAdId, CancellationToken cancellationToken = default);
}