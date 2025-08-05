using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Reports.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Reports.Queries.GetPayrollSummaryReport;

public record GetPayrollSummaryReportQuery(DateTime StartDate, DateTime EndDate)
    : IRequest<BaseResult<PayrollSummaryReportDto>>;

public class GetPayrollSummaryReportQueryHandler(
    IPayrollRepository payrollRepository,
    ILogger<GetPayrollSummaryReportQueryHandler> logger)
    : IRequestHandler<GetPayrollSummaryReportQuery, BaseResult<PayrollSummaryReportDto>>
{
    public async Task<BaseResult<PayrollSummaryReportDto>> Handle(
        GetPayrollSummaryReportQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var payrolls = await payrollRepository.GetPayrollFromDateAsync(request.StartDate, request.EndDate, cancellationToken);

            if (payrolls == null || !payrolls.Any())
            {
                return BaseResult<PayrollSummaryReportDto>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"No payroll records found between {request.StartDate:yyyy-MM-dd} and {request.EndDate:yyyy-MM-dd}."
                ));
            }

            var report = new PayrollSummaryReportDto(
                StartDate: request.StartDate,
                EndDate: request.EndDate,
                TotalGrossPayroll: payrolls.Sum(p => p.GrossSalary),
                TotalNetPayroll: payrolls.Sum(p => p.NetSalary),
                TotalTaxes: payrolls.Sum(p => p.TaxDeductions),
                TotalBenefits: payrolls.Sum(p => p.BenefitsDeductions),
                EmployeeCount: payrolls.Select(p => p.EmployeeId).Distinct().Count(),
                Payrolls: payrolls
                    .Where(p => p.Employee != null)
                    .GroupBy(p => p.Employee)
                    .Select(g => new EmployeePayrollSummaryDto(
                        EmployeeId: g.Key.Id,
                        EmployeeName: $"{g.Key.Name.FirstName} {g.Key.Name.LastName}",
                        TotalGross: g.Sum(p => p.GrossSalary),
                        TotalNet: g.Sum(p => p.NetSalary)
                    ))
                    .ToList()
            );

            return BaseResult<PayrollSummaryReportDto>.Ok(report);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating payroll summary report for period {Start} to {End}", request.StartDate, request.EndDate);

            return BaseResult<PayrollSummaryReportDto>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while generating the payroll summary report."
            ));
        }
    }
}
