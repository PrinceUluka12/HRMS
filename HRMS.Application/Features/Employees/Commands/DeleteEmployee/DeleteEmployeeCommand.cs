using HRMS.Application.Wrappers;
using MediatR;
using System;

namespace HRMS.Application.Features.Employees.Commands.DeleteEmployee;

/// <summary>
/// Command used to delete an existing employee by ID.
/// </summary>
public sealed record DeleteEmployeeCommand(Guid Id) : IRequest<BaseResult<Guid>>;