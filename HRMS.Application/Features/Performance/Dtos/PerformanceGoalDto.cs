using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Performance.Dtos;

public record PerformanceGoalDto(
    Guid Id,
    string Description,
    DateTime TargetDate,
    PerformanceGoalStatus Status,
    string? Comments);