using AutoMapper;
using HRMS.Application.Features.Onboarding.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Onboarding.Queries.GetOnboardingByEmployeeId;

public sealed record GetOnboardingByEmployeeIdQuery(Guid EmployeeId) : IRequest<BaseResult<OnboardingDto>>;

public class GetOnboardingByEmployeeIdQueryHandler(
    IOnboardingRepository onboardingRepository,
    IPositionRepository positionRepository,
    IDepartmentRepository departmentRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    IEmployeeRepository employeeRepository,
    ILogger<GetOnboardingByEmployeeIdQueryHandler> logger)
    : IRequestHandler<GetOnboardingByEmployeeIdQuery, BaseResult<OnboardingDto>>
{
    public async Task<BaseResult<OnboardingDto>> Handle(GetOnboardingByEmployeeIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var positions = await positionRepository.GetAllAsync();
            var departments = await departmentRepository.GetWithManagerViewAsync(cancellationToken);
            var employees = await employeeRepository.GetAllAsync();
            var data = await onboardingRepository.GetByEmployeeIdAsync(request.EmployeeId);
            if (data == null)
            {
                return BaseResult<OnboardingDto>.Ok(new OnboardingDto());
            }

            var onboardingData = new OnboardingDto();
            var onboardingStageDtos = new List<OnboardingStageDto>();
            var onboardingTasksDtos = new List<OnboardingTaskDto>();
            var employee = employees.Where(e => e.Id == data.EmployeeId).FirstOrDefault();
            onboardingData.EmployeeId = data.EmployeeId;
            onboardingData.EmployeeName = $"{employee.Name.FirstName}  {employee.Name.LastName}";
            onboardingData.Email = employee.Email;
            onboardingData.Position = positions.FirstOrDefault(e => e.Id == employee.PositionId).Title ?? "";
            onboardingData.Department = departments.FirstOrDefault(e => e.DepartmentId == employee.DepartmentId)
                .DepartmentName ?? "";
            onboardingData.Status = data.Status.ToString();
            onboardingData.StartDate = data.StartDate;
            onboardingData.OverallProgress = data.OverallProgress;
            onboardingData.DaysRemaining = data.DaysRemaining;
            onboardingData.ManagerName = departments.FirstOrDefault(e => e.DepartmentId == employee.DepartmentId)
                .ManagerFullName ?? "";

            var stageDtos = new List<OnboardingStageDto>();

            foreach (var stage in data.Stages)
            {
                var stageDto = new OnboardingStageDto
                {
                    StageName = stage.StageName,
                    Status = stage.Status.ToString(), // Convert enum to string
                    Progress = stage.Progress,
                    DueDate = stage.DueDate,
                    Tasks = stage.Tasks.Select(task => new OnboardingTaskDto
                    {
                        // Map task properties here (example only, adjust based on OnboardingTaskDto)
                        Id = task.Id,
                        TaskName = task.TaskName,
                        Status = task.Status.ToString(), // If status is an enum
                        DueDate = task.DueDate,
                        // Add other properties as needed
                    }).ToList().AsReadOnly() // Convert to IReadOnlyCollection
                };

                stageDtos.Add(stageDto);
            }
            onboardingData.Stages = stageDtos;
            
            return BaseResult<OnboardingDto>.Ok(onboardingData);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving Onboarding data", request);

            return BaseResult<OnboardingDto>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while retrieving Onboarding data."
            ));
        }
    }
}