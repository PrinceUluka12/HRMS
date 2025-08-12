using HRMS.Application.Common.Interfaces;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Enums;

namespace HRMS.Application.Interfaces.Repositories;

public interface ITimeOffBalanceRepository :IGenericRepository<TimeOffBalance>
{
    public Task<TimeOffBalance> GetByEmployeeIdAndLeaveTypeAsync(Guid id, LeaveType  leaveType, int year);
}