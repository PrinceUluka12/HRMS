using AutoMapper;
using HRMS.Application.Features.Recruitment.Dtos;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.RecruitmentAggregates;
using MediatR;

namespace HRMS.Application.Features.Recruitment.Queries;

public record GetJobVacancyDetailsQuery(Guid JobVacancyId) : IRequest<BaseResult<JobVacancyDto>>;

public class GetJobVacancyDetailsQueryHandler(IJobVacancyRepository jobVacancyRepository, IMapper mapper)
    : IRequestHandler<GetJobVacancyDetailsQuery, BaseResult<JobVacancyDto>>
{
    public async Task<BaseResult<JobVacancyDto>> Handle(GetJobVacancyDetailsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var data = await jobVacancyRepository.GetAllWithApplicationsAsync(request.JobVacancyId);
            var  result = mapper.Map<JobVacancyDto>(data);
            
            return BaseResult<JobVacancyDto>.Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BaseResult<JobVacancyDto>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while retrieving Job Vacancies."
            ));
        }
    }
}