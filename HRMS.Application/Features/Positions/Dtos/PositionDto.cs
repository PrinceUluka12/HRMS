namespace HRMS.Application.Features.Positions.Dtos;

public record PositionDto(
    Guid Id,
    string Title,
    string Code,
    decimal BaseSalary,
    string Description,
    Guid DepartmentId);