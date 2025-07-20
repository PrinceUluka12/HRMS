namespace HRMS.Domain.Enums;

public enum EnrollmentStatus
{
    Draft,
    PendingApproval,
    PendingDocuments,
    Completed,
    Failed,
    Cancelled
}

public enum EnrollmentErrorSeverity
{
    Information,
    Warning,
    Error,
    Critical
}

public enum BenefitPlanType
{
    Medical,
    Dental,
    Vision,
    LifeInsurance,
    Disability,
    Retirement,
    FlexibleSpending,
    Other
}