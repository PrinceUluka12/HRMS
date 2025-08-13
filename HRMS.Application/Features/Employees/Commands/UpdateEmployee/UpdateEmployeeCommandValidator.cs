using System;
using FluentValidation;
using HRMS.Domain.Enums;

namespace HRMS.Application.Features.Employees.Commands.UpdateEmployee;

/// <summary>
/// Validator for <see cref="UpdateEmployeeCommand"/>. Ensures an existing employee ID is provided
/// and validates changes to personal details, organisational structure and compensation.
/// </summary>
public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        // Required identifiers
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Employee ID is required.");

        // Personal info
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters.");
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters.");
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");
        RuleFor(x => x.Gender)
            .Must(g => Enum.TryParse(typeof(Gender), g, ignoreCase: true, out _))
            .WithMessage($"Gender must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(Gender)))}");
        RuleFor(x => x.MaritalStatus)
            .Must(ms => Enum.TryParse(typeof(MaritalStatus), ms, ignoreCase: true, out _))
            .WithMessage($"Marital status must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(MaritalStatus)))}");

        // Contact info
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

        // Organisational structure
        RuleFor(x => x.DepartmentId)
            .NotEqual(Guid.Empty).WithMessage("Department ID is required.");
        RuleFor(x => x.PositionId)
            .NotEqual(Guid.Empty).WithMessage("Position ID is required.");
        RuleFor(x => x.JobTitle)
            .NotEmpty().WithMessage("Job title is required.")
            .MaximumLength(100).WithMessage("Job title cannot exceed 100 characters.");

        // Employment details
        RuleFor(x => x.EmploymentType)
            .Must(type => Enum.TryParse(typeof(EmploymentType), type, ignoreCase: true, out _))
            .WithMessage($"Employment type must be one of the following values: {string.Join(", ", Enum.GetNames(typeof(EmploymentType)))}");
        RuleFor(x => x.FullTimeEquivalent)
            .InclusiveBetween(0m, 1m).WithMessage("Full‑time equivalent must be between 0 and 1.");

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