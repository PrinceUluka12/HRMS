using HRMS.Application.Interfaces;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using HRMS.Application.Interfaces.Repositories;

namespace HRMS.Application.Features.Equipments.Commands.DeleteEquipment;

/// <summary>
/// Handles deletion of an equipment asset.
/// </summary>
public class DeleteEquipmentCommandHandler(
    IEquipmentRepository equipmentRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteEquipmentCommandHandler> logger)
    : IRequestHandler<DeleteEquipmentCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(DeleteEquipmentCommand request, CancellationToken cancellationToken)
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
            await equipmentRepository.Delete(equipment);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return BaseResult<Guid>.Ok(equipment.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Failed to delete equipment with ID {EquipmentId}", request.Id);
            return BaseResult<Guid>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while deleting the equipment."
            ));
        }
    }
}