using AutoMapper;
using HRMS.Application.Features.Equipments.Dtos;
using HRMS.Application.Features.Onboarding.Dtos;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Equipments.Commands.AddEquipment;

public record AddEquipmentCommand(
    string serialNumber,
    string assetTag,
    string type,
    string brand,
    string model,
    string condition,
    DateTime purchaseDate,
    DateTime warrantyExpiry) : IRequest<BaseResult<EquipmentDto>>;

public class AddEquipmentCommandHandler(
    IEquipmentRepository equipmentRepository,
    IMapper mapper,
    ITranslator translator,
    ILogger<AddEquipmentCommandHandler> logger,
    IUnitOfWork unitOfWork
) : IRequestHandler<AddEquipmentCommand, BaseResult<EquipmentDto>>
{
    public async Task<BaseResult<EquipmentDto>> Handle(AddEquipmentCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var type = ParseEquipmentTypeFromStringNumber(request.type);
            var condition = ParseEquipmentConditionFromStringNumber(request.condition);
            
            var equipment = new Equipment(request.serialNumber, request.assetTag, type, request.brand, request.model,
                condition, request.purchaseDate, request.warrantyExpiry);

            await equipmentRepository.AddAsync(equipment);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            
            var dto = mapper.Map<EquipmentDto>(equipment);
            return BaseResult<EquipmentDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while adding equipment.");

            return BaseResult<EquipmentDto>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred. Please try again later."
            ));
        }
    }

    public EquipmentCondition ParseEquipmentConditionFromStringNumber(string input)
    {
        if (int.TryParse(input, out int value) &&
            Enum.IsDefined(typeof(EquipmentCondition), value))
        {
            return (EquipmentCondition)value;
        }
        throw new ArgumentException($"Invalid EquipmentCondition number: {input}");
    }

    public EquipmentType ParseEquipmentTypeFromStringNumber(string input)
    {
        if (int.TryParse(input, out int value) &&
            Enum.IsDefined(typeof(EquipmentType), value))
        {
            return (EquipmentType)value;
        }
        throw new ArgumentException($"Invalid EquipmentType number: {input}");
    }

}