using HRMS.Domain.Enums;
using HRMS.Domain.Exceptions;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OffboardingAggregate;

public class OffboardingTask : Entity<Guid>,IAggregateRoot
{
    public Guid ChecklistId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public OffboardingTaskCategory Category { get; private set; }
    public OffboardingTaskStatus Status { get; private set; }
    public bool IsMandatory { get; private set; }
    public DateTime? CompletedDate { get; private set; }
    public string? CompletedBy { get; private set; }
    public string? CompletionNotes { get; private set; }
    public DateTime CreatedDate { get; private set; }

    private OffboardingTask() { }

    public OffboardingTask(
        string title,
        string description,
        OffboardingTaskCategory category,
        bool isMandatory)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        Category = category;
        Status = OffboardingTaskStatus.Pending;
        IsMandatory = isMandatory;
        CreatedDate = DateTime.UtcNow;
    }

    public void MarkAsCompleted(string completedBy, string? notes = null)
    {
        if (Status == OffboardingTaskStatus.Completed)
            throw new DomainException("Task is already completed");

        Status = OffboardingTaskStatus.Completed;
        CompletedDate = DateTime.UtcNow;
        CompletedBy = completedBy ?? throw new ArgumentNullException(nameof(completedBy));
        CompletionNotes = notes;
    }

    public void ReopenTask(string reopenedBy, string reason)
    {
        if (Status != OffboardingTaskStatus.Completed)
            throw new DomainException("Only completed tasks can be reopened");

        Status = OffboardingTaskStatus.Pending;
        CompletionNotes = $"Reopened by {reopenedBy}: {reason}\n{CompletionNotes}";
        CompletedDate = null;
        CompletedBy = null;
    }
}