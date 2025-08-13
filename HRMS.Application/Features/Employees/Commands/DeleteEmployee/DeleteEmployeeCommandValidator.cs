using System;
using FluentValidation;

namespace HRMS.Application.Features.Employees.Commands.DeleteEmployee;

/// <summary>
/// Validator for <see cref="DeleteEmployeeCommand"/>. Ensures a valid employee identifier is provided.
/// </summary>
public class DeleteEmployeeCommandValidator : AbstractValidator<DeleteEmployeeCommand>
{
    public DeleteEmployeeCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Employee ID is required.");
    }
}