using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.PayrollAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface IPayrollRepository : IGenericRepository<Payroll>
{
    Task<List<Payroll>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);

    Task<List<Payroll>> GetByPeriodAsync(DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken = default);

    Task<List<Payroll>> GetPayrollFromDateAsync(DateTime startDate, DateTime endDate,
        CancellationToken cancellationToken = default);
}