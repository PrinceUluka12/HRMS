using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Interfaces.Services;

public class LeavePolicyService(
    IEmployeeRepository employeeRepository,
    ILeaveRequestRepository leaveRequestRepository,
    ILogger<LeavePolicyService> logger)
    : ILeavePolicyService
{
    private readonly ILogger<LeavePolicyService> _logger = logger;

    public async Task ValidateLeaveRequest(
        Guid employeeId,
        DateTime startDate,
        DateTime endDate,
        LeaveType type)
    {
        // Check basic validity
        if (endDate < startDate)
        {
            throw new DomainException("End date cannot be before start date");
        }

        // Get employee's leave balance
        var leaveBalance = await GetLeaveBalanceAsync(employeeId, type);

        // Calculate requested days
        var requestedDays = (endDate - startDate).Days + 1;

        if (requestedDays > leaveBalance.AvailableDays)
        {
            throw new DomainException(
                $"Not enough {type} leave days available. " +
                $"Requested: {requestedDays}, Available: {leaveBalance.AvailableDays}");
        }

        // Check for overlapping leave requests
        var overlappingRequests = await leaveRequestRepository.CheckOverlappingLeaveRequests(startDate, endDate, employeeId);

        if (overlappingRequests.Any())
        {
            throw new DomainException("The requested leave period overlaps with an approved leave request");
        }
    }

    public async Task<LeaveBalanceDto> GetLeaveBalanceAsync(Guid employeeId)
    {
        throw new NotImplementedException();
    }

    public async Task ApproveLeaveRequestAsync(Guid leaveRequestId, string approvedBy)
    {
        throw new NotImplementedException();
    }

    public async Task RejectLeaveRequestAsync(Guid leaveRequestId, string rejectedBy, string reason)
    {
        throw new NotImplementedException();
    }

    private async Task<LeaveBalance> GetLeaveBalanceAsync(Guid employeeId, LeaveType type)
    {
        // In a real system, this would calculate based on:
        // - Employee's accrual rate
        // - Already taken leave
        // - Company policy
        // This is a simplified version

        var employee = await employeeRepository.GetByIdWithIncludesAsync(employeeId, lr=> lr.LeaveRequests);
        
        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), employeeId);
        }

        var takenDays = employee.LeaveRequests
            .Where(lr => lr.Type == type && lr.Status == LeaveStatus.Approved)
            .Sum(lr => (lr.EndDate - lr.StartDate).Days + 1);

        // Default policy: 20 vacation days per year
        var availableDays = 20 - takenDays;

        return new LeaveBalance(type, availableDays, takenDays);
    }

    
}

public record LeaveBalance(LeaveType Type, int AvailableDays, int TakenDays);