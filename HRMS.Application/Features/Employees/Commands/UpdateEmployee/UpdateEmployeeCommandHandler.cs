using AutoMapper;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.Features.Employees.Commands.UpdateEmployee;

/// <summary>
/// Handles the logic for updating an employee. This handler validates enums,
/// maps DTOs to value objects and updates the employee aggregate using domain methods.
/// </summary>
public class UpdateEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IDepartmentRepository departmentRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    ILogger<UpdateEmployeeCommandHandler> logger)
    : IRequestHandler<UpdateEmployeeCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // Fetch the existing employee
            var employee = await employeeRepository.GetByIdAsync(request.Id);
            if (employee is null)
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"Employee with ID '{request.Id}' was not found.",
                    nameof(request.Id)
                ));
            }

            // Parse enums from strings
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
                employmentType = ParseEmploymentTypeFromString(request.EmploymentType);
                payFrequency = ParsePayFrequencyFromString(request.PayFrequency);
            }
            catch (ArgumentException ex)
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.FieldDataInvalid,
                    ex.Message
                ));
            }

            // Map DTOs to value objects
            var name = new PersonName(request.FirstName, request.LastName);
            var email = new Email(request.Email);
            var address = mapper.Map<Address>(request.PrimaryAddress);
            var bankDetails = mapper.Map<BankDetails>(request.BankDetails);

            // Determine new manager ID from department
            Guid? managerId = null;
            try
            {
                var department = await departmentRepository.GetByIdAsync(request.DepartmentId);
                managerId = department?.ManagerId;
            }
            catch
            {
                // If department cannot be loaded we still proceed without updating manager
                managerId = null;
            }

            // Apply updates using domain methods
            employee.UpdatePersonalInformation(name, request.DateOfBirth, gender, maritalStatus);
            employee.UpdateContactInformation(email, request.WorkPhone, request.PersonalPhone, address, null);
            employee.UpdateEmploymentDetails(request.DepartmentId, request.PositionId, managerId, request.JobTitle, null, employmentType, request.IsFullTime, request.FullTimeEquivalent);
            employee.UpdateCompensation(request.BaseSalary, payFrequency, bankDetails);

            // Mark entity as modified
            await employeeRepository.Update(employee);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return BaseResult<Guid>.Ok(employee.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Failed to update employee with ID {EmployeeId}", request.Id);
            return BaseResult<Guid>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while updating the employee."
            ));
        }
    }

    private static EmploymentType ParseEmploymentTypeFromString(string type)
    {
        var normalized = type.Trim().ToLower();
        return normalized switch
        {
            "permanent" => EmploymentType.Permanent,
            "contract" => EmploymentType.Contract,
            "temporary" => EmploymentType.Temporary,
            "seasonal" => EmploymentType.Seasonal,
            "intern" => EmploymentType.Intern,
            _ => throw new ArgumentException($"Invalid employment type: {type}")
        };
    }

    private static PayFrequency ParsePayFrequencyFromString(string frequency)
    {
        var normalized = frequency.Trim().ToLower();
        return normalized switch
        {
            "weekly" => PayFrequency.Weekly,
            "biweekly" => PayFrequency.BiWeekly,
            "semi-monthly" or "semimonthly" => PayFrequency.SemiMonthly,
            "monthly" => PayFrequency.Monthly,
            _ => throw new ArgumentException($"Invalid pay frequency: {frequency}")
        };
    }
}