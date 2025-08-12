using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Equipments.Dtos;

public class EquipmentDto
{
    public string SerialNumber { get; private set; }
    public string AssetTag { get; private set; }
    public EquipmentType Type { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public EquipmentCondition Condition { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public DateTime WarrantyExpiry { get; private set; }
    public EquipmentStatus Status { get; private set; }
    public string? Notes { get; private set; }
}