using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OffboardingAggregate;

public class AssetReturn : Entity<Guid>, IAggregateRoot
{
    public Guid ChecklistId { get; private set; }
    public string AssetName { get; private set; }
    public string AssetType { get; private set; }
    public string AssetId { get; private set; }
    public DateTime ReturnDate { get; private set; }
    public string ReceivedBy { get; private set; }
    public AssetCondition Condition { get; private set; }
    public string? Notes { get; private set; }

    private AssetReturn() { }

    public AssetReturn(
        Guid checklistId,
        string assetName,
        string assetType,
        string assetId,
        string receivedBy,
        AssetCondition condition,
        string? notes = null)
    {
        ChecklistId = checklistId;
        AssetName = assetName ?? throw new ArgumentNullException(nameof(assetName));
        AssetType = assetType ?? throw new ArgumentNullException(nameof(assetType));
        AssetId = assetId ?? throw new ArgumentNullException(nameof(assetId));
        ReturnDate = DateTime.UtcNow;
        ReceivedBy = receivedBy ?? throw new ArgumentNullException(nameof(receivedBy));
        Condition = condition;
        Notes = notes;
    }

    public void UpdateCondition(AssetCondition newCondition, string updatedBy, string? notes = null)
    {
        Condition = newCondition;
        ReceivedBy = updatedBy ?? throw new ArgumentNullException(nameof(updatedBy));
        Notes = notes;
    }
}