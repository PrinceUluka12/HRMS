using AutoMapper;
using HRMS.Application.Features.Equipments.Dtos;
using HRMS.Application.Features.Onboarding.Dtos;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Equipments.Commands.ReturnEquipment;

public record ReturnEquipmentCommand(Guid returnedBy, string returnCondition, string returnNotes, Guid equipmentId): IRequest<BaseResult<EquipmentAssignmentDto>>;

public class ReturnEquipmentCommandHandler(IEquipmentRepository equipmentRepository,
    ITranslator translator,
    IUnitOfWork unitOfWork,
   ILogger<ReturnEquipmentCommandHandler> logger,
    IMapper mapper) : IRequestHandler<ReturnEquipmentCommand, BaseResult<EquipmentAssignmentDto>>
{
    public async Task<BaseResult<EquipmentAssignmentDto>> Handle(ReturnEquipmentCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var equipment = await equipmentRepository.GetByIdWithIncludesAsync(request.equipmentId, e => e.Assignments);
            if (equipment is  null)
            {
                return BaseResult<EquipmentAssignmentDto>.Failure(new Error(
                    ErrorCode.FieldDataInvalid,
                    $"An Equipment with the Id '{request.equipmentId}' does not exists.",
                    nameof(request.equipmentId)
                ));
            }
            var equipmentCondition = ParseEquipmentCondition(request.returnCondition);
            
            var assignment = equipment.MarkReturned(request.returnedBy, equipmentCondition, request.returnNotes);
            
            await equipmentRepository.Update(equipment);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return mapper.Map<EquipmentAssignmentDto>(assignment);;
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Unexpected error occurred while returning equipment.");

            return BaseResult<EquipmentAssignmentDto>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred. Please try again later."
            ));
        }
    }
    
    public EquipmentCondition ParseEquipmentCondition(string conditionStr)
    {
        switch (conditionStr.ToLowerInvariant())
        {
            case "new":
                return EquipmentCondition.New;
            case "good":
                return EquipmentCondition.Good;
            case "fair":
                return EquipmentCondition.Fair;
            case "poor":
                return EquipmentCondition.Poor;
            default:
                throw new ArgumentException($"Unknown equipment condition: {conditionStr}");
        }
    }

}