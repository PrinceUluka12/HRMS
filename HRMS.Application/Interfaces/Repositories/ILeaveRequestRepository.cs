using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.LeaveAggregate;

namespace HRMS.Application.Interfaces.Repositories;

public interface ILeaveRequestRepository : IGenericRepository<LeaveRequest>
{
   
    Task<List<LeaveRequest>> GetByEmployeeIdAsync(Guid employeeId, CancellationToken cancellationToken = default);
    Task<List<LeaveRequest>> GetPendingByDepartmentAsync(Guid departmentId, CancellationToken cancellationToken = default);
    Task<List<LeaveRequest>> GetApprovedInPeriodAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<List<LeaveRequest>> CheckOverlappingLeaveRequests(DateTime startDate, DateTime endDate,Guid employeeId, CancellationToken cancellationToken = default);
}