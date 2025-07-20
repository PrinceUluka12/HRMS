using AutoMapper;
using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Leave.Commands.RequestLeave;

public record RequestLeaveCommand(
    Guid EmployeeId,
    DateTime StartDate,
    DateTime EndDate,
    LeaveType Type,
    string Reason) : IRequest<LeaveRequestDto>;

public class RequestLeaveCommandHandler(
    IEmployeeRepository employeeRepository,
    ILeavePolicyService leavePolicyService,
    IUnitOfWork unitOfWork,
    IMapper mapper)
    : IRequestHandler<RequestLeaveCommand, LeaveRequestDto>
{
    public async Task<LeaveRequestDto> Handle(RequestLeaveCommand request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), request.EmployeeId);
        }

        // Validate leave request against policy
        await leavePolicyService.ValidateLeaveRequest(
            request.EmployeeId, 
            request.StartDate, 
            request.EndDate, 
            request.Type);

        var leaveRequest = new LeaveRequest(
            request.EmployeeId,
            request.StartDate,
            request.EndDate,
            request.Type,
            request.Reason,
            LeaveStatus.Pending);

        employee.RequestLeave(leaveRequest);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<LeaveRequestDto>(leaveRequest);
    }
}