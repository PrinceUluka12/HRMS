namespace HRMS.Application.Features.Onboarding.Dtos;

public class OnboardingTaskDto
{
    public Guid Id { get; set; }
    public string TaskName { get; set; }
    public string Description { get; set; }
    public string  Status { get;  set; }
    public string AssignedTo { get; set; } // e.g. HR, IT, Manager
    public DateTime DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? Notes { get;  set; }
}