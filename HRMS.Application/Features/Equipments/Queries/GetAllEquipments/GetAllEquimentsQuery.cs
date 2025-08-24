using AutoMapper;
using HRMS.Application.Features.Equipments.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Equipments.Queries.GetAllEquipments;

public record GetAllEquimentsQuery():  IRequest<BaseResult<IEnumerable<EquipmentDto>>>;

public class GetAllEquimentsQueryHandler(IMapper mapper,
    IEquipmentRepository equipmentRepository,
    ILogger<GetAllEquimentsQueryHandler>logger,
    ITranslator translator) : IRequestHandler<GetAllEquimentsQuery, BaseResult<IEnumerable<EquipmentDto>>>
{
    public async Task<BaseResult<IEnumerable<EquipmentDto>>> Handle(GetAllEquimentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var data =  await equipmentRepository.GetAllAsync();
            var result  =  mapper.Map<IEnumerable<EquipmentDto>>(data);
             return BaseResult<IEnumerable<EquipmentDto>>.Ok(result);
        }
        catch (Exception ex)
        {
            return BaseResult<IEnumerable<EquipmentDto>>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString(TranslatorMessages.GeneralMessages.Unexpected_Error(ex.Message))
            ));
        }
    }
}   