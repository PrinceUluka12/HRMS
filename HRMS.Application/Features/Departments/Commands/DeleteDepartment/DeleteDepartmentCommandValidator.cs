using System;
using FluentValidation;

namespace HRMS.Application.Features.Departments.Commands.DeleteDepartment;

/// <summary>
/// Validator for <see cref="DeleteDepartmentCommand"/>. Ensures that a valid department identifier is provided.
/// </summary>
public class DeleteDepartmentCommandValidator : AbstractValidator<DeleteDepartmentCommand>
{
    public DeleteDepartmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Department ID is required.");
    }
}