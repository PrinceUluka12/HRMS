using HRMS.Domain.Aggregates.PayrollAggregate;
using HRMS.Domain.Enums;

namespace HRMS.Domain.Interfaces.Repositories;

public interface IPayrollItemRepository : IRepository<PayrollItem>
{
    Task<List<PayrollItem>> GetByPayrollIdAsync(Guid payrollId, CancellationToken cancellationToken = default);
    Task<List<PayrollItem>> GetByEmployeeAndTypeAsync(Guid employeeId, PayrollItemType type, DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task AddAsync(PayrollItem payrollItem);
    void Update(PayrollItem payrollItem);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}