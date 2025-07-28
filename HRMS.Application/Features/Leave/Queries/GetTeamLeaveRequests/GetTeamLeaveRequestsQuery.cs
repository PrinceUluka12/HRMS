using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Interfaces.Repositories;
using MediatR;

namespace HRMS.Application.Features.Leave.Queries.GetTeamLeaveRequests;

public record GetTeamLeaveRequestsQuery(Guid ManagerId) : IRequest<IEnumerable<TeamLeaveRequestsDto>>;

public class GetTeamLeaveRequestsQueryHandler(
    IEmployeeRepository employeeRepository,
    ILeaveRequestRepository leaveRequestRepository)
    : IRequestHandler<GetTeamLeaveRequestsQuery, IEnumerable<TeamLeaveRequestsDto>>
{
    public async Task<IEnumerable<TeamLeaveRequestsDto>> Handle(GetTeamLeaveRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new List<TeamLeaveRequestsDto>();
        try
        {
            var employee =
                await employeeRepository.GetByIdAsyncIncludeRelationship(request.ManagerId, e => e.Department,
                    e => e.Position);

            var leaveRequests =
                await leaveRequestRepository.GetPendingByDepartmentAsync(employee.DepartmentId, cancellationToken);

            foreach (var leaveRequest in leaveRequests)
            {
                var data = new TeamLeaveRequestsDto();
                data.Id = leaveRequest.Id;
                data.EmployeeId = leaveRequest.EmployeeId;
                data.EmployeeName = await GetEmployeeName(leaveRequest.EmployeeId, cancellationToken);
                data.StartDate = leaveRequest.StartDate;
                data.EndDate = leaveRequest.EndDate;
                data.LeaveType = leaveRequest.Type.ToString();
                data.Reason = leaveRequest.Reason;
                result.Add(data);
            }
            return result.AsEnumerable();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private async Task<string> GetEmployeeName(Guid employeeId, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(employeeId, cancellationToken);
        return $"{employee.Name.FirstName} {employee.Name.LastName}";
    }
}