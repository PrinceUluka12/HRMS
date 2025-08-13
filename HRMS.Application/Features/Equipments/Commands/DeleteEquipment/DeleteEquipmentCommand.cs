using HRMS.Application.Wrappers;
using MediatR;
using System;

namespace HRMS.Application.Features.Equipments.Commands.DeleteEquipment;

/// <summary>
/// Command for deleting an equipment record by ID.
/// </summary>
public sealed record DeleteEquipmentCommand(Guid Id) : IRequest<BaseResult<Guid>>;