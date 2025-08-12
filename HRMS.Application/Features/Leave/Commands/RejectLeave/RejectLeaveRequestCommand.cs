using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Leave.Commands.RejectLeave;

public record RejectLeaveRequestCommand(Guid RequestId, string ApproverId, string Reason): IRequest<BaseResult<Guid>>;

public class RejectLeaveRequestHandler(ILeaveRequestRepository leaveRequestRepository,
    IUnitOfWork unitOfWork,
    ITranslator translator,
    IEmployeeRepository employeeRepository,
    ILogger<RejectLeaveRequestHandler> logger) : IRequestHandler<RejectLeaveRequestCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(RejectLeaveRequestCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var leaveRequest = await leaveRequestRepository.GetByIdWithIncludesAsync(
                request.RequestId,
                e => e.Employee);

            if (leaveRequest is null)
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.NotFound,
                    translator.GetString($"Leave request with ID {request.RequestId} was not found."),
                    nameof(request.RequestId)
                ));
            }
            
            var manager = await employeeRepository.GetByIdWithIncludesAsync(
                request.ApproverId,
                e => e.Department);

            if (manager is null)
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.NotFound,
                    translator.GetString($"Manager with ID {request.ApproverId} was not found."),
                    nameof(request.ApproverId)
                ));
            }
            
            // Reject the leave request
            leaveRequest.Reject(ApproverType.Manager, manager.Id,$"{manager.Name.FirstName} {manager.Name.LastName}", request.Reason);
            leaveRequestRepository.Update(leaveRequest);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return BaseResult<Guid>.Ok(leaveRequest.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Error while approving leave request ID: {LeaveRequestId}", request.RequestId);

            return BaseResult<Guid>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString(TranslatorMessages.GeneralMessages.Unexpected_Error(ex.Message))
            ));
        }
    }
}