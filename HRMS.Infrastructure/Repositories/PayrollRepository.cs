using HRMS.Application.Common.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.PayrollAggregate;
using HRMS.Domain.Interfaces;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class PayrollRepository(ApplicationDbContext context): GenericRepository<Payroll>(context),IPayrollRepository
{
    

    public async Task<List<Payroll>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default)
    {
       var data = await context.Payrolls.AsNoTracking().Include(e => e.Employee).Where(p => p.EmployeeId == employeeId).ToListAsync(cancellationToken);
       return data;
    }

    public async Task<List<Payroll>> GetByPeriodAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await context.Payrolls
            .Include(p => p.Employee)
            .AsNoTracking()
            .Where(p => p.PayPeriodStart <= endDate && p.PayPeriodEnd >= startDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Payroll>> GetPayrollFromDateAsync(
        DateTime startDate, 
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        var payrolls = await context.Payrolls
            .Where(p => p.PayPeriodStart >= startDate && p.PayPeriodEnd <= endDate)
            .Include(p => p.Employee)
            .ToListAsync(cancellationToken);
        
        return payrolls;
    }
}