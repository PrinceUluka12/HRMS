using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Enums;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class TimeOffBalanceRepository(ApplicationDbContext context) :GenericRepository<TimeOffBalance>(context),  ITimeOffBalanceRepository
{
    public async Task<TimeOffBalance> GetByEmployeeIdAndLeaveTypeAsync(Guid id, LeaveType leaveType, int year)
    {

        return await context.TimeOffBalances
            .Where(x => x.EmployeeId == id && x.LeaveType == leaveType).FirstOrDefaultAsync();
    }
}