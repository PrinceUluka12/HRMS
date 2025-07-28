using HRMS.Application.Features.Payroll.Dtos;
using HRMS.Application.Interfaces.Repositories;
using MediatR;

namespace HRMS.Application.Features.Payroll.Queries.GetPayrollSummaryByEmployeeId;

public record GetPayrollSummaryByEmployeeIdQuery(DateTime StartDate, DateTime EndDate, Guid EmployeeId)
    : IRequest<IEnumerable<EmployeePayrollSummaryDto>>;

public class GetPayrollSummaryReportQueryHandler(
    IEmployeeRepository employeeRepository,
    IPayrollRepository payrollRepository)
    : IRequestHandler<GetPayrollSummaryByEmployeeIdQuery, IEnumerable<EmployeePayrollSummaryDto>>
{
    public async Task<IEnumerable<EmployeePayrollSummaryDto>> Handle(GetPayrollSummaryByEmployeeIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var payrolls = await payrollRepository.GetByEmployeeIdAsync(request.EmployeeId, cancellationToken);
            if (payrolls == null || !payrolls.Any())
                return Enumerable.Empty<EmployeePayrollSummaryDto>();

            var filtered = payrolls
                .Where(p => p.PayPeriodStart == request.StartDate && p.PayPeriodEnd == request.EndDate);

            return filtered.Select(p => new EmployeePayrollSummaryDto
            {
                EmployeeId = p.EmployeeId,
                EmployeeName = $"{p.Employee?.Name?.FirstName} {p.Employee?.Name?.LastName}".Trim(),
                TotalGross = p.GrossSalary,
                TotalNet = p.NetSalary
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex); // Consider using a proper logging mechanism
            throw;
        }
    }

}