using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.PayrollAggregate;

public class PayrollItem : Entity<Guid>, IAggregateRoot
{
    public Guid PayrollId { get; private set; }
    public string Description { get; private set; }
    public decimal Amount { get; private set; }
    public PayrollItemType Type { get; private set; }
    public string? ReferenceId { get; private set; }
    public DateTime EffectiveDate { get; private set; }

    private PayrollItem() { }

    public PayrollItem(
        Guid payrollId,
        string description,
        decimal amount,
        PayrollItemType type,
        string? referenceId = null)
    {
        PayrollId = payrollId;
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Amount = amount;
        Type = type;
        ReferenceId = referenceId;
        EffectiveDate = DateTime.UtcNow;
    }

    public void UpdateAmount(decimal newAmount)
    {
        Amount = newAmount;
    }

    public void UpdateReference(string referenceId)
    {
        ReferenceId = referenceId;
    }
}