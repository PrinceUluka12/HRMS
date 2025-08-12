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
    public RequestStatus Status { get; private set; }
    public string Reason { get; private set; }
    public DateTime RequestDate { get; private set; }
    public string? ApprovedBy { get; private set; }
    public DateTime? ApprovedDate { get; private set; }
    public DateTime? ReviewedAt { get; private set; }
    public string? ReviewedBy { get; private set; }
    public string? ReviewComments { get; private set; }
    public string? RejectionReason { get; private set; }
    public string[]? Attachments { get; private set; }
    public bool IsHalfDay { get; private set; }
    public decimal DurationDays => (decimal)(EndDate - StartDate).TotalDays + (IsHalfDay ? 0.5m : 1m);
    private readonly List<ApprovalHistory> _approvalHistories = new();
    public IReadOnlyCollection<ApprovalHistory> ApprovalHistories => _approvalHistories.AsReadOnly();

    private LeaveRequest() { }

    public LeaveRequest(
        Guid employeeId,
        DateTime startDate,
        DateTime endDate,
        LeaveType type,
        string reason
        )
    {
        if (startDate.Date > endDate.Date)
            throw new DomainException("Start date cannot be after end date.");
        
        EmployeeId = employeeId;
        StartDate = startDate;
        EndDate = endDate;
        Type = type;
        Reason = reason ?? throw new ArgumentNullException(nameof(reason));
        Status = RequestStatus.Draft;
        RequestDate = DateTime.UtcNow;
        IsHalfDay = false;
        Attachments = Array.Empty<string>();
    }

    // Approve request by approverType (manager, hr, admin)
    public void Approve(ApproverType approverType, Guid approverId, string approverName, string? comments = null)
    {
        var expectedStatus = GetExpectedStatusForApprover(approverType);

        if (Status != expectedStatus)
            throw new DomainException($"Request is not pending {approverType} approval.");

        AddApprovalHistory(ActionType.Approved, approverType, approverId, approverName, comments);

        // Update status according to approval step
        switch (approverType)
        {
            case ApproverType.Manager:
                ChangeStatus(RequestStatus.PendingHr, ApproverType.HR, approverName, comments);
                break;

            case ApproverType.HR:
                ChangeStatus(RequestStatus.Approved, null, approverName, comments);
                ApprovedBy = approverName;
                ApprovedDate = DateTime.UtcNow;
                break;

            case ApproverType.Admin:
                ChangeStatus(RequestStatus.Approved, null, approverName, comments);
                ApprovedBy = approverName;
                ApprovedDate = DateTime.UtcNow;
                break;

            default:
                throw new DomainException("Unknown approver type.");
        }
    }
    
    public void Submit()
    {
        if (Status != RequestStatus.Draft)
            throw new DomainException("Only draft requests can be submitted.");

        ChangeStatus(RequestStatus.PendingManager, ApproverType.Manager, null, "Request submitted");
    }

    // Reject request by approver
    public void Reject(ApproverType approverType, Guid approverId, string approverName, string reason)
    {
        var expectedStatus = GetExpectedStatusForApprover(approverType);

        if (Status != expectedStatus)
            throw new DomainException($"Request is not pending {approverType} approval.");

        AddApprovalHistory(ActionType.Denied, approverType, approverId, approverName, reason);

        ChangeStatus(RequestStatus.Denied, null, approverName, reason);
        RejectionReason = reason;
    }


    public void Cancel(string cancelledBy)
    {
        if (Status == RequestStatus.Approved || Status == RequestStatus.Denied || Status == RequestStatus.Cancelled)
            throw new DomainException("Cannot cancel a finalized leave request.");

        AddApprovalHistory(ActionType.Cancelled, ApproverType.Manager, Guid.Empty, cancelledBy, "Request cancelled");

        ChangeStatus(RequestStatus.Cancelled, null, cancelledBy, "Cancelled by user");
    }

    public void MarkAsHalfDay()
    {
        if ((EndDate - StartDate).TotalDays > 0)
            throw new DomainException("Half day leave can only be marked if start and end date are the same.");

        IsHalfDay = true;
    }
    
    public void AddAttachments(params string[] attachments)
    {
        if (attachments == null || attachments.Length == 0)
            return;

        if (Attachments == null)
            Attachments = attachments;
        else
            Attachments = CombineArrays(Attachments, attachments);
    }
    private static string[] CombineArrays(string[] arr1, string[] arr2)
    {
        var result = new string[arr1.Length + arr2.Length];
        arr1.CopyTo(result, 0);
        arr2.CopyTo(result, arr1.Length);
        return result;
    }
    
    // Private helper to change status and record history
    private void ChangeStatus(RequestStatus newStatus, ApproverType? nextApproverType, string? actorName, string? comments)
    {
        var previousStatus = Status;
        Status = newStatus;

        // Optionally set ReviewedAt etc.
        ReviewedAt = DateTime.UtcNow;
        ReviewedBy = actorName;
        ReviewComments = comments;

        // If nextApproverType != null, could notify next approver externally
    }
    
    private RequestStatus GetExpectedStatusForApprover(ApproverType approverType) => approverType switch
    {
        ApproverType.Manager => RequestStatus.PendingManager,
        ApproverType.HR => RequestStatus.PendingHr,
        ApproverType.Admin => RequestStatus.PendingHr, // Extend if needed
        _ => throw new ArgumentException("Invalid approver type.")
    };
    
    
    public void UpdateDates(DateTime newStartDate, DateTime newEndDate)
    {
        if (newStartDate.Date > newEndDate.Date)
            throw new DomainException("Start date cannot be after end date.");

        if (Status == RequestStatus.Approved)
            throw new DomainException("Cannot update dates of an approved leave request.");

        StartDate = newStartDate.Date;
        EndDate = newEndDate.Date;
    }
    public void AddReview(string reviewer, string? comments = null)
    {
        ReviewedBy = reviewer ?? throw new ArgumentNullException(nameof(reviewer));
        ReviewedAt = DateTime.UtcNow;
        ReviewComments = comments;
    }
    
    private void AddApprovalHistory(
        ActionType action,
        ApproverType approverType,
        Guid approverId,
        string approverName,
        string? comments)
    {
        var history = new ApprovalHistory(
            Guid.NewGuid(),
            this.Id,
            approverType,
            approverId,
            approverName,
            action,
            Status,
            Status,
            DateTime.Now,
            comments
        );

        _approvalHistories.Add(history);
    }
}