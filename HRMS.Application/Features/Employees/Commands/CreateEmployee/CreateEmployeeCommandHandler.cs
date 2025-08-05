using AutoMapper;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler(
    IDepartmentRepository departmentRepository,
    IEmployeeRepository employeeRepository,
    IAzureAdService azureAdService,
    IMediator mediator,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    ILogger<CreateEmployeeCommandHandler> logger,
    IOnboardingService onboardingService)
    : IRequestHandler<CreateEmployeeCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(CreateEmployeeCommand request,
        CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Verify Azure AD user exists

            // Check Azure AD existence
            if (!await azureAdService.VerifyUserExistsAsync(request.AzureAdId))
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"Azure AD user with ID '{request.AzureAdId}' was not found.",
                    nameof(request.AzureAdId)
                ));
            }

            // Map enums safely
            if (!Enum.TryParse<Gender>(request.Gender, true, out var gender))
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.FieldDataInvalid,
                    $"Invalid gender value '{request.Gender}'.",
                    nameof(request.Gender)
                ));
            }

            if (!Enum.TryParse<MaritalStatus>(request.MaritalStatus, true, out var maritalStatus))
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.FieldDataInvalid,
                    $"Invalid marital status value '{request.MaritalStatus}'.",
                    nameof(request.MaritalStatus)
                ));
            }

            EmploymentType employmentType;
            PayFrequency payFrequency;

            try
            {
                employmentType = GetEmploymentTypeFromString(request.EmploymentType);
                payFrequency = GetPayFrequencyFromString(request.PayFrequency);
            }
            catch (ArgumentException ex)
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.FieldDataInvalid,
                    ex.Message
                ));
            }

            // Map value objects
            var name = new PersonName(request.FirstName, request.LastName);
            var email = new Email(request.Email);
            var phone = request.PersonalPhone;
            var address = mapper.Map<Address>(request.PrimaryAddress);
            var bank = mapper.Map<BankDetails>(request.BankDetails);
            var managerId =  await GetDepartmentManagerId(request.DepartmentId);


            var employee =  Employee.Create(
                request.AzureAdId,
                request.EmployeeNumber,
                request.GovernmentId,
                request.TaxIdentificationNumber,
                name,
                request.DateOfBirth,
                gender,
                maritalStatus,
                email,
                request.WorkPhone,
                phone,
                address,
                request.HireDate,
                employmentType,
                request.IsFullTime,
                request.DepartmentId,
                request.PositionId,
                managerId,
                request.JobTitle,
                request.BaseSalary,
                payFrequency,
                bank);

            await employeeRepository.AddAsync(employee);

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            
            await onboardingService.BeginOnboardingAsync(employee.Id, cancellationToken);
            return BaseResult<Guid>.Ok(employee.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Failed to create employee.");

            return BaseResult<Guid>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while creating the employee."
            ));
        }
    }

    private EmploymentType GetEmploymentTypeFromString(string type)
    {
        return type.Trim().ToLower() switch
        {
            "permanent" => EmploymentType.Permanent,
            "contract" => EmploymentType.Contract,
            "temporary" => EmploymentType.Temporary,
            "seasonal" => EmploymentType.Seasonal,
            "intern" => EmploymentType.Intern,
            _ => throw new ArgumentException($"Invalid employment type: {type}")
        };
    }

    private PayFrequency GetPayFrequencyFromString(string frequency)
    {
        return frequency.Trim().ToLower() switch
        {
            "weekly" => PayFrequency.Weekly,
            "biweekly" => PayFrequency.BiWeekly,
            "semi-monthly" or "semimonthly" => PayFrequency.SemiMonthly,
            "monthly" => PayFrequency.Monthly,
            _ => throw new ArgumentException($"Invalid pay frequency: {frequency}")
        };
    }

    private async Task<Guid?> GetDepartmentManagerId(Guid DepartmentId)
    {
        var dep =  await departmentRepository.GetByIdAsync(DepartmentId);
        return dep.ManagerId;
    }
}