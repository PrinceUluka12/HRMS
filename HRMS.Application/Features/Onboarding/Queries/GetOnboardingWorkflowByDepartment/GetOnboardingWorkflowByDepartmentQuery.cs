using AutoMapper;
using HRMS.Application.Features.Onboarding.Dtos;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;

namespace HRMS.Application.Features.Onboarding.Queries.GetOnboardingWorkflowByDepartment;

public record GetOnboardingWorkflowByDepartmentQuery(Guid DepartmentId)
    : IRequest<BaseResult<IEnumerable<OnboardingWorkflowDto>>>;

public class GetOnboardingWorkflowByDepartmentQueryHandler(
    IOnboardingWorkflowRepository onboardingWorkflowRepository,
    IMapper mapper,
    ITranslator translator) : IRequestHandler<GetOnboardingWorkflowByDepartmentQuery,
    BaseResult<IEnumerable<OnboardingWorkflowDto>>>
{
    public async Task<BaseResult<IEnumerable<OnboardingWorkflowDto>>> Handle(
        GetOnboardingWorkflowByDepartmentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var workFlow = await onboardingWorkflowRepository.GetByDepartment(request.DepartmentId);
            var dto = mapper.Map<IEnumerable<OnboardingWorkflowDto>>(workFlow);
            
            return BaseResult<IEnumerable<OnboardingWorkflowDto>>.Ok(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}