namespace HRMS.Domain.Enums;

public enum PerformanceReviewStatus
{
    Draft,
    PendingApproval,
    Approved,
    Rejected,
    Canceled
}

public enum PerformanceGoalStatus
{
    NotStarted,
    InProgress,
    Completed,
    Achieved,
    OnHold,
    Abandoned
}

public enum FeedbackType
{
    Strength,
    AreaForImprovement,
    GeneralComment,
    PeerFeedback,
    ManagerFeedback
}

public enum RatingScale
{
    Unsatisfactory = 1,
    NeedsImprovement = 2,
    MeetsExpectations = 3,
    ExceedsExpectations = 4,
    Outstanding = 5
}