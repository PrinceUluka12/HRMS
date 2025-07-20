using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Onboarding.Dtos;

public record OnboardingTaskDto(
    Guid Id,
    Guid EmployeeId,
    string Title,
    string Description,
    DateTime DueDate,
    OnboardingTaskStatus Status,
    string? CompletedBy,
    DateTime? CompletedDate);