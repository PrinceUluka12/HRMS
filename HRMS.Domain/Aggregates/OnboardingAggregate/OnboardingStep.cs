using HRMS.Domain.Enums;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.OnboardingAggregate;

public class OnboardingStep : Entity<Guid>
{
    public Guid WorkflowId { get; private set; }
    public int StepNumber { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public StepType StepType { get; private set; }
    public bool IsRequired { get; private set; }
    public int EstimatedHours { get; private set; }
    public List<Guid>? Dependencies { get; private set; }
    public string? AssignedRole { get; private set; }
    public List<Guid>? DocumentIds { get; private set; }
    public List<EquipmentType>? EquipmentTypes { get; private set; }
    public bool AutoComplete { get; private set; }
    public List<int>? ReminderDays { get; private set; }
    
    private OnboardingStep() { }

    public OnboardingStep(Guid workflowId, int stepNumber, string title, StepType stepType, bool isRequired, int estimatedHours)
    {
        Id = Guid.NewGuid();
        WorkflowId = workflowId;
        StepNumber = stepNumber;
        Title = title ?? throw new ArgumentNullException(nameof(title));
        StepType = stepType;
        IsRequired = isRequired;
        EstimatedHours = estimatedHours;
    }
    
    
}