namespace HRMS.Application.Features.Onboarding.Dtos;

public class OnboardingStageDto
{
    public string StageName { get; set; }
    public string Status { get; set; }
    public int Progress { get;  set; } // 0–100
    public DateTime DueDate { get;  set; }
    public IReadOnlyCollection<OnboardingTaskDto> Tasks  {get;  set; }

}

public enum OnboardingStageStatusDto
{
    Pending,
    InProgress,
    Completed,
    Overdue
}