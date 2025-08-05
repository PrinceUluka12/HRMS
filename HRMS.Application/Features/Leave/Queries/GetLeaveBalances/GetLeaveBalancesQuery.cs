using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Leave.Queries.GetLeaveBalances;

public record GetLeaveBalancesQuery(Guid EmployeeId) : IRequest<BaseResult<EmployeeLeaveSummaryDto>>;

public class GetLeaveBalancesQueryHandler(
    IEmployeeRepository employeeRepository,
    ILeavePolicyService leavePolicyService,
    ITranslator translator,
    ILogger<GetLeaveBalancesQueryHandler> logger)
    : IRequestHandler<GetLeaveBalancesQuery, BaseResult<EmployeeLeaveSummaryDto>>
{
    public async Task<BaseResult<EmployeeLeaveSummaryDto>> Handle(
        GetLeaveBalancesQuery request,
        CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee is null)
        {
            return BaseResult<EmployeeLeaveSummaryDto>.Ok(new EmployeeLeaveSummaryDto());
        }

        var balances = new List<LeaveBalanceDto>();

        foreach (LeaveType leaveType in Enum.GetValues(typeof(LeaveType)))
        {
            try
            {
                var balance = await leavePolicyService.GetLeaveBalanceAsync(request.EmployeeId);
                if (balance != null)
                {
                    balances.Add(balance);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Could not retrieve balance for LeaveType {LeaveType} for employee {EmployeeId}", leaveType, request.EmployeeId);
                // Continue gracefully to collect other leave types
            }
        }

        var summary = new EmployeeLeaveSummaryDto
        {
            EmployeeId = employee.Id,
            EmployeeName = $"{employee.Name.FirstName} {employee.Name.LastName}",
            AsOfDate = DateTime.UtcNow,
            Balances = balances
        };

        return BaseResult<EmployeeLeaveSummaryDto>.Ok(summary);
    }
}
