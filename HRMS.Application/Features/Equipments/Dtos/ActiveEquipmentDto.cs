namespace HRMS.Application.Features.Equipments.Dtos;

public class ActiveEquipmentDto
{
    public Guid EquipmentAssignmentId { get; set; }
    public Guid EquipmentId { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime AssignedAt { get; set; }
    public string AssignedBy { get; set; }
    public string SerialNumber { get; set; }
    public string AssetTag { get; set; }
    public string Type { get;  set; }
    public string Brand { get; set; }
    public string Model { get; set; }
}