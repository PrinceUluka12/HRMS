namespace HRMS.Application.Features.Reports.Dtos;

public record EmployeePayrollSummaryDto(
    Guid EmployeeId,
    string EmployeeName,
    decimal TotalGross,
    decimal TotalNet);