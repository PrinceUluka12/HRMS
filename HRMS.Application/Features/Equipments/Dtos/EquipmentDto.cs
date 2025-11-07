using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Equipments.Dtos;

public class EquipmentDto
{
    public string SerialNumber { get;  set; }
    public string AssetTag { get;  set; }
    public EquipmentType Type { get;  set; }
    public string Brand { get;  set; }
    public string Model { get;  set; }
    public EquipmentCondition Condition { get;  set; }
    public DateTime PurchaseDate { get;  set; }
    public DateTime WarrantyExpiry { get;  set; }
    public EquipmentStatus Status { get;  set; }
    public string? Notes { get;  set; }
}