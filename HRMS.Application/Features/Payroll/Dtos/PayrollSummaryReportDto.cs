namespace HRMS.Application.Features.Payroll.Dtos;

public class PayrollSummaryReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public decimal TotalGrossPayroll { get; set; }
    public decimal TotalNetPayroll { get; set; }
    public decimal TotalTaxes { get; set; }
    public decimal TotalBenefits { get; set; }

    public int EmployeeCount { get; set; }

    public List<EmployeePayrollSummaryDto> Payrolls { get; set; } = new();
}
public class EmployeePayrollSummaryDto
{
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; }

    public decimal TotalGross { get; set; }
    public decimal TotalNet { get; set; }
}
