using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Payroll.Dtos;

public record PayrollItemDto(
    Guid Id,
    string Description,
    decimal Amount,
    PayrollItemType Type);