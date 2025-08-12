using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class EquipmentAssignment: Entity<Guid>
{
    public Guid EquipmentId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public Guid AssignedBy { get; private set; }
    public DateTime? ReturnedAt { get; private set; }
    public Guid? ReturnedBy { get; private set; }
    public EquipmentCondition? ReturnCondition { get; private set; }
    public string? ReturnNotes { get; private set; }
    public EquipmentAssignmentStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    
    private EquipmentAssignment() { }

    public EquipmentAssignment(Guid equipmentId, Guid employeeId, Guid assignedBy)
    {
        Id = Guid.NewGuid();
        EquipmentId = equipmentId;
        EmployeeId = employeeId;
        AssignedAt = DateTime.UtcNow;
        AssignedBy = assignedBy;
        Status = EquipmentAssignmentStatus.Assigned;
        CreatedAt = AssignedAt;
        UpdatedAt = AssignedAt;
    }
    
    public void MarkReturned(Guid returnedBy, EquipmentCondition returnCondition, string? returnNotes)
    {
        ReturnedAt = DateTime.UtcNow;
        ReturnedBy = returnedBy;
        ReturnCondition = returnCondition;
        ReturnNotes = returnNotes;
        Status = EquipmentAssignmentStatus.Returned;
        UpdatedAt = DateTime.UtcNow;
    }
}