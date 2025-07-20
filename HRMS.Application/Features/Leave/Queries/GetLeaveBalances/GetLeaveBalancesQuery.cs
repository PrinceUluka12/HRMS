using AutoMapper;
using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Enums;
using MediatR;

namespace HRMS.Application.Features.Leave.Queries.GetLeaveBalances;

public record GetLeaveBalancesQuery(Guid EmployeeId) : IRequest<EmployeeLeaveSummaryDto>;

public class GetLeaveBalancesQueryHandler(
    IEmployeeRepository employeeRepository,
    ILeavePolicyService leavePolicyService,
    IMapper mapper)
    : IRequestHandler<GetLeaveBalancesQuery, EmployeeLeaveSummaryDto>
{
    private readonly IMapper _mapper = mapper;

    public async Task<EmployeeLeaveSummaryDto> Handle(
        GetLeaveBalancesQuery request,
        CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), request.EmployeeId);
        }

        var balances = new List<LeaveBalanceDto>();
        
        // Get balances for each leave type
        foreach (LeaveType leaveType in Enum.GetValues(typeof(LeaveType)))
        {
            try
            {
                var balance = await leavePolicyService.GetLeaveBalanceAsync(request.EmployeeId);
                balances.Add(balance);
            }
            catch (Exception ex)
            {
                // Log error but continue with other leave types
                Console.WriteLine($"Error getting balance for {leaveType}: {ex.Message}");
            }
        }

        return new EmployeeLeaveSummaryDto
        {
            EmployeeId = employee.Id,
            EmployeeName = $"{employee.Name.FirstName} {employee.Name.LastName}",
            AsOfDate = DateTime.UtcNow,
            Balances = balances
        };
    }
}