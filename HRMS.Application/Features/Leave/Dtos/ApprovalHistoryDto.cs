using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Leave.Dtos;

public record ApprovalHistoryDto(Guid Id,
    Guid RequestId,
    ApproverType ApproverType,
    Guid ApproverId,
    string ApproverName,
    ActionType Action,
    RequestStatus PreviousStatus,
    RequestStatus NewStatus,
    string? Comments,
    DateTime Timestamp
    );