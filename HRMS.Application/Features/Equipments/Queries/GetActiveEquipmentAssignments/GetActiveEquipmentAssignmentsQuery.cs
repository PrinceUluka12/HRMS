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
    IEquipmentAssignmentRepository equipmentAssignmentRepository,
    IEquipmentRepository equipmentRepository,
    IMapper mapper,
    ILogger<GetActiveEquipmentAssignmentsQueryHandler> logger,
    ITranslator translator) : IRequestHandler<GetActiveEquipmentAssignmentsQuery, BaseResult<IEnumerable<EquipmentDto>>>
{
    public async Task<BaseResult<IEnumerable<EquipmentDto>>> Handle(GetActiveEquipmentAssignmentsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = new List<EquipmentDto>();
            var equipments = await equipmentAssignmentRepository.GetActiveEquipmentAsync(cancellationToken);

            if (equipments == null || !equipments.Any())
            {
                return BaseResult<IEnumerable<EquipmentDto>>.Ok(new List<EquipmentDto>(0));
            }
            else
            {
                foreach (var equipment in equipments)
                {
                    var data = await GetEquipment(equipment.EquipmentId);
                    result.Add(data);
                }

            }
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

    private async Task<EquipmentDto> GetEquipment(Guid EquipmentId)
    {
        return mapper.Map<EquipmentDto>(await equipmentRepository.GetByIdAsync(EquipmentId));
    }
}