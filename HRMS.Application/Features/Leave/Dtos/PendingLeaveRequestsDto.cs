namespace HRMS.Application.Features.Leave.Dtos;

public class PendingLeaveRequestsDto
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string LeaveType { get; set; }
    public string Reason { get; set; }
    public string Status { get; set; }
}