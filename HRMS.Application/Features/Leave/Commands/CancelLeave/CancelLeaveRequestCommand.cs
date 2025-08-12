using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Leave.Commands.CancelLeave;

public record CancelLeaveRequestCommand(Guid RequestId):IRequest<BaseResult<Guid>>;

public class CancelLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
    IUnitOfWork unitOfWork,
    ILogger<CancelLeaveRequestCommandHandler> logger,
    ITranslator translator) : IRequestHandler<CancelLeaveRequestCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(CancelLeaveRequestCommand request, CancellationToken cancellationToken)
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
            
            // Cancel the leave request
            leaveRequest.Cancel(null);
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