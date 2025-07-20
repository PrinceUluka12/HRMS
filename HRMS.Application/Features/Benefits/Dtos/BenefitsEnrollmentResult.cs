using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Benefits.Dtos;

public record BenefitsEnrollmentResult
{
    public Guid EnrollmentId { get; init; }
    public Guid EmployeeId { get; init; }
    public string EmployeeName { get; init; }
    public DateTime EnrollmentDate { get; init; }
    public DateTime EffectiveDate { get; init; }
    public DateTime? EndDate { get; init; }
    public EnrollmentStatus Status { get; init; }
    public string StatusMessage { get; init; }
    public decimal TotalEmployeeCost { get; init; }
    public decimal TotalEmployerCost { get; init; }
    public decimal TotalCost => TotalEmployeeCost + TotalEmployerCost;
    
    public IEnumerable<EnrolledBenefitDto> EnrolledBenefits { get; init; } = new List<EnrolledBenefitDto>();
    public IEnumerable<EnrollmentErrorDto> Errors { get; init; } = new List<EnrollmentErrorDto>();
    public IEnumerable<BenefitsDocumentDto> RequiredDocuments { get; init; } = new List<BenefitsDocumentDto>();

    // Helper properties
    public bool IsSuccess => Status == EnrollmentStatus.Completed && !Errors.Any();
    public bool RequiresAction => Status == EnrollmentStatus.PendingApproval || 
                                Status == EnrollmentStatus.PendingDocuments;
    public bool HasWarnings => Errors.Any(e => e.Severity == EnrollmentErrorSeverity.Warning);

    // Factory methods
    public static BenefitsEnrollmentResult Success(
        Guid enrollmentId,
        Guid employeeId,
        string employeeName,
        DateTime enrollmentDate,
        DateTime effectiveDate,
        IEnumerable<EnrolledBenefitDto> benefits,
        decimal employeeCost,
        decimal employerCost)
    {
        return new BenefitsEnrollmentResult
        {
            EnrollmentId = enrollmentId,
            EmployeeId = employeeId,
            EmployeeName = employeeName,
            EnrollmentDate = enrollmentDate,
            EffectiveDate = effectiveDate,
            Status = EnrollmentStatus.Completed,
            StatusMessage = "Enrollment completed successfully",
            TotalEmployeeCost = employeeCost,
            TotalEmployerCost = employerCost,
            EnrolledBenefits = benefits
        };
    }

    public static BenefitsEnrollmentResult Pending(
        Guid enrollmentId,
        Guid employeeId,
        string employeeName,
        DateTime enrollmentDate,
        DateTime effectiveDate,
        IEnumerable<EnrolledBenefitDto> benefits,
        IEnumerable<BenefitsDocumentDto> requiredDocuments,
        decimal estimatedEmployeeCost,
        decimal estimatedEmployerCost)
    {
        return new BenefitsEnrollmentResult
        {
            EnrollmentId = enrollmentId,
            EmployeeId = employeeId,
            EmployeeName = employeeName,
            EnrollmentDate = enrollmentDate,
            EffectiveDate = effectiveDate,
            Status = EnrollmentStatus.PendingApproval,
            StatusMessage = "Enrollment pending approval",
            TotalEmployeeCost = estimatedEmployeeCost,
            TotalEmployerCost = estimatedEmployerCost,
            EnrolledBenefits = benefits,
            RequiredDocuments = requiredDocuments
        };
    }

    public static BenefitsEnrollmentResult WithErrors(
        Guid enrollmentId,
        Guid employeeId,
        string employeeName,
        DateTime enrollmentDate,
        IEnumerable<EnrollmentErrorDto> errors,
        IEnumerable<EnrolledBenefitDto>? partialBenefits = null)
    {
        return new BenefitsEnrollmentResult
        {
            EnrollmentId = enrollmentId,
            EmployeeId = employeeId,
            EmployeeName = employeeName,
            EnrollmentDate = enrollmentDate,
            Status = EnrollmentStatus.Failed,
            StatusMessage = "Enrollment completed with errors",
            EnrolledBenefits = partialBenefits ?? new List<EnrolledBenefitDto>(),
            Errors = errors
        };
    }
}

