using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Benefits.Dtos;

public record EnrollmentErrorDto
{
    public string Code { get; init; }
    public string Message { get; init; }
    public EnrollmentErrorSeverity Severity { get; init; }
    public string? AffectedBenefitId { get; init; }
    public string? ResolutionSuggestion { get; init; }
}