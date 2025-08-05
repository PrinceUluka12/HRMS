namespace HRMS.Application.Features.Employees.Dtos;

public class EmployeeOnboardingDetailsDto
{
    public Guid EmployeeId { get; set; }
    public string EmployeeNumber { get; set; }
    public PersonalInfoDto PersonalInfo { get; set; }
    public string OnboardingStatus { get; set; } // Could be enum
    public int DaysRemaining { get; set; }
    public int OverallProgress { get; set; } // 0-100
    public List<OnboardingStageDto> Stages { get; set; } = new();
    public List<OnboardingDocumentDto> Documents { get; set; } = new();
    public DateTime LastActivity { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class PersonalInfoDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateTime StartDate { get; set; }
    public string Position { get; set; }
    public string Department { get; set; }
    public string Manager { get; set; }
}

public class OnboardingStageDto
{
    public Guid StageId { get; set; }
    public string StageName { get; set; }
    public string Status { get; set; } // Could be enum
    public int Progress { get; set; } // 0-100
    public DateTime DueDate { get; set; }
    public List<OnboardingTaskDto> Tasks { get; set; } = new();
}

public class OnboardingTaskDto
{
    public Guid TaskId { get; set; }
    public string TaskName { get; set; }
    public string Description { get; set; }
    public string Status { get; set; } // Could be enum
    public string AssignedTo { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? Notes { get; set; }
}

public class OnboardingDocumentDto
{
    public Guid DocumentId { get; set; }
    public string DocumentName { get; set; }
    public string DocumentType { get; set; }
    public string Status { get; set; } // Could be enum
    public DateTime? UploadedDate { get; set; }
    public DateTime? ReviewedDate { get; set; }
}
