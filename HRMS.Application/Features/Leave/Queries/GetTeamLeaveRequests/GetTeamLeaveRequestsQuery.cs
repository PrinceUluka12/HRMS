using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Leave.Queries.GetTeamLeaveRequests;

public record GetTeamLeaveRequestsQuery(Guid ManagerId) : IRequest<BaseResult<IEnumerable<TeamLeaveRequestsDto>>>;

public class GetTeamLeaveRequestsQueryHandler(
    IEmployeeRepository employeeRepository,
    ILeaveRequestRepository leaveRequestRepository,
    ILogger<GetTeamLeaveRequestsQueryHandler> logger)
    : IRequestHandler<GetTeamLeaveRequestsQuery, BaseResult<IEnumerable<TeamLeaveRequestsDto>>>
{
    public async Task<BaseResult<IEnumerable<TeamLeaveRequestsDto>>> Handle(
        GetTeamLeaveRequestsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var manager = await employeeRepository.GetByIdWithIncludesAsync(
                request.ManagerId,
                e => e.Department,
                e => e.Position);

            if (manager == null || manager.Department == null)
            {
                return BaseResult<IEnumerable<TeamLeaveRequestsDto>>.Ok(new List<TeamLeaveRequestsDto>());
            }

            var leaveRequests = await leaveRequestRepository.GetByDepartmentIdAsync(manager.DepartmentId, cancellationToken);

            var result = new List<TeamLeaveRequestsDto>();

            foreach (var leaveRequest in leaveRequests)
            {
                var employee = await employeeRepository.GetByIdAsync(leaveRequest.EmployeeId, cancellationToken);
                var employeeName = employee != null
                    ? $"{employee.Name.FirstName} {employee.Name.LastName}"
                    : "Unknown";

                result.Add(new TeamLeaveRequestsDto
                {
                    Id = leaveRequest.Id,
                    EmployeeId = leaveRequest.EmployeeId,
                    EmployeeName = employeeName,
                    StartDate = leaveRequest.StartDate,
                    EndDate = leaveRequest.EndDate,
                    LeaveType = leaveRequest.Type.ToString(),
                    Reason = leaveRequest.Reason
                });
            }

            return BaseResult<IEnumerable<TeamLeaveRequestsDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving team leave requests for ManagerId: {ManagerId}", request.ManagerId);

            return BaseResult<IEnumerable<TeamLeaveRequestsDto>>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while retrieving team leave requests."
            ));
        }
    }
}
