using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.CreateEmployee;

public sealed record CreateEmployeeCommand(
    Guid AzureAdId,
    string EmployeeNumber,
    string GovernmentId,
    string TaxIdentificationNumber,

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

    // Employment Details
    DateTime HireDate,
    string Status,
    string EmploymentType,
    bool IsFullTime,
    decimal FullTimeEquivalent,

    // Org Structure
    Guid DepartmentId,
    Guid PositionId,
    string JobTitle,

    // Compensation
    decimal BaseSalary,
    string PayFrequency,
    BankDetailsDto BankDetails,

    // Optional collections
    IEnumerable<EmergencyContactDto>? EmergencyContacts,
    IEnumerable<DependentDto>? Dependents,
    IEnumerable<SkillDto>? Skills,
    IEnumerable<CertificationDto>? Certifications,
    IEnumerable<EducationDto>? EducationHistory
) : IRequest<BaseResult<Guid>>;

