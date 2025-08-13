using System;
using FluentValidation;
using HRMS.Application.Features.Employees.Commands.CreateEmployee;
using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Employees.Commands.CreateEmployee;

/// <summary>
/// Validator for <see cref="CreateEmployeeCommand"/>. Ensures that required fields are
/// provided and conform to basic business rules such as non-empty identifiers, dates in the past
/// and valid enumeration values. Having validators helps centralize validation logic and
/// makes controllers thinner; validation errors are automatically returned as 400 Bad Request
/// when integrated with FluentValidation.
/// </summary>
public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        // Unique identifiers must be provided
        RuleFor(x => x.AzureAdId)
            .NotEqual(Guid.Empty)
            .WithMessage("Azure AD identifier is required.");

        // Employee number and government ID should not be empty
        RuleFor(x => x.EmployeeNumber)
            .NotEmpty().WithMessage("Employee number is required.")
            .MaximumLength(50).WithMessage("Employee number cannot exceed 50 characters.");
        RuleFor(x => x.GovernmentId)
            .NotEmpty().WithMessage("Government ID is required.")
            .MaximumLength(50).WithMessage("Government ID cannot exceed 50 characters.");
        RuleFor(x => x.TaxIdentificationNumber)
            .NotEmpty().WithMessage("Tax identification number is required.")
            .MaximumLength(50).WithMessage("Tax identification number cannot exceed 50 characters.");

        // Personal information
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");
        RuleFor(x => x.Gender)
            .Must(gender => Enum.TryParse(typeof(Gender), gender, ignoreCase: true, out _))
            .WithMessage($"Gender must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(Gender)))}");
        RuleFor(x => x.MaritalStatus)
            .Must(status => Enum.TryParse(typeof(MaritalStatus), status, ignoreCase: true, out _))
            .WithMessage($"Marital status must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(MaritalStatus)))}");

        // Contact information
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Email address is not valid.");
        RuleFor(x => x.WorkPhone)
            .NotEmpty().WithMessage("Work phone number is required.")
            .MaximumLength(20).WithMessage("Work phone number cannot exceed 20 characters.");
        RuleFor(x => x.PersonalPhone)
            .NotEmpty().WithMessage("Personal phone number is required.")
            .MaximumLength(20).WithMessage("Personal phone number cannot exceed 20 characters.");
        RuleFor(x => x.PrimaryAddress)
            .NotNull().WithMessage("Primary address is required.");

        // Employment details
        RuleFor(x => x.HireDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Hire date cannot be in the future.");
        RuleFor(x => x.Status)
            .Must(status => !string.IsNullOrWhiteSpace(status) && Enum.TryParse(typeof(EmploymentStatus), status, ignoreCase: true, out _))
            .WithMessage($"Status must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(EmploymentStatus)))}");
        RuleFor(x => x.EmploymentType)
            .Must(type => Enum.TryParse(typeof(EmploymentType), type, ignoreCase: true, out _))
            .WithMessage($"Employment type must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(EmploymentType)))}");
        RuleFor(x => x.FullTimeEquivalent)
            .InclusiveBetween(0m, 1m).WithMessage("Full‑time equivalent must be between 0 and 1.");

        // Organisational structure
        RuleFor(x => x.DepartmentId)
            .NotEqual(Guid.Empty).WithMessage("Department ID is required.");
        RuleFor(x => x.PositionId)
            .NotEqual(Guid.Empty).WithMessage("Position ID is required.");
        RuleFor(x => x.JobTitle)
            .NotEmpty().WithMessage("Job title is required.")
            .MaximumLength(100).WithMessage("Job title cannot exceed 100 characters.");

        // Compensation
        RuleFor(x => x.BaseSalary)
            .GreaterThanOrEqualTo(0m).WithMessage("Base salary must be a non‑negative amount.");
        RuleFor(x => x.PayFrequency)
            .Must(freq => Enum.TryParse(typeof(PayFrequency), freq, ignoreCase: true, out _))
            .WithMessage($"Pay frequency must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(PayFrequency)))}");
        RuleFor(x => x.BankDetails)
            .NotNull().WithMessage("Bank details are required.");
    }
}