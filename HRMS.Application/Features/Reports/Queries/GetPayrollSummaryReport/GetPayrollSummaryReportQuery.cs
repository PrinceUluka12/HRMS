using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Reports.Dtos;
using HRMS.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Features.Reports.Queries.GetPayrollSummaryReport;

public record GetPayrollSummaryReportQuery(
    DateTime StartDate,
    DateTime EndDate) : IRequest<PayrollSummaryReportDto>;

public class GetPayrollSummaryReportQueryHandler(IPayrollRepository payrollRepository)
    : IRequestHandler<GetPayrollSummaryReportQuery, PayrollSummaryReportDto>
{
    

    public async Task<PayrollSummaryReportDto> Handle(
        GetPayrollSummaryReportQuery request,
        CancellationToken cancellationToken)
    {
        var payrolls = await payrollRepository.GetPayrollFromDateAsync(request.StartDate, request.EndDate, cancellationToken);

        var report = new PayrollSummaryReportDto(
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            TotalGrossPayroll: payrolls.Sum(p => p.GrossSalary),
            TotalNetPayroll: payrolls.Sum(p => p.NetSalary),
            TotalTaxes: payrolls.Sum(p => p.TaxDeductions),
            TotalBenefits: payrolls.Sum(p => p.BenefitsDeductions),
            EmployeeCount: payrolls.Select(p => p.EmployeeId).Distinct().Count(),
            Payrolls: payrolls.GroupBy(p => p.Employee)
                .Select(g => new EmployeePayrollSummaryDto(
                    EmployeeId: g.Key.Id,
                    EmployeeName: $"{g.Key.Name.FirstName} {g.Key.Name.LastName}",
                    TotalGross: g.Sum(p => p.GrossSalary),
                    TotalNet: g.Sum(p => p.NetSalary)
                )).ToList()
        );

        return report;
    }
}