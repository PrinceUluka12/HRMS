namespace HRMS.Application.Features.Benefits.Dtos;

public record BenefitDependentDto
{
    public Guid DependentId { get; init; }
    public string Name { get; init; }
    public string Relationship { get; init; }
    public DateTime DateOfBirth { get; init; }
    public bool IsStudent { get; init; }
    public bool IsDisabled { get; init; }
}