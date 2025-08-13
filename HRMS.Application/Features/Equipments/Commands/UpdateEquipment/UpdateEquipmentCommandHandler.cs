using HRMS.Application.Interfaces;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.Features.Equipments.Commands.UpdateEquipment;

/// <summary>
/// Handles updates to equipment records.
/// </summary>
public class UpdateEquipmentCommandHandler(
    IEquipmentRepository equipmentRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateEquipmentCommandHandler> logger)
    : IRequestHandler<UpdateEquipmentCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(UpdateEquipmentCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var equipment = await equipmentRepository.GetByIdAsync(request.Id);
            if (equipment is null)
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"Equipment with ID '{request.Id}' was not found.",
                    nameof(request.Id)
                ));
            }

            // Parse enum values from provided strings (numbers or names)
            var type = ParseEquipmentTypeFromStringOrNumber(request.Type);
            var condition = ParseEquipmentConditionFromStringOrNumber(request.Condition);

            equipment.UpdateDetails(
                request.SerialNumber,
                request.AssetTag,
                type,
                request.Brand,
                request.Model,
                condition,
                request.PurchaseDate,
                request.WarrantyExpiry,
                request.Notes);

            await equipmentRepository.Update(equipment);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return BaseResult<Guid>.Ok(equipment.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Failed to update equipment with ID {EquipmentId}", request.Id);
            return BaseResult<Guid>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while updating the equipment."
            ));
        }
    }

    private static EquipmentType ParseEquipmentTypeFromStringOrNumber(string input)
    {
        // Accept either numeric representation or string name
        if (int.TryParse(input, out int numeric) && Enum.IsDefined(typeof(EquipmentType), numeric))
        {
            return (EquipmentType)numeric;
        }
        if (Enum.TryParse<EquipmentType>(input, true, out var parsed))
        {
            return parsed;
        }
        throw new ArgumentException($"Invalid EquipmentType value: {input}");
    }

    private static EquipmentCondition ParseEquipmentConditionFromStringOrNumber(string input)
    {
        if (int.TryParse(input, out int numeric) && Enum.IsDefined(typeof(EquipmentCondition), numeric))
        {
            return (EquipmentCondition)numeric;
        }
        if (Enum.TryParse<EquipmentCondition>(input, true, out var parsed))
        {
            return parsed;
        }
        throw new ArgumentException($"Invalid EquipmentCondition value: {input}");
    }
}