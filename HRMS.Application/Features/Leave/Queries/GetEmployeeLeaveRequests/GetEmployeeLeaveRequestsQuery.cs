using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Leave.Queries.GetEmployeeLeaveRequests;

public record GetEmployeeLeaveRequestsQuery(Guid EmployeeId) 
    : IRequest<BaseResult<IEnumerable<LeaveRequestDto>>>;

public class GetEmployeeLeaveRequestsQueryHandler(
    ILeaveRequestRepository leaveRequestRepository,
    IMapper mapper,
    ILogger<GetEmployeeLeaveRequestsQueryHandler> logger)
    : IRequestHandler<GetEmployeeLeaveRequestsQuery, BaseResult<IEnumerable<LeaveRequestDto>>>
{
    public async Task<BaseResult<IEnumerable<LeaveRequestDto>>> Handle(
        GetEmployeeLeaveRequestsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var leaveRequests = await leaveRequestRepository.GetByEmployeeIdAsync(request.EmployeeId);

            if (leaveRequests == null || !leaveRequests.Any())
            {
                return BaseResult<IEnumerable<LeaveRequestDto>>.Ok(new List<LeaveRequestDto>());
            }

            var result = leaveRequests.Select(lea => new LeaveRequestDto
            {
                Id = lea.Id,
                EmployeeId = lea.EmployeeId,
                EmployeeName = $"{lea.Employee?.Name.FirstName} {lea.Employee?.Name.LastName}",
                StartDate = lea.StartDate,
                EndDate = lea.EndDate,
                Type = lea.Type,
                Reason = lea.Reason,
                Status = lea.Status,
                RequestDate = lea.RequestDate,
                ApprovedBy = lea.ApprovedBy,
                ApprovedDate = lea.ApprovedDate,
                RejectionReason = lea.RejectionReason
            }).ToList();

            return BaseResult<IEnumerable<LeaveRequestDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving leave requests for employee {EmployeeId}", request.EmployeeId);

            return BaseResult<IEnumerable<LeaveRequestDto>>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while retrieving leave requests."
            ));
        }
    }
}
