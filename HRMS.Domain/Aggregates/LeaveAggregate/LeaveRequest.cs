using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Exceptions;
using HRMS.Domain.SeedWork;

namespace HRMS.Domain.Aggregates.LeaveAggregate;

public class LeaveRequest : Entity<Guid>, IAggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public Employee Employee { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public LeaveType Type { get; private set; }
    public string Reason { get; private set; }
    public LeaveStatus Status { get; private set; }
    public DateTime RequestDate { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovedDate { get; private set; }
    public string? RejectionReason { get; private set; }
    public bool IsHalfDay { get; private set; }
    public decimal DurationDays => (decimal)(EndDate - StartDate).TotalDays + (IsHalfDay ? 0.5m : 1m);

    private LeaveRequest() { }

    public LeaveRequest(
        Guid employeeId,
        DateTime startDate,
        DateTime endDate,
        LeaveType type,
        string reason,
        LeaveStatus status = LeaveStatus.Pending)
    {
        EmployeeId = employeeId;
        StartDate = startDate;
        EndDate = endDate;
        Type = type;
        Reason = reason ?? throw new ArgumentNullException(nameof(reason));
        Status = status;
        RequestDate = DateTime.UtcNow;
    }

    public void Approve(string approvedBy)
    {
        if (Status != LeaveStatus.Pending)
            throw new DomainException("Only pending leave requests can be approved");

        Status = LeaveStatus.Approved;
        ApprovedBy = approvedBy ?? throw new ArgumentNullException(nameof(approvedBy));
        ApprovedDate = DateTime.Now;
             
    }

    public void Reject(string rejectedBy, string reason)
    {
        if (Status != LeaveStatus.Pending)
            throw new DomainException("Only pending leave requests can be rejected");

        Status = LeaveStatus.Rejected;
        ApprovedBy = rejectedBy ?? throw new ArgumentNullException(nameof(rejectedBy));
        RejectionReason = reason ?? throw new ArgumentNullException(nameof(reason));
    }

    public void Cancel()
    {
        if (Status != LeaveStatus.Pending && Status != LeaveStatus.Approved)
            throw new DomainException("Only pending or approved leave requests can be canceled");

        Status = LeaveStatus.Cancelled;
    }

    public void MarkAsHalfDay()
    {
        IsHalfDay = true;
    }
}