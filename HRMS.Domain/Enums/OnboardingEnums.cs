namespace HRMS.Domain.Enums;

public enum OnboardingTaskStatus
{
    NotStarted,
    InProgress,
    Completed,
}

public enum OnboardingStage
{
    PreOnboarding,
    Day1,
    Week1To2,
    Day30To90,
    Completed
}

public enum OnboardingTaskCategory
{
    Documentation,
    ITSetup,
    HRSession,
    Training,
    ManagerMeeting,
    Compliance
}

public enum DocumentStatus
{
    PendingReview,
    Approved,
    Rejected,
    Archived
}

public enum TaskPriority
{
    Critical = 1,
    High = 2,
    Medium = 3,
    Low = 4,
    Optional = 5
}