using AutoMapper;
using HRMS.Application.Features.Departments.Dtos;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Employees.Queries.GetOnboardingDetailsByEmployee;

public sealed record GetOnboardingDetailsQuery(Guid Id) : IRequest<BaseResult<EmployeeOnboardingDetailsDto>>;

public class GetOnboardingDetailsQueryHandler(
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository,
    ILogger<GetOnboardingDetailsQueryHandler> logger,
    IPositionRepository positionRepository,
    IMapper mapper,
    IOnboardingRepository onboardingRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<GetOnboardingDetailsQuery, BaseResult<EmployeeOnboardingDetailsDto>>
{
    public async Task<BaseResult<EmployeeOnboardingDetailsDto>> Handle(GetOnboardingDetailsQuery request,
        CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        EmployeeOnboardingDetailsDto data =  new EmployeeOnboardingDetailsDto();
        try
        {
            var employee = await employeeRepository.GetByIdAsync(request.Id);
            
            if (employee == null)
            {
                return BaseResult<EmployeeOnboardingDetailsDto>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"No employee Found with this  Employee ID {request.Id}.",
                    nameof(request.Id)
                ));
            }
            var department =  await GetDepartmentWithManagerViewAsync(employee.DepartmentId);
            
            var position = await positionRepository.GetByIdAsync(employee.PositionId);
            
            var onboarding = await onboardingRepository.GetByEmployeeIdAsync(request.Id);

            if (onboarding == null)
            {
                return BaseResult<EmployeeOnboardingDetailsDto>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"No Onboarding Data found for this Employee ID {request.Id}.",
                    nameof(request.Id)
                ));
            }
            PersonalInfoDto personalInfo =  new PersonalInfoDto();
            
            personalInfo.FirstName = employee.Name.FirstName;
            personalInfo.LastName = employee.Name.LastName;
            personalInfo.Email = employee.Email;
            personalInfo.StartDate = onboarding.StartDate;
            personalInfo.Department = department.DepartmentName;
            personalInfo.Manager = department.ManagerFullName ?? "";
            personalInfo.Position = position.Title ?? "";
            data.PersonalInfo = personalInfo;
            data.EmployeeId = onboarding.EmployeeId;
            data.EmployeeNumber = employee.EmployeeNumber;
            data.DaysRemaining = onboarding.DaysRemaining;
            data.OnboardingStatus = onboarding.Status.ToString();
            data.OverallProgress = onboarding.OverallProgress;
            data.LastActivity = onboarding.LastActivity;
            data.CreatedDate = onboarding.CreatedDate;
            data.Documents = mapper.Map<List<OnboardingDocumentDto>>(onboarding.Documents);
            data.Stages = mapper.Map<List<OnboardingStageDto>>(onboarding.Stages);

            return BaseResult<EmployeeOnboardingDetailsDto>.Ok(data);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving Onboarding data for employee {EmployeeId}", request.Id);

            return BaseResult<EmployeeOnboardingDetailsDto>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while retrieving Onboarding data."
            ));
        }
    }

    private async Task<DepartmentWithManagerView> GetDepartmentWithManagerViewAsync(Guid departmentId)
    {
        var department = await departmentRepository.GetWithManagerByIdViewAsync(departmentId);
        return department;
    }
}