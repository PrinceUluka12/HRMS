using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Recruitment.Commands;

public record PublishJobVacancyCommand(Guid JobVacancyId) : IRequest<BaseResult<Guid>>;

public class PublishJobVacancyCommandHandler(IUnitOfWork unitOfWork, IJobVacancyRepository jobVacancyRepository)
    : IRequestHandler<PublishJobVacancyCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(PublishJobVacancyCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var jobVacancy = await jobVacancyRepository.GetByIdAsync(request.JobVacancyId);
            if (jobVacancy == null)
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"Job Vacancy with ID '{request.JobVacancyId}' was not found.",
                    nameof(request.JobVacancyId)
                ));
            }
            else
            {
                jobVacancy.Publish();
                await jobVacancyRepository.Update(jobVacancy);
                await unitOfWork.CommitTransactionAsync(cancellationToken);
                return BaseResult<Guid>.Ok(jobVacancy.Id);
            }
        }
        catch (Exception e)
        {
            return BaseResult<Guid>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while Publishing job Vacancy."
            ));
        }
    }
}