using System;
using FluentValidation;

namespace HRMS.Application.Features.Departments.Commands.UpdateDepartment;

/// <summary>
/// Validator for <see cref="UpdateDepartmentCommand"/>. Checks that the department ID is valid and
/// required fields (Name, Code) are not empty. Description may be optional.
/// </summary>
public class UpdateDepartmentCommandValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Department ID is required.");
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required.")
            .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.");
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Department code is required.")
            .MaximumLength(20).WithMessage("Department code cannot exceed 20 characters.");
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Department description cannot exceed 500 characters.");
        // ManagerId is nullable so no validation here.
    }
}