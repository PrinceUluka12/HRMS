namespace HRMS.Domain.Enums;

public enum OnboardingStatus
{
    Pending,
    InProgress,
    Completed,
    Overdue
}

public enum DocumentStatus
{
    Pending,
    Submitted,
    Approved,
    Rejected
}

public enum OnboardingStageStatus
{
    Pending,
    InProgress,
    Completed,
    Overdue
}

public enum OnboardingTaskStatus
{
    Pending,
    InProgress,
    Completed,
    Overdue
}

public enum DocumentType
{
    PDF,
    DOCX,
    Contract,
    Policy,
    Handbook,
    Form,
    Other
}

public enum SignatureStatus
{
    Pending,
    Signed,
    Declined,
        Completed,
    Expired,
    Cancelled
}

public enum EquipmentType
{
    Laptop,
    Phone,
    Monitor,
    Other
}

public enum EquipmentCondition
{
    New,
    Good,
    Fair,
    Poor
}

public enum EquipmentStatus
{
    Available,
    Assigned,
    Maintenance
}

public enum EquipmentAssignmentStatus
{
    Assigned,
    Returned,
    Lost
}

public enum BuddyPairStatus
{
    Active,
    Inactive,
    Completed
}

public enum StepType
{
    Task,
    Document,
    Training
}

public enum StepStatus
{
    NotStarted,
    InProgress,
    Completed,
    Blocked
}

public enum NotificationType
{
    Email,
    SMS,
    Push,
    Info,
    Warning,
    Task,
    System
}

public enum NotificationTrigger
{
    StepOverdue,
    StepCompleted,
    WorkflowStarted
}

public enum NotificationStatus
{
    Pending,
    Sent,
    Failed,
    Unread,
    Read,
    Archived
}