using HRMS.Application.Features.Onboarding.Dtos;
using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Equipments.Dtos;

public class EquipmentAssignmentDto
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public EquipmentDto Equipment { get; set; } = null!;
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = null!;
    public DateTime AssignedAt { get; set; }
    public string AssignedBy { get; set; } = null!;
    public DateTime? ReturnedAt { get; set; }
    public string? ReturnedBy { get; set; }
    public EquipmentCondition? ReturnCondition { get; set; }
    public string? ReturnNotes { get; set; }
    public EquipmentAssignmentStatus Status { get; set; }
}