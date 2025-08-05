using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class OnboardingDocument : Entity<Guid>
{
    public string DocumentName { get; private set; }
    public string DocumentType { get; private set; }
    public DocumentStatus Status { get; private set; }

    public DateTime? UploadedDate { get; private set; }
    public DateTime? ReviewedDate { get; private set; }

    private OnboardingDocument() { } // For EF Core

    public OnboardingDocument(string documentName, string documentType)
    {
        Id = Guid.NewGuid();
        DocumentName = documentName;
        DocumentType = documentType;
        Status = DocumentStatus.Pending;
    }

    public void MarkAsSubmitted(DateTime uploadedDate)
    {
        UploadedDate = uploadedDate;
        Status = DocumentStatus.Submitted;
    }

    public void Approve(DateTime reviewedDate)
    {
        ReviewedDate = reviewedDate;
        Status = DocumentStatus.Approved;
    }

    public void Reject(DateTime reviewedDate)
    {
        ReviewedDate = reviewedDate;
        Status = DocumentStatus.Rejected;
    }
}