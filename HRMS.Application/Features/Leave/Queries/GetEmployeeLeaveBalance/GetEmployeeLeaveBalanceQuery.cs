using AutoMapper;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Leave.Queries.GetEmployeeLeaveBalance;

public record GetEmployeeLeaveBalanceQuery(Guid EmployeeId, LeaveType LeaveType, int PolicyYear)
    : IRequest<BaseResult<TimeOffBalanceDto?>>;

public class GetEmployeeLeaveBalanceQueryHandler(
    ITimeOffBalanceRepository timeOffBalanceRepository,
    ILogger<GetEmployeeLeaveBalanceQueryHandler> logger,
    IMapper  mapper)
    : IRequestHandler<GetEmployeeLeaveBalanceQuery, BaseResult<TimeOffBalanceDto?>>
{
    public async Task<BaseResult<TimeOffBalanceDto?>> Handle(GetEmployeeLeaveBalanceQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var balance = await timeOffBalanceRepository.GetByEmployeeIdAndLeaveTypeAsync(
                request.EmployeeId, request.LeaveType, request.PolicyYear);

            if (balance == null)
                return null;

            return mapper.Map<TimeOffBalanceDto>(balance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}