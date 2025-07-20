using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OffboardingAggregate;

public class AccessRevocation : Entity<Guid>, IAggregateRoot
{
    public Guid ChecklistId { get; private set; }
    public string SystemName { get; private set; }
    public string AccessType { get; private set; }
    public DateTime RevocationDate { get; private set; }
    public string RevokedBy { get; private set; }
    public bool IsRevoked { get; private set; }
    public string? VerificationMethod { get; private set; }

    private AccessRevocation() { }

    public AccessRevocation(
        Guid checklistId,
        string systemName,
        string accessType,
        string revokedBy,
        string? verificationMethod = null)
    {
        ChecklistId = checklistId;
        SystemName = systemName ?? throw new ArgumentNullException(nameof(systemName));
        AccessType = accessType ?? throw new ArgumentNullException(nameof(accessType));
        RevocationDate = DateTime.UtcNow;
        RevokedBy = revokedBy ?? throw new ArgumentNullException(nameof(revokedBy));
        IsRevoked = true;
        VerificationMethod = verificationMethod;
    }

    public void MarkAsVerified(string verifiedBy, string method)
    {
        VerificationMethod = method ?? throw new ArgumentNullException(nameof(method));
        RevokedBy = verifiedBy ?? throw new ArgumentNullException(nameof(verifiedBy));
    }
}