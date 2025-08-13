using System;
using FluentValidation;

namespace HRMS.Application.Features.Employees.Queries.GetEmployeeDetail;

/// <summary>
/// Validator for <see cref="GetEmployeeDetailQuery"/>. Ensures that a non-empty identifier
/// is supplied when retrieving employee details.
/// </summary>
public class GetEmployeeDetailQueryValidator : AbstractValidator<GetEmployeeDetailQuery>
{
    public GetEmployeeDetailQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Employee ID is required.");
    }
}