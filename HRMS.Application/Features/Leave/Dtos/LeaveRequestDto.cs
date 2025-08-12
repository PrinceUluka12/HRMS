using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Leave.Dtos;

public class LeaveRequestDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LeaveType Type { get; set; }
    public string Reason { get; set; }
    public RequestStatus Status { get; set; }
    public DateTime RequestDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? RejectionReason { get; set; }
}