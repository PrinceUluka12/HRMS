using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Leave.Dtos;

public record LeavePolicyDto(Guid Id,
    LeaveType LeaveType,
    Guid? DepartmentId,
    Guid? PositionId,
    int AnnualAllocation,
    int MaxCarryOver,
    AccrualMethod AccrualMethod,
    decimal AccrualRate,
    int MinRequestDays,
    int MaxConsecutiveDays,
    bool RequiresApproval,
    bool RequiresHRApproval,
    List<DateTime> BlackoutDates,
    bool IsActive,
    DateTime EffectiveDate,
    DateTime CreatedAt,
    DateTime UpdatedAt);