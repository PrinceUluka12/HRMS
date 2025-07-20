namespace HRMS.Application.Features.Benefits.Dtos;

public record BenefitsDocumentDto
{
public Guid DocumentTypeId { get; init; }
public string DocumentName { get; init; }
public string Description { get; init; }
public bool IsRequired { get; init; }
public DateTime? ReceivedDate { get; init; }
public string? DocumentStatus { get; init; }
}
