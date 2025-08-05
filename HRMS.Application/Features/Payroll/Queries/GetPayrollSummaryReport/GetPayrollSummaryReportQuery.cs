using HRMS.Application.Features.Payroll.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Payroll.Queries.GetPayrollSummaryReport;

public record GetPayrollSummaryReportQuery(DateTime StartDate, DateTime EndDate) 
    : IRequest<BaseResult<PayrollSummaryReportDto>>;

public class GetPayrollSummaryReportQueryHandler(
    IEmployeeRepository employeeRepository,
    IPayrollRepository payrollRepository,
    ILogger<GetPayrollSummaryReportQueryHandler> logger)
    : IRequestHandler<GetPayrollSummaryReportQuery, BaseResult<PayrollSummaryReportDto>>
{
    public async Task<BaseResult<PayrollSummaryReportDto>> Handle(GetPayrollSummaryReportQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var payrolls = await payrollRepository.GetByPeriodAsync(request.StartDate, request.EndDate, cancellationToken);

            if (payrolls == null || !payrolls.Any())
            {
                return BaseResult<PayrollSummaryReportDto>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"No payroll records found between {request.StartDate:yyyy-MM-dd} and {request.EndDate:yyyy-MM-dd}."));
            }

            decimal totalGross = 0, totalNet = 0, totalTax = 0, totalBenefits = 0;
            int employeeCount = 0;
            var employeeSummaries = new List<EmployeePayrollSummaryDto>();

            foreach (var payroll in payrolls)
            {
                totalGross += payroll.GrossSalary;
                totalNet += payroll.NetSalary;
                totalTax += payroll.TaxDeductions;
                totalBenefits += payroll.BenefitsDeductions;
                employeeCount++;

                var employeeName = payroll.Employee != null
                    ? $"{payroll.Employee.Name.FirstName} {payroll.Employee.Name.LastName}"
                    : "Unknown";

                employeeSummaries.Add(new EmployeePayrollSummaryDto
                {
                    EmployeeId = payroll.EmployeeId,
                    EmployeeName = employeeName,
                    TotalGross = payroll.GrossSalary,
                    TotalNet = payroll.NetSalary
                });
            }

            var summary = new PayrollSummaryReportDto
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TotalGrossPayroll = totalGross,
                TotalNetPayroll = totalNet,
                TotalTaxes = totalTax,
                TotalBenefits = totalBenefits,
                EmployeeCount = employeeCount,
                Payrolls = employeeSummaries
            };

            return BaseResult<PayrollSummaryReportDto>.Ok(summary);
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
