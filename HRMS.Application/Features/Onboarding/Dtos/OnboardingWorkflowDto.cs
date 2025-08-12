using HRMS.Domain.Aggregates.OnboardingAggregate;

namespace HRMS.Application.Features.Onboarding.Dtos;

public class OnboardingWorkflowDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = null!;
    public bool IsActive { get; set; }
    public int EstimatedDays { get; set; }
    public List<OnboardingStepDto> Steps { get; set; } = new();
    public string CreatedBy { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}