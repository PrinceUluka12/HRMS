using AutoMapper;
using HRMS.Application.Features.Equipments.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Equipments.Queries.GetEquipmentsByEmployee;

public record GetEquipmentsByEmployeeQuery(Guid EmployeeId) : IRequest<BaseResult<IEnumerable<EquipmentAssignmentDto>>>;

public class
    GetEquipmentsByEmployeeQueryHandler(
        IEquipmentAssignmentRepository equipmentRepository,
        IMapper mapper,
        ILogger<GetEquipmentsByEmployeeQueryHandler> logger,
        ITranslator translator) : IRequestHandler<GetEquipmentsByEmployeeQuery,
    BaseResult<IEnumerable<EquipmentAssignmentDto>>>
{
    public async Task<BaseResult<IEnumerable<EquipmentAssignmentDto>>> Handle(GetEquipmentsByEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var data = await equipmentRepository.GetByEmployeeAsync(request.EmployeeId, cancellationToken);
            if (data is null)
            {
                return new BaseResult<IEnumerable<EquipmentAssignmentDto>>();
            }

            var dto = mapper.Map<IEnumerable<EquipmentAssignmentDto>>(data);
            
            return BaseResult<IEnumerable<EquipmentAssignmentDto>>.Ok(dto);
        }
        catch (Exception ex)
        {
            return BaseResult<IEnumerable<EquipmentAssignmentDto>>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString(TranslatorMessages.GeneralMessages.Unexpected_Error(ex.Message))
            ));
        }
    }
}