using HRMS.Application.Features.Onboarding.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Onboarding.Queries.GetAllOnboarding;

public record GetAllOnboardingQuery() : IRequest<BaseResult<IEnumerable<OnboardingDto>>>;

public class GetAllOnboardingQueryHandler(
    IOnboardingRepository onboardingRepository,
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository,
    IPositionRepository positionRepository,
    ILogger<GetAllOnboardingQueryHandler> logger)
    : IRequestHandler<GetAllOnboardingQuery, BaseResult<IEnumerable<OnboardingDto>>>
{
    public async Task<BaseResult<IEnumerable<OnboardingDto>>> Handle(GetAllOnboardingQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = new List<OnboardingDto>();
            var positions = await positionRepository.GetAllAsync();
            var departments = await departmentRepository.GetWithManagerViewAsync(cancellationToken);
            var data = await onboardingRepository.GetAllDataAsync(cancellationToken);
            var employees = await employeeRepository.GetAllAsync();

            if (data == null || data.Count == 0)
            {
                return BaseResult<IEnumerable<OnboardingDto>>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"No Onboarding data Found .",
                    nameof(request)
                ));
            }

            foreach (var onboarding in data)
            {
                var onboardingData = new OnboardingDto();
                var onboardingStageDtos = new List<OnboardingStageDto>();
                var employee = employees.Where(e => e.Id == onboarding.EmployeeId).FirstOrDefault();
                onboardingData.EmployeeId = onboarding.EmployeeId;
                onboardingData.EmployeeName = $"{employee.Name.FirstName}  {employee.Name.LastName}";
                onboardingData.Email = employee.Email;
                onboardingData.Position = positions.FirstOrDefault(e => e.Id == employee.PositionId).Title;
                onboardingData.Department = departments.FirstOrDefault(e => e.DepartmentId == employee.DepartmentId)
                    .DepartmentName;
                onboardingData.Status = onboarding.Status.ToString();
                onboardingData.StartDate = onboarding.StartDate;
                onboardingData.OverallProgress = onboarding.OverallProgress;
                onboardingData.DaysRemaining = onboarding.DaysRemaining;
                onboardingData.ManagerName = departments.FirstOrDefault(e => e.DepartmentId == employee.DepartmentId)
                    .ManagerFullName;
                foreach (var onboardingStatuses in onboarding.Stages)
                {
                    var onboardingStageDto = new OnboardingStageDto();

                    onboardingStageDto.StageName = onboardingStatuses.StageName;
                    onboardingStageDto.Status = onboardingStatuses.Status.ToString();
                    onboardingStageDto.Progress = onboardingStatuses.Progress;
                    onboardingStageDto.DueDate = onboardingStatuses.DueDate;
                    onboardingStageDtos.Add(onboardingStageDto);
                }

                onboardingData.Stages = onboardingStageDtos;

                result.Add(onboardingData);
            }

            return BaseResult<IEnumerable<OnboardingDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving Onboarding data", request);

            return BaseResult<IEnumerable<OnboardingDto>>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while retrieving Onboarding data."
            ));
        }
    }
}