using AutoMapper;
using HRMS.Application.Features.Equipments.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Wrappers;
using HRMS.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Equipments.Queries.GetEquipmentsByType;

public record GetEquipmentsByTypeQuery(string Type) : IRequest<BaseResult<IEnumerable<EquipmentDto>>>;

public class GetEquipmentsByTypeQueryHandler(IMapper mapper,
    ITranslator translator,
    IEquipmentRepository equipmentRepository,
    ILogger<GetEquipmentsByTypeQueryHandler> logger) : IRequestHandler<GetEquipmentsByTypeQuery, BaseResult<IEnumerable<EquipmentDto>>>
{
    public async Task<BaseResult<IEnumerable<EquipmentDto>>> Handle(GetEquipmentsByTypeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var type  = ParseEquipmentType(request.Type);
            var data = await equipmentRepository.GetByTypeAsync(type, cancellationToken);
            return mapper.Map<BaseResult<IEnumerable<EquipmentDto>>>(data);
        }
        catch (Exception ex)
        {
            return BaseResult<IEnumerable<EquipmentDto>>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString(TranslatorMessages.GeneralMessages.Unexpected_Error(ex.Message))
            ));
        }
    }
    
    private  EquipmentType ParseEquipmentType(string typeStr)
    {
        switch (typeStr.ToLowerInvariant())
        {
            case "laptop":
                return EquipmentType.Laptop;
            case "phone":
                return EquipmentType.Phone;
            case "monitor":
                return EquipmentType.Monitor;
            case "other":
                return EquipmentType.Other;
            default:
                throw new ArgumentException($"Unknown equipment type: {typeStr}");
        }
    }

}
