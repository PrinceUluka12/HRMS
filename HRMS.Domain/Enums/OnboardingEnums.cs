namespace HRMS.Domain.Enums;

public enum OnboardingTaskStatus
{
    NotStarted,
    InProgress,
    Completed,
    OnHold,
    Cancelled
}

public enum OnboardingTaskCategory
{
    Documentation,
    Training,
    Equipment,
    Access,
    Compliance,
    Orientation,
    HRPaperwork,
    ITSetup
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