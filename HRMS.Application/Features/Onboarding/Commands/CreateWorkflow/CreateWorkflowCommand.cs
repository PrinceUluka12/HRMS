using AutoMapper;
using HRMS.Application.Features.Onboarding.Dtos;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Onboarding.Commands.CreateWorkflow;

public record CreateWorkflowCommand(
    string name,
    Guid departmentId,
    int estimatedDays,
    Guid createdById,
    string? description = null) : IRequest<BaseResult<OnboardingWorkflowDto>>;

public class CreateWorkflowCommandHandler(IOnboardingWorkflowRepository onboardingWorkflowRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ITranslator translator,
    ILogger<CreateWorkflowCommandHandler> logger) : IRequestHandler<CreateWorkflowCommand, BaseResult<OnboardingWorkflowDto>>
{
    public async Task<BaseResult<OnboardingWorkflowDto>> Handle(CreateWorkflowCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var workflow = new OnboardingWorkflow(request.name, request.departmentId, request.estimatedDays, request.createdById, request.description);
            
            await onboardingWorkflowRepository.AddAsync(workflow);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var onboardingWorkflowDto = mapper.Map<OnboardingWorkflowDto>(workflow);
            
            return BaseResult<OnboardingWorkflowDto>.Ok(onboardingWorkflowDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}