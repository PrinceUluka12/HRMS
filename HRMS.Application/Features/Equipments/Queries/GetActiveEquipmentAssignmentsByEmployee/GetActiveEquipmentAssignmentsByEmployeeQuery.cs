using AutoMapper;
using HRMS.Application.Features.Equipments.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;

namespace HRMS.Application.Features.Equipments.Queries.GetActiveEquipmentAssignmentsByEmployee;

public record GetActiveEquipmentAssignmentsByEmployeeQuery(Guid EmployeeId)
    : IRequest<BaseResult<IEnumerable<ActiveEquipmentDto>>>;

public class GetActiveEquipmentAssignmentsByEmployeeQueryHandler(
    IEquipmentAssignmentRepository equipmentAssignmentRepository,
    IEquipmentRepository equipmentRepository,
    IMapper mapper,
    ITranslator translator,
    IEmployeeRepository employeeRepository)
    : IRequestHandler<GetActiveEquipmentAssignmentsByEmployeeQuery, BaseResult<IEnumerable<ActiveEquipmentDto>>>
{
    public async Task<BaseResult<IEnumerable<ActiveEquipmentDto>>> Handle(
        GetActiveEquipmentAssignmentsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var list = new List<ActiveEquipmentDto>();
            var activeEquipment = new ActiveEquipmentDto();
            var equipments = await equipmentAssignmentRepository.GetActiveEquipmentAsync(cancellationToken);

            foreach (var equipment in equipments)
            {
                var equip =  await equipmentRepository.GetByIdAsync(equipment.EquipmentId);
                activeEquipment.EquipmentAssignmentId = equipment.Id;
                activeEquipment.EquipmentId = equipment.EquipmentId;
                activeEquipment.AssignedAt = equipment.AssignedAt;
                activeEquipment.AssignedBy = await GetEmploueeNameById(equipment.AssignedBy);
                activeEquipment.EmployeeId = equipment.EmployeeId;
                activeEquipment.Type = equip.Type.ToString();
                activeEquipment.Brand = equip.Brand;
                activeEquipment.Model = equip.Model;
                activeEquipment.SerialNumber = equip.SerialNumber;
                activeEquipment.AssetTag = equip.AssetTag;
                list.Add(activeEquipment);
            }
            return BaseResult<IEnumerable<ActiveEquipmentDto>>.Ok(list);
        }
        catch (Exception ex)
        {
            return BaseResult<IEnumerable<ActiveEquipmentDto>>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString(TranslatorMessages.GeneralMessages.Unexpected_Error(ex.Message))
            ));
        }
    }

    private async Task<string> GetEmploueeNameById(Guid id)
    {
        var data = await employeeRepository.GetByIdAsync(id);
        
       return  $"{data.Name.FirstName} {data.Name.LastName}";
    }
    
    
}