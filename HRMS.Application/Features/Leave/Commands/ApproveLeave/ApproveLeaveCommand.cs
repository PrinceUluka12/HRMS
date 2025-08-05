using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;

namespace HRMS.Application.Features.Leave.Commands.ApproveLeave;

public record ApproveLeaveCommand(
    Guid LeaveRequestId,
    Guid ManagerId) : IRequest<BaseResult<Guid>>;

public class ApproveLeaveCommandHandler(
    IEmployeeRepository employeeRepository,
    ILeaveRequestRepository leaveRequestRepository,
    IUnitOfWork unitOfWork,
    ITranslator translator,
    ILogger<ApproveLeaveCommandHandler> logger)
    : IRequestHandler<ApproveLeaveCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(ApproveLeaveCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var leaveRequest = await leaveRequestRepository.GetByIdWithIncludesAsync(
                request.LeaveRequestId,
                e => e.Employee);

            if (leaveRequest is null)
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.NotFound,
                    translator.GetString($"Leave request with ID {request.LeaveRequestId} was not found."),
                    nameof(request.LeaveRequestId)
                ));
            }

            var manager = await employeeRepository.GetByIdWithIncludesAsync(
                request.ManagerId,
                e => e.Department);

            if (manager is null)
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.NotFound,
                    translator.GetString($"Manager with ID {request.ManagerId} was not found."),
                    nameof(request.ManagerId)
                ));
            }

            // Approve the leave request
            leaveRequest.Approve($"{manager.Name.FirstName} {manager.Name.LastName}");
            leaveRequestRepository.Update(leaveRequest);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return BaseResult<Guid>.Ok(leaveRequest.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Error while approving leave request ID: {LeaveRequestId}", request.LeaveRequestId);

            return BaseResult<Guid>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString(TranslatorMessages.GeneralMessages.Unexpected_Error(ex.Message))
            ));
        }
    }
}
