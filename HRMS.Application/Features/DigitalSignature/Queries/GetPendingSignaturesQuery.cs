using HRMS.Application.Features.DigitalSignature.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS.Application.Features.DigitalSignature.Queries
{
    public class GetPendingSignaturesQuery : IRequest<BaseResult<IEnumerable<SignatureRequestDto>>>
    {
        public Guid EmployeeId { get; set; }
    }

    public class GetPendingSignaturesHandler(IDigitalSignatureRepository _repository) : IRequestHandler<GetPendingSignaturesQuery, BaseResult<IEnumerable<SignatureRequestDto>>>
    {
        public async Task<BaseResult<IEnumerable<SignatureRequestDto>>> Handle(GetPendingSignaturesQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetPendingByEmployeeAsync(request.EmployeeId);


            // Example mapping to request DTO – in a real system you might have a separate SignatureRequest aggregate
            var data =  items.Select(i => new SignatureRequestDto
            {
                Id = i.Id,
                EmployeeId = i.EmployeeId,
                EmployeeName = string.Empty, // populate if employee data available via user service
                DocumentId = i.DocumentId,
                DocumentName = i.DocumentName,
                DocumentType = i.DocumentType.ToString().ToLower(),
                RequestedAt = i.SignedAt, // if pending, SignedAt may represent request time in some flows
                RequiredBy = i.RequiredBy,
                Status = i.Status.ToString().ToLower(),
                RemindersSent = 0
            });

            return BaseResult<IEnumerable<SignatureRequestDto>>.Ok(data);
        }
    }
}
