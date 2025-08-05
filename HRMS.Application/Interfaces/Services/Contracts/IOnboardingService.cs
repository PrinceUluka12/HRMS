namespace HRMS.Application.Interfaces.Services.Contracts;

public interface IOnboardingService
{
    Task BeginOnboardingAsync(Guid employeeId, CancellationToken cancellationToken);
    Task<Guid> ManualStartOnboarding(Guid employeeId, CancellationToken cancellationToken);
}