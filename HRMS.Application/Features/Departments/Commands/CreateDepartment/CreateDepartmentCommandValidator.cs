using FluentValidation;

namespace HRMS.Application.Features.Departments.Commands.CreateDepartment;

/// <summary>
/// Validator for <see cref="CreateDepartmentCommand"/>. Ensures that required fields such as
/// Name, Code and Description are supplied and within reasonable length limits.
/// </summary>
public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required.")
            .MaximumLength(100).WithMessage("Department name cannot exceed 100 characters.");
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Department code is required.")
            .MaximumLength(20).WithMessage("Department code cannot exceed 20 characters.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Department description is required.")
            .MaximumLength(500).WithMessage("Department description cannot exceed 500 characters.");
        // ManagerId is optional; no validation required.
    }
}