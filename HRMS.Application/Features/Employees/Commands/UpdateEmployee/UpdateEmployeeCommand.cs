using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Wrappers;
using HRMS.Domain.Enums;
using MediatR;
using System;

namespace HRMS.Application.Features.Employees.Commands.UpdateEmployee;

/// <summary>
/// Command used to update an existing employee's details. Only mutable fields are exposed here.
/// Note that certain properties such as EmployeeNumber and GovernmentId are immutable once created.
/// </summary>
public sealed record UpdateEmployeeCommand(
    Guid Id,
    // Personal Info
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Gender,
    string MaritalStatus,
    // Contact Info
    string Email,
    string WorkPhone,
    string PersonalPhone,
    AddressDto PrimaryAddress,
    // Organisational Structure
    Guid DepartmentId,
    Guid PositionId,
    string JobTitle,
    // Employment Details
    string EmploymentType,
    bool IsFullTime,
    decimal FullTimeEquivalent,
    // Compensation
    decimal BaseSalary,
    string PayFrequency,
    BankDetailsDto BankDetails
) : IRequest<BaseResult<Guid>>;