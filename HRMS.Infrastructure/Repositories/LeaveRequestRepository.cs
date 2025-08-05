using HRMS.Application.Common.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using HRMS.Domain.Interfaces.Repositories;
using HRMS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Infrastructure.Repositories;

public class LeaveRequestRepository(ApplicationDbContext context, IUnitOfWork unitOfWork) : GenericRepository<LeaveRequest>(context),ILeaveRequestRepository
{
    
    public async Task<List<LeaveRequest>> GetByEmployeeIdAsync(
        Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        return await context.LeaveRequests
            .Include(e => e.Employee)
            .Where(lr => lr.EmployeeId == employeeId)
            .OrderByDescending(lr => lr.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<LeaveRequest>> GetPendingByDepartmentAsync(
        Guid departmentId,
        CancellationToken cancellationToken = default)
    {
        return await context.LeaveRequests
            .Include(lr => lr.Employee)
            .Where(lr => lr.Status == LeaveStatus.Pending && 
                         lr.Employee.DepartmentId == departmentId)
            .OrderBy(lr => lr.StartDate)
            .ToListAsync(cancellationToken);
    }
    public async Task<List<LeaveRequest>> GetByDepartmentIdAsync(Guid departmentId, CancellationToken cancellationToken = default)
    {
        return await context.LeaveRequests
            .Include(lr => lr.Employee)
            .Where(lr => lr.Employee.DepartmentId == departmentId)
            .OrderByDescending(lr => lr.StartDate)
            .ToListAsync(cancellationToken);
    }
    public async Task<List<LeaveRequest>> GetApprovedInPeriodAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        return await context.LeaveRequests
            .Include(lr => lr.Employee)
            .Where(lr => lr.Status == LeaveStatus.Approved &&
                         lr.StartDate <= endDate &&
                         lr.EndDate >= startDate)
            .OrderByDescending(lr => lr.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<LeaveRequest>> CheckOverlappingLeaveRequests(DateTime startDate, DateTime endDate, Guid employeeId,
        CancellationToken cancellationToken = default)
    {
        var overlappingRequests = await context.LeaveRequests
            .Where(lr => lr.EmployeeId == employeeId)
            .Where(lr => lr.Status == LeaveStatus.Approved)
            .Where(lr => (startDate <= lr.EndDate && endDate >= lr.StartDate))
            .ToListAsync();
        return overlappingRequests;
    }

    
}

