using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class OnboardingTask : Entity<Guid>
{
    public string TaskName { get; private set; }
    public string Description { get; private set; }
    public OnboardingTaskStatus Status { get; private set; }
    public string AssignedTo { get; private set; } // e.g. HR, IT, Manager
    public DateTime DueDate { get; private set; }
    public DateTime? CompletedDate { get; private set; }
    public string? Notes { get; private set; }

    private OnboardingTask() { } // EF Core

    public OnboardingTask(string taskName, string description, string assignedTo, DateTime dueDate)
    {
        Id = Guid.NewGuid();
        TaskName = taskName;
        Description = description;
        AssignedTo = assignedTo;
        DueDate = dueDate;
        Status = OnboardingTaskStatus.Pending;
    }

    public void MarkInProgress()
    {
        Status = OnboardingTaskStatus.InProgress;
    }

    public void Complete(DateTime completedDate, string? notes = null)
    {
        Status = OnboardingTaskStatus.Completed;
        CompletedDate = completedDate;
        Notes = notes;
    }

    public void MarkOverdue()
    {
        if (Status != OnboardingTaskStatus.Completed && DueDate.Date < DateTime.UtcNow.Date)
            Status = OnboardingTaskStatus.Overdue;
    }
}