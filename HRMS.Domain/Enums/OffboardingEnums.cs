namespace HRMS.Domain.Enums;

public enum OffboardingStatus
{
    Pending,
    InProgress,
    Completed,
    Cancelled
}

public enum OffboardingType
{
    Voluntary,
    Involuntary,
    Retirement,
    ContractEnd
}

public enum OffboardingTaskCategory
{
    AssetReturn,
    Security,
    Payroll,
    Benefits,
    Interview,
    Documentation,
    KnowledgeTransfer,
    Compliance
}

public enum OffboardingTaskStatus
{
    Pending,
    InProgress,
    Completed,
    OnHold
}

public enum AssetCondition
{
    Excellent,
    Good,
    Fair,
    Damaged,
    Lost
}