using HRMS.Application.Features.Payroll.Dtos;
using HRMS.Application.Interfaces.Repositories;
using MediatR;

namespace HRMS.Application.Features.Payroll.Queries.GetPayrollSummaryReport;

public record GetPayrollSummaryReportQuery(DateTime StartDate, DateTime EndDate) : IRequest<PayrollSummaryReportDto>;

public class GetPayrollSummaryReportQueryHandler(
    IEmployeeRepository employeeRepository,
    IPayrollRepository payrollRepository) : IRequestHandler<GetPayrollSummaryReportQuery, PayrollSummaryReportDto>
{
    public async Task<PayrollSummaryReportDto> Handle(GetPayrollSummaryReportQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var payrolls =
                await payrollRepository.GetByPeriodAsync(request.StartDate, request.EndDate, cancellationToken);
            decimal TotalGrossPayroll = 0;
            decimal TotalNetPayroll = 0;
            decimal TotalTaxes = 0;
            decimal TotalBenefits = 0;
            int EmployeeCount = 0;
            List<EmployeePayrollSummaryDto> employeePayroll = new();
            foreach (var payroll in payrolls)
            {
                TotalGrossPayroll += payroll.GrossSalary;
                TotalNetPayroll += payroll.NetSalary;
                TotalTaxes += payroll.TaxDeductions;
                TotalBenefits += payroll.BenefitsDeductions;
                EmployeeCount++;

                var employee = new EmployeePayrollSummaryDto()
                {
                    EmployeeId = payroll.EmployeeId,
                    EmployeeName = $"{payroll.Employee.Name.FirstName}  {payroll.Employee.Name.LastName}",
                    TotalGross = payroll.GrossSalary,
                    TotalNet = payroll.NetSalary
                };
                employeePayroll.Add(employee);
            }

            var data = new PayrollSummaryReportDto()
            {
                TotalNetPayroll = TotalNetPayroll,
                TotalGrossPayroll = TotalGrossPayroll,
                TotalBenefits = TotalBenefits,
                TotalTaxes = TotalTaxes,
                EmployeeCount = EmployeeCount,
                Payrolls = employeePayroll,
                StartDate = request.StartDate,
                EndDate = request.EndDate
            };
            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}