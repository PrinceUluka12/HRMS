using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.LeaveAggregate;

public class LeavePolicy: Entity<Guid>, IAggregateRoot
{
    public Guid Id { get; private set; }
    public LeaveType LeaveType { get; private set; }
    public Guid? DepartmentId { get; private set; } // null for company-wide
    public Guid? PositionId { get; private set; }   // null for department-wide
    public int AnnualAllocation { get; private set; }
    public int MaxCarryOver { get; private set; }
    public AccrualMethod AccrualMethod { get; private set; }
    public decimal AccrualRate { get; private set; }
    public int MinRequestDays { get; private set; } // Minimum advance notice
    public int MaxConsecutiveDays { get; private set; }
    public bool RequiresApproval { get; private set; }
    public bool RequiresHRApproval { get; private set; } // For extended leave
    public bool IsActive { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private LeavePolicy() { }
    
    public LeavePolicy(
        Guid id,
        LeaveType leaveType,
        Guid? departmentId,
        Guid? positionId,
        int annualAllocation,
        int maxCarryOver,
        AccrualMethod accrualMethod,
        decimal accrualRate,
        int minRequestDays,
        int maxConsecutiveDays,
        bool requiresApproval,
        bool requiresHRApproval,
        bool isActive,
        DateTime effectiveDate,
        DateTime createdAt,
        DateTime updatedAt)
    {
        Id = id;
        LeaveType = leaveType;
        DepartmentId = departmentId;
        PositionId = positionId;
        AnnualAllocation = annualAllocation;
        MaxCarryOver = maxCarryOver;
        AccrualMethod = accrualMethod;
        AccrualRate = accrualRate;
        MinRequestDays = minRequestDays;
        MaxConsecutiveDays = maxConsecutiveDays;
        RequiresApproval = requiresApproval;
        RequiresHRApproval = requiresHRApproval;
        IsActive = isActive;
        EffectiveDate = effectiveDate;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
}