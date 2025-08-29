using AutoMapper;
using HRMS.Application.Features.Equipments.Dtos;
using HRMS.Application.Features.Onboarding.Dtos;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Equipments.Commands.AssignEquipment;

public record AssignEquipmentCommand(Guid EquipmentId, Guid EmployeeId, Guid AssignedById)
    : IRequest<BaseResult<EquipmentAssignmentDto>>;

public class AssignEquipmentCommandHandler(
    IEquipmentRepository equipmentRepository,
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ILogger<AssignEquipmentCommandHandler> logger,
    ITranslator translator) : IRequestHandler<AssignEquipmentCommand, BaseResult<EquipmentAssignmentDto>>
{
    public async Task<BaseResult<EquipmentAssignmentDto>> Handle(AssignEquipmentCommand request,
        CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var equipment = await equipmentRepository.GetByIdAsync(request.EquipmentId);
            if (equipment is null)
            {
                return BaseResult<EquipmentAssignmentDto>.Failure(new Error(
                    ErrorCode.FieldDataInvalid,
                    $"An Equipment with the Id '{request.EquipmentId}' does not exists.",
                    nameof(request.EquipmentId)
                ));
            }

            var assignment = equipment.AssignToEmployee(request.EmployeeId, request.AssignedById);

            await equipmentRepository.Update(equipment);
            await equipmentRepository.AddAssignmentAsync(assignment, cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);


            return mapper.Map<EquipmentAssignmentDto>(assignment);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while assigning equipment.");

            return BaseResult<EquipmentAssignmentDto>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred. Please try again later."
            ));
        }
    }
}