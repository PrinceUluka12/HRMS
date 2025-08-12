using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class OnboardingWorkflow: Entity<Guid>, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid DepartmentId { get; private set; }
    public bool IsActive { get; private set; }
    public int EstimatedDays { get; private set; }
    public Guid CreatedBy { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private readonly List<OnboardingStep> _steps = new();
    public IReadOnlyCollection<OnboardingStep> Steps => _steps.AsReadOnly();

    private OnboardingWorkflow() { }
    
    public OnboardingWorkflow(string name, Guid departmentId, int estimatedDays, Guid createdBy, string? description = null)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        DepartmentId = departmentId;
        EstimatedDays = estimatedDays;
        CreatedBy = createdBy;
        Description = description;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }
    
    public void AddStep(OnboardingStep step)
    {
        if (step == null) throw new ArgumentNullException(nameof(step));
        _steps.Add(step);
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateStatus(bool isActive)
    {
        IsActive = isActive;
        UpdatedAt = DateTime.UtcNow;
    }

}