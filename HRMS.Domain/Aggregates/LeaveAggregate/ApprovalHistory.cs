using HRMS.Domain.Enums;

namespace HRMS.Domain.Aggregates.LeaveAggregate;

public class ApprovalHistory
{
    public Guid Id { get; private set; }
    public Guid RequestId { get; private set; }
    public ApproverType ApproverType { get; private set; }
    public Guid ApproverId { get; private set; }
    public string ApproverName { get; private set; }
    public ActionType Action { get; private set; }
    public RequestStatus PreviousStatus { get; private set; }
    public RequestStatus NewStatus { get; private set; }
    public string? Comments { get; private set; }
    public DateTime Timestamp { get; private set; }

    private ApprovalHistory() { }
    public ApprovalHistory(
        Guid id,
        Guid requestId,
        ApproverType approverType,
        Guid approverId,
        string approverName,
        ActionType action,
        RequestStatus previousStatus,
        RequestStatus newStatus,
        DateTime timestamp,
        string? comments = null
        )
    {
        Id = id;
        RequestId = requestId;
        ApproverType = approverType;
        ApproverId = approverId;
        ApproverName = approverName ?? throw new ArgumentNullException(nameof(approverName));
        Action = action;
        PreviousStatus = previousStatus;
        NewStatus = newStatus;
        Timestamp = timestamp;
        Comments = comments;
    }
    
    public void UpdateComments(string newComments)
    {
        if (string.IsNullOrWhiteSpace(newComments))
            throw new ArgumentException("Comments cannot be empty.", nameof(newComments));

        Comments = newComments;
        // Usually you'd update a timestamp here, but since it's a history record, 
        // it might be better to keep it immutable. If mutable, uncomment below:
        // Timestamp = DateTime.UtcNow;
    }
}