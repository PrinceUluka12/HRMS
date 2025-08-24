using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.RecruitmentAggregates;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Recruitment.Commands;

public record CreateJobVacancyCommand(
    string Title,
    string Description,
    string Location,
    string EmploymentType,
    DateTime ClosingOn) : IRequest<BaseResult<Guid>>;

public class CreateJobVacancyCommandHandler(IJobVacancyRepository jobVacancyRepository, IUnitOfWork unitOfWork) : IRequestHandler<CreateJobVacancyCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(CreateJobVacancyCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            EmploymentType employmentType = GetEmploymentTypeFromString(request.EmploymentType);

            var jobVacancy = new JobVacancy(request.Title, request.Description, request.Location, employmentType,
                request.ClosingOn);

            await jobVacancyRepository.AddAsync(jobVacancy);

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return BaseResult<Guid>.Ok(jobVacancy.Id);
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BaseResult<Guid>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while creating the Job Vacancy."
            ));
        }
    }

    private EmploymentType GetEmploymentTypeFromString(string type)
    {
        return type.Trim().ToLower() switch
        {
            "permanent" => EmploymentType.Permanent,
            "contract" => EmploymentType.Contract,
            "temporary" => EmploymentType.Temporary,
            "seasonal" => EmploymentType.Seasonal,
            "intern" => EmploymentType.Intern,
            _ => throw new ArgumentException($"Invalid employment type: {type}")
        };
    }
}
    
    