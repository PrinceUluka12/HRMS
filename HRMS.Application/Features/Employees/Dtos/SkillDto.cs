using HRMS.Domain.Aggregates.EmployeeAggregate;

namespace HRMS.Application.Features.Employees.Dtos;

public record SkillDto(
    Guid Id,
    string Name,
    SkillLevel Level,
    DateTime AcquiredDate);