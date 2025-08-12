using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Onboarding.Dtos;

public class OnboardingStepDto
{
    public Guid Id { get; set; }
    public Guid WorkflowId { get; set; }
    public int StepNumber { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public StepType StepType { get; set; }
    public bool IsRequired { get; set; }
    public int EstimatedHours { get; set; }
    public List<Guid> Dependencies { get; set; } = new();
    public string? AssignedRole { get; set; }
    public List<Guid>? DocumentIds { get; set; }
    public List<EquipmentType>? EquipmentTypes { get; set; }
    public bool AutoComplete { get; set; }
    public List<int> ReminderDays { get; set; } = new();
}