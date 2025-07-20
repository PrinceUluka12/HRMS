using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Leave.Dtos;

public record LeaveRequestDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    DateTime StartDate,
    DateTime EndDate,
    LeaveType Type,
    string Reason,
    LeaveStatus Status,
    DateTime RequestDate,
    string? ApprovedBy,
    DateTime? ApprovedDate,
    string? RejectionReason);