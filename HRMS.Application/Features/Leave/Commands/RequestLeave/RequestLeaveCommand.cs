using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Leave.Commands.RequestLeave;

public record RequestLeaveCommand(
    Guid EmployeeId,
    DateTime StartDate,
    DateTime EndDate,
    LeaveType Type,
    string Reason) : IRequest<BaseResult<LeaveRequestDto>>;

public class RequestLeaveCommandHandler(
    IEmployeeRepository employeeRepository,
    ILeavePolicyService leavePolicyService,
    ILeaveRequestRepository leaveRequestRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ITranslator translator,
    ILogger<RequestLeaveCommandHandler> logger)
    : IRequestHandler<RequestLeaveCommand, BaseResult<LeaveRequestDto>>
{
    public async Task<BaseResult<LeaveRequestDto>> Handle(RequestLeaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            var employee = await employeeRepository.GetByIdAsync(request.EmployeeId, cancellationToken);
            if (employee is null)
            {
                return BaseResult<LeaveRequestDto>.Failure(new Error(
                    ErrorCode.NotFound,
                    translator.GetString($"Employee with ID {request.EmployeeId} not found."),
                    nameof(request.EmployeeId)
                ));
            }

            // Validate against policy (wrapped in try-catch for better control)
            try
            {
                await leavePolicyService.ValidateLeaveRequest(
                    request.EmployeeId,
                    request.StartDate,
                    request.EndDate,
                    request.Type);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Leave policy validation failed for employee {EmployeeId}", request.EmployeeId);
                return BaseResult<LeaveRequestDto>.Failure(new Error(
                    ErrorCode.FieldDataInvalid,
                    translator.GetString(ex.Message)
                ));
            }

            var leaveRequest = new LeaveRequest(
                request.EmployeeId,
                request.StartDate,
                request.EndDate,
                request.Type,
                request.Reason);

            await leaveRequestRepository.AddAsync(leaveRequest);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            var dto = mapper.Map<LeaveRequestDto>(leaveRequest);
            return BaseResult<LeaveRequestDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Unexpected error while requesting leave for employee {EmployeeId}", request.EmployeeId);

            return BaseResult<LeaveRequestDto>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString(TranslatorMessages.GeneralMessages.Unexpected_Error(ex.Message))
            ));
        }
    }
}
