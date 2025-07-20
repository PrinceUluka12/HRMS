namespace HRMS.Application.Features.Employees.Dtos;

public record CertificationDto(
    Guid Id,
    string Name,
    string IssuingOrganization,
    DateTime IssueDate,
    DateTime? ExpirationDate);