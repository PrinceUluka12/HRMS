using HRMS.Domain.Aggregates.OnboardingAggregate;

namespace HRMS.Domain.Interfaces;

public interface IDefaultStageFactory
{
    List<OnboardingStage> Create();
}