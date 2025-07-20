using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Payroll.Dtos;

public record PayrollDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    DateTime PayPeriodStart,
    DateTime PayPeriodEnd,
    decimal GrossSalary,
    decimal TaxDeductions,
    decimal BenefitsDeductions,
    decimal NetSalary,
    PayrollStatus Status,
    DateTime ProcessedDate,
    string ProcessedBy);