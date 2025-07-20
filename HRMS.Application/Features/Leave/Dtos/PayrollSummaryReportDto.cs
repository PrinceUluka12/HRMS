namespace HRMS.Application.Features.Reports.Dtos;

public record PayrollSummaryReportDto(
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalGrossPayroll,
    decimal TotalNetPayroll,
    decimal TotalTaxes,
    decimal TotalBenefits,
    int EmployeeCount,
    List<EmployeePayrollSummaryDto> Payrolls);