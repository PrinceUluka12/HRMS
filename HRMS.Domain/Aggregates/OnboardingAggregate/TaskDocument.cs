using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class TaskDocument : Entity<Guid>, IAggregateRoot
{
    public Guid OnboardingTaskId { get; private set; }
    public string Name { get; private set; }
    public string FilePath { get; private set; }
    public string FileType { get; private set; }
    public long FileSize { get; private set; }
    public DateTime UploadDate { get; private set; }
    public string UploadedBy { get; private set; }
    public bool IsRequired { get; private set; }
    public DocumentStatus Status { get; private set; }

    private TaskDocument() { }

    public TaskDocument(
        Guid onboardingTaskId,
        string name,
        string filePath,
        string fileType,
        long fileSize,
        string uploadedBy,
        bool isRequired)
    {
        OnboardingTaskId = onboardingTaskId;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        FilePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
        FileType = fileType ?? throw new ArgumentNullException(nameof(fileType));
        FileSize = fileSize;
        UploadDate = DateTime.UtcNow;
        UploadedBy = uploadedBy ?? throw new ArgumentNullException(nameof(uploadedBy));
        IsRequired = isRequired;
        Status = DocumentStatus.PendingReview;
    }

    public void Approve(string approvedBy)
    {
        Status = DocumentStatus.Approved;
    }

    public void Reject(string rejectedBy, string reason)
    {
        Status = DocumentStatus.Rejected;
    }

    public void UpdateFile(string newFilePath, string newFileType, long newFileSize, string updatedBy)
    {
        FilePath = newFilePath ?? throw new ArgumentNullException(nameof(newFilePath));
        FileType = newFileType ?? throw new ArgumentNullException(nameof(newFileType));
        FileSize = newFileSize;
        Status = DocumentStatus.PendingReview;
        UploadDate = DateTime.UtcNow;
        UploadedBy = updatedBy ?? throw new ArgumentNullException(nameof(updatedBy));
    }
}