using AutoMapper;
using HRMS.Application.Features.Recruitment.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Recruitment.Commands;

public record ChangeApplicationStatusCommand(Guid Id, string newStatus) : IRequest<BaseResult<ApplicationDto>>;

public class ChangeApplicationStatusCommandHandler(
    IApplicationRepository applicationRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ChangeApplicationStatusCommand, BaseResult<ApplicationDto>>
{
    public async Task<BaseResult<ApplicationDto>> Handle(ChangeApplicationStatusCommand request,
        CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            if (!Enum.TryParse<ApplicationStatus>(request.newStatus, true, out var applicationStatus))
            {
                return BaseResult<ApplicationDto>.Failure(new Error(
                    ErrorCode.FieldDataInvalid,
                    $"Invalid Application Status value '{request.newStatus}'.",
                    nameof(request.newStatus)
                ));
            }

            var application = await applicationRepository.GetByIdAsync(request.Id);
            if (application == null)
            {
            }

            application.ChangeStatus(applicationStatus);
            
            await applicationRepository.Update(application);
            
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            
            return BaseResult<ApplicationDto>.Ok(mapper.Map<ApplicationDto>(application));
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BaseResult<ApplicationDto>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while changing the application status."
            ));
        }
    }
}