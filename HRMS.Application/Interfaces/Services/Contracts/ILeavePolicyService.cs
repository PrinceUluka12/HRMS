using HRMS.Application.Features.Leave.Dtos;
using HRMS.Domain.Enums;

namespace HRMS.Application.Interfaces.Services.Contracts;

public interface ILeavePolicyService
{
    Task ValidateLeaveRequest(Guid employeeId, DateTime startDate, DateTime endDate, LeaveType type);
    Task<LeaveBalanceDto> GetLeaveBalanceAsync(Guid employeeId);
    Task ApproveLeaveRequestAsync(Guid leaveRequestId, string approvedBy);
    Task RejectLeaveRequestAsync(Guid leaveRequestId, string rejectedBy, string reason);
}