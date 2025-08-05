using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Interfaces;

namespace HRMS.Application.Interfaces.Services;

public class OnboardingService(IDefaultStageFactory defaultStageFactory, IOnboardingRepository onboardingRepository, IUnitOfWork unitOfWork)
    : IOnboardingService
{
    public async Task BeginOnboardingAsync(Guid employeeId, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var defaultStages = defaultStageFactory.Create();
            var onboarding = new Onboarding(employeeId, DateTime.UtcNow);

            foreach (var stage in defaultStages)
            {
                onboarding.AddStage(stage);
            }

            await onboardingRepository.AddAsync(onboarding);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
       
    }

    public async Task<Guid> ManualStartOnboarding(Guid employeeId, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var defaultStages = defaultStageFactory.Create();
            var onboarding = new Onboarding(employeeId, DateTime.UtcNow);

            foreach (var stage in defaultStages)
            {
                onboarding.AddStage(stage);
            }

            await onboardingRepository.AddAsync(onboarding);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return onboarding.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}