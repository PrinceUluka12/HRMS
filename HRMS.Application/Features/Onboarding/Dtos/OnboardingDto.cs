namespace HRMS.Application.Features.Onboarding.Dtos;

public class OnboardingDto
{
    public Guid EmployeeId { get;  set; }
    public string EmployeeName { get; set; }
    public string Email { get; set; }
    public string Position { get; set; }
    public string Department { get; set; }
    public DateTime StartDate { get; set; }
    public string Status { get;  set; }
    public int OverallProgress { get;  set; } // 0–100
    public int DaysRemaining { get;  set; }
    public IReadOnlyCollection<OnboardingStageDto> Stages { get; set; }
    public string ManagerName { get;  set; }
}