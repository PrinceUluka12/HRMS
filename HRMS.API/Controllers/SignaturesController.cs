using HRMS.Application.Features.DigitalSignature.Commands;
using HRMS.Application.Features.DigitalSignature.Dtos;
using HRMS.Application.Features.DigitalSignature.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/signatures")]
    public class SignaturesController : ControllerBase
    {
        private readonly IMediator _mediator;


        public SignaturesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<ActionResult<DigitalSignatureDto>> Create([FromBody] CreateSignatureCommand command)
        {
            if (command == null) return BadRequest();


            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet("employee/{employeeId:guid}")]
        public async Task<ActionResult<IEnumerable<DigitalSignatureDto>>> GetByEmployee([FromRoute] Guid employeeId)
        {
            var query = new GetEmployeeSignaturesQuery { EmployeeId = employeeId };
            var items = await _mediator.Send(query);
            return Ok(items);
        }


        [HttpGet("pending/{employeeId:guid}")]
        public async Task<ActionResult<IEnumerable<SignatureRequestDto>>> GetPending([FromRoute] Guid employeeId)
        {
            var query = new GetPendingSignaturesQuery { EmployeeId = employeeId };
            var items = await _mediator.Send(query);
            return Ok(items);
        }
    }
}
