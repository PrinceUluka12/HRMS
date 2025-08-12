using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Leave.Dtos;

public record TimeOffBalanceDto(Guid Id,
    Guid EmployeeId,
    LeaveType LeaveType,
    decimal TotalAllowed,
    decimal Used,
    decimal Pending,
    decimal CarryOver,
    decimal AccrualRate,
    DateTime LastAccrualDate,
    int PolicyYear,
    DateTime CreatedAt,
    DateTime UpdatedAt);