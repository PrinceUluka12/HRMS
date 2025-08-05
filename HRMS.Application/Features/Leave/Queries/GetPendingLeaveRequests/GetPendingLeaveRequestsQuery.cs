using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Leave.Queries.GetPendingLeaveRequests;

public record GetPendingLeaveRequestsQuery(Guid ManagerId) : IRequest<BaseResult<IEnumerable<PendingLeaveRequestsDto>>>;

public class GetPendingLeaveRequestsQueryHandler(
    IEmployeeRepository employeeRepository,
    ILeaveRequestRepository leaveRequestRepository,
    ILogger<GetPendingLeaveRequestsQueryHandler> logger)
    : IRequestHandler<GetPendingLeaveRequestsQuery, BaseResult<IEnumerable<PendingLeaveRequestsDto>>>
{
    public async Task<BaseResult<IEnumerable<PendingLeaveRequestsDto>>> Handle(GetPendingLeaveRequestsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var manager = await employeeRepository.GetByIdWithIncludesAsync(
                request.ManagerId,
                e => e.Department,
                e => e.Position);

            if (manager == null || manager.Department == null)
            {
                return BaseResult<IEnumerable<PendingLeaveRequestsDto>>.Ok(new List<PendingLeaveRequestsDto>());
            }

            var leaveRequests = await leaveRequestRepository.GetPendingByDepartmentAsync(manager.DepartmentId, cancellationToken);

            var result = new List<PendingLeaveRequestsDto>();

            foreach (var leaveRequest in leaveRequests)
            {
                var employee = await employeeRepository.GetByIdAsync(leaveRequest.EmployeeId, cancellationToken);
                var employeeName = employee != null
                    ? $"{employee.Name.FirstName} {employee.Name.LastName}"
                    : "Unknown";

                result.Add(new PendingLeaveRequestsDto
                {
                    Id = leaveRequest.Id,
                    EmployeeId = leaveRequest.EmployeeId,
                    EmployeeName = employeeName,
                    StartDate = leaveRequest.StartDate,
                    EndDate = leaveRequest.EndDate,
                    LeaveType = leaveRequest.Type.ToString(),
                    Reason = leaveRequest.Reason,
                    Status = leaveRequest.Status.ToString()
                });
            }

            return BaseResult<IEnumerable<PendingLeaveRequestsDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching pending leave requests for manager {ManagerId}", request.ManagerId);

            return BaseResult<IEnumerable<PendingLeaveRequestsDto>>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while retrieving pending leave requests."
            ));
        }
    }
}
