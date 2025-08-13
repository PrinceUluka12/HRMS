using HRMS.Application.Wrappers;
using MediatR;
using System;

namespace HRMS.Application.Features.Departments.Commands.DeleteDepartment;

/// <summary>
/// Command used to delete a department by its identifier.
/// </summary>
public sealed record DeleteDepartmentCommand(Guid Id) : IRequest<BaseResult<Guid>>;