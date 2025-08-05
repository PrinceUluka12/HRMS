using HRMS.Application.Features.Payroll.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Payroll.Queries.GetPayrollSummaryByEmployeeId;

public record GetPayrollSummaryByEmployeeIdQuery(DateTime StartDate, DateTime EndDate, Guid EmployeeId)
    : IRequest<BaseResult<IEnumerable<EmployeePayrollSummaryDto>>>;

public class GetPayrollSummaryReportQueryHandler(
    IEmployeeRepository employeeRepository,
    IPayrollRepository payrollRepository,
    ILogger<GetPayrollSummaryReportQueryHandler> logger)
    : IRequestHandler<GetPayrollSummaryByEmployeeIdQuery, BaseResult<IEnumerable<EmployeePayrollSummaryDto>>>
{
    public async Task<BaseResult<IEnumerable<EmployeePayrollSummaryDto>>> Handle(
        GetPayrollSummaryByEmployeeIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var payrolls = await payrollRepository.GetByEmployeeIdAsync(request.EmployeeId, cancellationToken);

            if (payrolls == null || !payrolls.Any())
            {
                return BaseResult<IEnumerable<EmployeePayrollSummaryDto>>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"No payroll records found for employee ID {request.EmployeeId}.",
                    nameof(request.EmployeeId)
                ));
            }

            var filtered = payrolls
                .Where(p => p.PayPeriodStart.Date == request.StartDate.Date &&
                            p.PayPeriodEnd.Date == request.EndDate.Date)
                .ToList();

            if (!filtered.Any())
            {
                return BaseResult<IEnumerable<EmployeePayrollSummaryDto>>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"No payroll records found for employee ID {request.EmployeeId} within the specified date range.",
                    nameof(request.EmployeeId)
                ));
            }

            var result = filtered.Select(p => new EmployeePayrollSummaryDto
            {
                EmployeeId = p.EmployeeId,
                EmployeeName = $"{p.Employee?.Name?.FirstName ?? ""} {p.Employee?.Name?.LastName ?? ""}".Trim(),
                TotalGross = p.GrossSalary,
                TotalNet = p.NetSalary
            }).ToList();

            return BaseResult<IEnumerable<EmployeePayrollSummaryDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving payroll summary for employee ID {EmployeeId}", request.EmployeeId);
            return BaseResult<IEnumerable<EmployeePayrollSummaryDto>>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while retrieving payroll summary."
            ));
        }
    }
}
