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
    Guid EmployeeId) : IRequest<List<LeaveRequestDto>>;

public class GetEmployeeLeaveRequestsQueryHandler(
    ILeaveRequestRepository leaveRequestRepository,
    IMapper mapper,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetEmployeeLeaveRequestsQuery, List<LeaveRequestDto>>
{
    public async Task<List<LeaveRequestDto>> Handle(
        GetEmployeeLeaveRequestsQuery request,
        CancellationToken cancellationToken)
    {
        // Authorization check
        if (!currentUserService.IsInRole("HR.Admin") && 
            !currentUserService.IsInRole("Department.Manager") &&
            currentUserService.UserId != request.EmployeeId.ToString())
        {
            throw new ForbiddenAccessException();
        }

        var leaveRequests = await leaveRequestRepository.GetByEmployeeIdAsync(request.EmployeeId);

        return mapper.Map<List<LeaveRequestDto>>(leaveRequests);
    }
}