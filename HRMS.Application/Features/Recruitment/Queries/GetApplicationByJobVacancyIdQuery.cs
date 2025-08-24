using AutoMapper;
using HRMS.Application.Features.Recruitment.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;

namespace HRMS.Application.Features.Recruitment.Queries;

public record GetApplicationByJobVacancyIdQuery(Guid JobVacancyId) : IRequest<BaseResult<IEnumerable<ApplicationDto>>>;

public class GetApplicationByJobVacancyIdQueryHandler(IJobVacancyRepository jobVacancyRepository, IMapper mapper)
    : IRequestHandler<GetApplicationByJobVacancyIdQuery, BaseResult<IEnumerable<ApplicationDto>>>
{
    public async Task<BaseResult<IEnumerable<ApplicationDto>>> Handle(GetApplicationByJobVacancyIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var data = await jobVacancyRepository.GetAllApplicationsByVacancyId(request.JobVacancyId);
            var  result = mapper.Map<IEnumerable<ApplicationDto>>(data);
            
            return BaseResult<IEnumerable<ApplicationDto>>.Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BaseResult<IEnumerable<ApplicationDto>>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while retrieving Applications."
            ));
        }
    }
}