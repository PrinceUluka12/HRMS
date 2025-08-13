using HRMS.Application.Wrappers;
using MediatR;
using System;

namespace HRMS.Application.Features.Equipments.Commands.UpdateEquipment;

/// <summary>
/// Command to update details of an equipment asset.
/// </summary>
public sealed record UpdateEquipmentCommand(
    Guid Id,
    string SerialNumber,
    string AssetTag,
    string Type,
    string Brand,
    string Model,
    string Condition,
    DateTime PurchaseDate,
    DateTime WarrantyExpiry,
    string? Notes
) : IRequest<BaseResult<Guid>>;