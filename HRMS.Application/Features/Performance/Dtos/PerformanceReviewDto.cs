using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Performance.Dtos;

public record PerformanceReviewDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    Guid ReviewerId,
    string ReviewerName,
    DateTime ReviewDate,
    DateTime? NextReviewDate,
    PerformanceRating OverallRating,
    string Comments,
    List<PerformanceGoalDto> Goals);