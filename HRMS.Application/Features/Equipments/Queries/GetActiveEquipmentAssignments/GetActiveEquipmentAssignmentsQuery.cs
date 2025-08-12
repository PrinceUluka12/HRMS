using AutoMapper;
using HRMS.Application.Features.Equipments.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Equipments.Queries.GetActiveEquipmentAssignments;

public sealed record GetActiveEquipmentAssignmentsQuery() : IRequest<BaseResult<IEnumerable<EquipmentDto>>>;

public class GetActiveEquipmentAssignmentsQueryHandler(
    IEquipmentAssignmentRepository equipmentRepository,
    IMapper mapper,
    ILogger<GetActiveEquipmentAssignmentsQueryHandler> logger,
    ITranslator translator) : IRequestHandler<GetActiveEquipmentAssignmentsQuery, BaseResult<IEnumerable<EquipmentDto>>>
{
    public async Task<BaseResult<IEnumerable<EquipmentDto>>> Handle(GetActiveEquipmentAssignmentsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var equipments = await equipmentRepository.GetActiveEquipmentAsync(cancellationToken);
            var dto =  mapper.Map<IEnumerable<EquipmentDto>>(equipments);
            return BaseResult<IEnumerable<EquipmentDto>>.Ok(dto);
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