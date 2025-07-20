namespace HRMS.Application.Features.Benefits.Dtos;

public record BenefitOptionDto
{
    public Guid OptionId { get; init; }
    public string OptionName { get; init; }
    public string OptionDescription { get; init; }
    public decimal AdditionalCost { get; init; }
}


