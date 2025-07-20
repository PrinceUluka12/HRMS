using HRMS.Domain.Enums;
using HRMS.Domain.Exceptions;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OffboardingAggregate;

public class OffboardingChecklist : Entity<Guid>, IAggregateRoot
{
    // Core properties
    public Guid EmployeeId { get; private set; }
    public DateTime InitiationDate { get; private set; }
    public DateTime? CompletionDate { get; private set; }
    public OffboardingStatus Status { get; private set; }
    public OffboardingType Type { get; private set; }
    public string InitiatedBy { get; private set; }
    public string? CompletedBy { get; private set; }

    // Checklist items
    private readonly List<OffboardingTask> _tasks = new();
    public IReadOnlyCollection<OffboardingTask> Tasks => _tasks.AsReadOnly();

    // Exit details
    public DateTime LastWorkingDate { get; private set; }
    public string? ExitReason { get; private set; }
    public string? ExitInterviewNotes { get; private set; }
    public bool ExitInterviewConducted { get; private set; }

    // Asset tracking
    private readonly List<AssetReturn> _returnedAssets = new();
    public IReadOnlyCollection<AssetReturn> ReturnedAssets => _returnedAssets.AsReadOnly();

    // Access revocation
    private readonly List<AccessRevocation> _accessRevocations = new();
    public IReadOnlyCollection<AccessRevocation> AccessRevocations => _accessRevocations.AsReadOnly();

    // Audit fields
    public DateTime CreatedDate { get; private set; }
    public DateTime? ModifiedDate { get; private set; }

    // Private constructor for EF Core
    private OffboardingChecklist() { }

    public OffboardingChecklist(
        Guid employeeId,
        OffboardingType type,
        DateTime lastWorkingDate,
        string initiatedBy,
        string? exitReason = null)
    {
        EmployeeId = employeeId;
        Type = type;
        LastWorkingDate = lastWorkingDate;
        InitiatedBy = initiatedBy ?? throw new ArgumentNullException(nameof(initiatedBy));
        ExitReason = exitReason;
        Status = OffboardingStatus.Pending;
        InitiationDate = DateTime.UtcNow;
        CreatedDate = DateTime.UtcNow;

        Validate();
        InitializeDefaultTasks();
    }

    // Domain methods
    public void AddTask(OffboardingTask task)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));
        if (Status == OffboardingStatus.Completed)
            throw new DomainException("Cannot add tasks to completed checklist");

        _tasks.Add(task);
    }

    public void CompleteTask(Guid taskId, string completedBy, string? notes = null)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == taskId);
       // if (task != null)
            //throw new NotFoundException(nameof(OffboardingTask), taskId);

        task.MarkAsCompleted(completedBy, notes);
        ModifiedDate = DateTime.UtcNow;

        if (_tasks.All(t => t.Status == OffboardingTaskStatus.Completed))
        {
            CompleteChecklist(completedBy);
        }
    }

    public void AddReturnedAsset(AssetReturn asset)
    {
        if (asset == null) throw new ArgumentNullException(nameof(asset));
        _returnedAssets.Add(asset);
        ModifiedDate = DateTime.UtcNow;
    }

    public void AddAccessRevocation(AccessRevocation accessRevocation)
    {
        if (accessRevocation == null) throw new ArgumentNullException(nameof(accessRevocation));
        _accessRevocations.Add(accessRevocation);
        ModifiedDate = DateTime.UtcNow;
    }

    public void ConductExitInterview(string conductedBy, string notes)
    {
        ExitInterviewConducted = true;
        ExitInterviewNotes = notes ?? throw new ArgumentNullException(nameof(notes));
        ModifiedDate = DateTime.UtcNow;
    }

    public void UpdateExitDetails(DateTime lastWorkingDate, string? exitReason)
    {
        if (Status == OffboardingStatus.Completed)
            throw new DomainException("Cannot modify completed checklist");

        LastWorkingDate = lastWorkingDate;
        ExitReason = exitReason;
        ModifiedDate = DateTime.UtcNow;
        Validate();
    }

    private void CompleteChecklist(string completedBy)
    {
        Status = OffboardingStatus.Completed;
        CompletionDate = DateTime.UtcNow;
        CompletedBy = completedBy ?? throw new ArgumentNullException(nameof(completedBy));
        ModifiedDate = DateTime.UtcNow;
    }

    private void InitializeDefaultTasks()
    {
        // Add system-defined offboarding tasks based on type
        switch (Type)
        {
            case OffboardingType.Voluntary:
                AddTask(new OffboardingTask("Return company assets", "Return all assigned equipment", OffboardingTaskCategory.AssetReturn, true));
                AddTask(new OffboardingTask("Exit interview", "Complete exit interview with HR", OffboardingTaskCategory.Interview, true));
                break;
                
            case OffboardingType.Involuntary:
                AddTask(new OffboardingTask("Immediate access revocation", "Revoke all system accesses", OffboardingTaskCategory.Security, true));
                AddTask(new OffboardingTask("Asset recovery", "Recover all company property", OffboardingTaskCategory.AssetReturn, true));
                break;
                
            case OffboardingType.Retirement:
                AddTask(new OffboardingTask("Benefits consultation", "Review retirement benefits", OffboardingTaskCategory.Benefits, true));
                AddTask(new OffboardingTask("Knowledge transfer", "Complete knowledge transfer sessions", OffboardingTaskCategory.KnowledgeTransfer, true));
                break;
        }

        // Common tasks for all types
        AddTask(new OffboardingTask("Final paycheck", "Confirm final paycheck details", OffboardingTaskCategory.Payroll, true));
        AddTask(new OffboardingTask("Benefits termination", "Process benefits termination", OffboardingTaskCategory.Benefits, true));
    }

    private void Validate()
    {
        if (LastWorkingDate < DateTime.UtcNow.Date)
            throw new DomainException("Last working date cannot be in the past");

        if (Type == OffboardingType.Involuntary && string.IsNullOrWhiteSpace(ExitReason))
            throw new DomainException("Exit reason is required for involuntary offboarding");
    }
}