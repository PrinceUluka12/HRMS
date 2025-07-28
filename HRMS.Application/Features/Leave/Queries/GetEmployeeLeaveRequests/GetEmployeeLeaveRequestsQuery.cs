using AutoMapper;
using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Application.Features.Leave.Queries.GetEmployeeLeaveRequests;

public record GetEmployeeLeaveRequestsQuery(
    Guid EmployeeId) : IRequest<IEnumerable<LeaveRequestDto>>;

public class GetEmployeeLeaveRequestsQueryHandler(
    ILeaveRequestRepository leaveRequestRepository,
    IMapper mapper,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetEmployeeLeaveRequestsQuery, IEnumerable<LeaveRequestDto>>
{
    public async Task<IEnumerable<LeaveRequestDto>> Handle(
        GetEmployeeLeaveRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var leaveRequests = await leaveRequestRepository.GetByEmployeeIdAsync(request.EmployeeId);

        if (leaveRequests.Count != 0)
        {
            var data = leaveRequests.Select(lea => new LeaveRequestDto(
                lea.Id,
                lea.EmployeeId,
                "",
                lea.StartDate,
                lea.EndDate,
                lea.Type,
                lea.Reason,
                lea.Status,
                lea.RequestDate,
                lea.ApprovedBy,
                lea.ApprovedDate,
                lea.RejectionReason
            ));
            return data;
        }

        return new  List<LeaveRequestDto>();
    }
}