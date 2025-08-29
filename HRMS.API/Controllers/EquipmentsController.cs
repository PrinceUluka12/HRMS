using HRMS.Application.Features.Equipments.Commands.AddEquipment;
using HRMS.Application.Features.Equipments.Commands.AssignEquipment;
using HRMS.Application.Features.Equipments.Commands.ReturnEquipment;
using HRMS.Application.Features.Equipments.Commands.UpdateEquipment;
using HRMS.Application.Features.Equipments.Commands.DeleteEquipment;
using HRMS.Application.Features.Equipments.Queries.GetActiveEquipmentAssignments;
using HRMS.Application.Features.Equipments.Queries.GetAllEquipments;
using HRMS.Application.Features.Equipments.Queries.GetEquipmentsByEmployee;
using HRMS.Application.Features.Equipments.Queries.GetEquipmentsByType;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using HRMS.Application.Wrappers;
using System;
using HRMS.Application.Features.Equipments.Queries.GetActiveEquipmentAssignmentsByEmployee;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EquipmentsController(IMediator mediator): ControllerBase
{
    [HttpPost("AssignEquipment")]
    public async Task<IActionResult> AssignEquipment(AssignEquipmentCommand assignEquipmentCommand)
    {
        var  result =  await mediator.Send(assignEquipmentCommand);
        return Ok(result);
    }
    
    [HttpPost("ReturnEquipment")]
    public async Task<IActionResult> ReturnEquipment(ReturnEquipmentCommand returnEquipmentCommand)
    {
        var  result =  await mediator.Send(returnEquipmentCommand);
        return Ok(result);
    }
    
    
    [HttpPost("AddEquipment")]
    public async Task<IActionResult> AddEquipment(AddEquipmentCommand command)
    {
        var  result =  await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing equipment record.
    /// </summary>
    [HttpPut("{id:guid}")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateEquipment(Guid id, [FromBody] UpdateEquipmentCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest(BaseResult<Guid>.Failure(new Error(
                ErrorCode.ModelStateNotValid,
                "Route ID does not match request body ID.",
                nameof(command.Id)
            )));
        }
        var result = await mediator.Send(command);
        return Ok(result);
    }

    /// <summary>
    /// Deletes an equipment record by ID.
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteEquipment(Guid id)
    {
        var result = await mediator.Send(new DeleteEquipmentCommand(id));
        return Ok(result);
    }

    [HttpGet("GetEquipmentsByType")]
    public async Task<IActionResult> GetEquipmentsByType([FromQuery] GetEquipmentsByTypeQuery getEquipmentsByTypeQuery)
    {
        var  result =  await mediator.Send(getEquipmentsByTypeQuery);
        return Ok(result);
    }

    [HttpGet("assignments/employee/{employeeId:guid}")]
    public async Task<IActionResult> GetEmployeeAssignments(Guid employeeId)
    {
        var result =  await mediator.Send(new GetEquipmentsByEmployeeQuery(employeeId));
        return Ok(result);
    }
    
    [HttpGet("assignments/active")]
    public async Task<IActionResult> GetActiveEquipments()
    {
        var result =  await mediator.Send(new GetActiveEquipmentAssignmentsQuery());
        return Ok(result);
    }
    
    [HttpGet("assignments/active/employee")]
    public async Task<IActionResult> GetActiveEquipments([FromQuery]GetActiveEquipmentAssignmentsByEmployeeQuery query)
    {
        var result =  await mediator.Send(query);
        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAllEquiments([FromQuery]GetAllEquimentsQuery query)
    {
        var result =  await mediator.Send(query);
        return Ok(result);
    }
    
    
}