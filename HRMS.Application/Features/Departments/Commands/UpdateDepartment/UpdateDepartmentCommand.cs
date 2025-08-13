using HRMS.Application.Wrappers;
using MediatR;
using System;

namespace HRMS.Application.Features.Departments.Commands.UpdateDepartment;

/// <summary>
/// Command for updating an existing department's basic details and manager assignment.
/// </summary>
public sealed record UpdateDepartmentCommand(
    Guid Id,
    string Name,
    string Code,
    string Description,
    Guid? ManagerId
) : IRequest<BaseResult<Guid>>;