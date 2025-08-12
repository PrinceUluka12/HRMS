using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class DigitalSignature: Entity<Guid>, IAggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public Guid DocumentId { get; private set; }
    public string DocumentName { get; private set; }
    public DocumentType DocumentType { get; private set; }
    public string SignatureData { get; private set; } // Base64
    public DateTime? SignedAt { get; private set; }
    public string? IpAddress { get; private set; }
    public SignatureStatus Status { get; private set; }
    public DateTime? RequiredBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private DigitalSignature() { } // EF Core
    
    
    public DigitalSignature(Guid employeeId, Guid documentId, string documentName, DocumentType documentType, string signatureData, DateTime? requiredBy = null)
    {
        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        DocumentId = documentId;
        DocumentName = documentName ?? throw new ArgumentNullException(nameof(documentName));
        DocumentType = documentType;
        SignatureData = signatureData ?? throw new ArgumentNullException(nameof(signatureData));
        Status = SignatureStatus.Pending;
        RequiredBy = requiredBy;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    
    public void MarkSigned(string ipAddress)
    {
        Status = SignatureStatus.Signed;
        SignedAt = DateTime.UtcNow;
        IpAddress = ipAddress;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Decline(string ipAddress)
    {
        Status = SignatureStatus.Declined;
        SignedAt = DateTime.UtcNow;
        IpAddress = ipAddress;
        UpdatedAt = DateTime.UtcNow;
    }
}