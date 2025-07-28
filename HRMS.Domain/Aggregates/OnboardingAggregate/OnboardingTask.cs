using HRMS.Domain.Enums;
using HRMS.Domain.Exceptions;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class OnboardingTask : Entity<Guid>, IAggregateRoot
{
    // Core task properties
    public Guid EmployeeId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime DueDate { get; private set; }
    public OnboardingTaskStatus Status { get; private set; }
    public OnboardingTaskCategory Category { get; private set; }
    public int Priority { get; private set; }
        
    // Task completion tracking
    public DateTime? CompletedDate { get; private set; }
    public string? CompletedBy { get; private set; }
    public string? CompletionNotes { get; private set; }

    // Task dependencies
    private readonly List<Guid> _dependencyTaskIds = new();
    public IReadOnlyCollection<Guid> DependencyTaskIds => _dependencyTaskIds.AsReadOnly();

    // Documents and resources
    private readonly List<TaskDocument> _documents = new();
    public IReadOnlyCollection<TaskDocument> Documents => _documents.AsReadOnly();

    // Audit fields
    public DateTime CreatedDate { get; private set; }
    public string CreatedBy { get; private set; }
    public DateTime? ModifiedDate { get; private set; }
    public string? ModifiedBy { get; private set; }

    // Private constructor for EF Core
    private OnboardingTask() { }

    public OnboardingTask(
        Guid employeeId,
        string title,
        string description,
        DateTime dueDate,
        OnboardingTaskCategory category,
        int priority,
        string createdBy)
    {
        EmployeeId = employeeId;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        DueDate = dueDate;
        Category = category;
        Priority = priority;
        Status = OnboardingTaskStatus.NotStarted;
        CreatedDate = DateTime.Now;
        CreatedBy = createdBy ?? throw new ArgumentNullException(nameof(createdBy));

        Validate();
    }

    // Domain methods
    public void StartTask(string startedBy)
    {
        if (Status != OnboardingTaskStatus.NotStarted)
            throw new DomainException("Only tasks that are not started can be started");

        
        Status = OnboardingTaskStatus.InProgress;
        ModifiedDate = DateTime.Now;
        ModifiedBy = startedBy ?? throw new ArgumentNullException(nameof(startedBy));
    }

    public void CompleteTask(string completedBy, string? notes = null)
    {
        if (Status == OnboardingTaskStatus.Completed)
            throw new DomainException("Task is already completed");

        if (HasUncompletedDependencies())
            throw new DomainException("Cannot complete task with uncompleted dependencies");

        Status = OnboardingTaskStatus.Completed;
        CompletedDate = DateTime.Now;
        CompletedBy = completedBy ?? throw new ArgumentNullException(nameof(completedBy));
        CompletionNotes = notes;
        ModifiedDate = DateTime.Now;
        ModifiedBy = completedBy;
    }

    public void ReopenTask(string reopenedBy, string reason)
    {
        if (Status != OnboardingTaskStatus.Completed)
            throw new DomainException("Only completed tasks can be reopened");

        Status = OnboardingTaskStatus.InProgress;
        CompletionNotes = $"Reopened: {reason}\n{CompletionNotes}";
        ModifiedDate = DateTime.UtcNow;
        ModifiedBy = reopenedBy ?? throw new ArgumentNullException(nameof(reopenedBy));
    }

    public void AddDependency(Guid taskId)
    {
        if (taskId == Id)
            throw new DomainException("Task cannot depend on itself");

        if (!_dependencyTaskIds.Contains(taskId))
        {
            _dependencyTaskIds.Add(taskId);
        }
    }

    public void RemoveDependency(Guid taskId)
    {
        _dependencyTaskIds.Remove(taskId);
    }

    public void AddDocument(TaskDocument document)
    {
        if (document == null) throw new ArgumentNullException(nameof(document));
        _documents.Add(document);
    }

    public void UpdateDetails(
        string title,
        string description,
        DateTime dueDate,
        OnboardingTaskCategory category,
        int priority,
        string modifiedBy)
    {
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Description = description ?? throw new ArgumentNullException(nameof(description));
        DueDate = dueDate;
        Category = category;
        Priority = priority;
        ModifiedDate = DateTime.UtcNow;
        ModifiedBy = modifiedBy ?? throw new ArgumentNullException(nameof(modifiedBy));

        Validate();
    }

    // Validation
    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Title))
            throw new DomainException("Title cannot be empty");

        if (DueDate < DateTime.UtcNow.Date)
            throw new DomainException("Due date cannot be in the past");

        if (Priority < 1 || Priority > 5)
            throw new DomainException("Priority must be between 1 and 5");
    }

    private bool HasUncompletedDependencies()
    {
        // In a real implementation, this would check against completed tasks
        return _dependencyTaskIds.Any();
    }
}