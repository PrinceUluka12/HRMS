using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class Equipment: Entity<Guid>, IAggregateRoot
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
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<EquipmentAssignment> _assignments = new();
    public IReadOnlyCollection<EquipmentAssignment> Assignments => _assignments.AsReadOnly();

    private Equipment() { }
    
    public Equipment(string serialNumber, string assetTag, EquipmentType type, string brand, string model, EquipmentCondition condition, DateTime purchaseDate, DateTime warrantyExpiry)
    {
        Id = Guid.NewGuid();
        SerialNumber = serialNumber ?? throw new ArgumentNullException(nameof(serialNumber));
        AssetTag = assetTag ?? throw new ArgumentNullException(nameof(assetTag));
        Type = type;
        Brand = brand ?? throw new ArgumentNullException(nameof(brand));
        Model = model ?? throw new ArgumentNullException(nameof(model));
        Condition = condition;
        PurchaseDate = purchaseDate;
        WarrantyExpiry = warrantyExpiry;
        Status = EquipmentStatus.Available;
        CreatedAt = DateTime.Now;
        UpdatedAt = CreatedAt;
    }
    
    public EquipmentAssignment AssignToEmployee(Guid employeeId, Guid assignedBy)
    {
        if (Status != EquipmentStatus.Available)
            throw new InvalidOperationException("Equipment is not available for assignment.");

        var assignment = new EquipmentAssignment(Id, employeeId, assignedBy);
        _assignments.Add(assignment);
        Status = EquipmentStatus.Assigned;
        UpdatedAt = DateTime.Now;
        return assignment;
    }

    public EquipmentAssignment MarkReturned(Guid returnedBy, EquipmentCondition returnCondition, string? returnNotes = null)
    {
        var currentAssignment = GetCurrentAssignment();
        if (currentAssignment == null)
            throw new InvalidOperationException("No active assignment found.");

        currentAssignment.MarkReturned(returnedBy, returnCondition, returnNotes);
        Status = EquipmentStatus.Available;
        UpdatedAt = DateTime.Now;
        return currentAssignment;
    }
    
    private EquipmentAssignment? GetCurrentAssignment() =>
        _assignments.FindLast(a => !a.ReturnedAt.HasValue);
}