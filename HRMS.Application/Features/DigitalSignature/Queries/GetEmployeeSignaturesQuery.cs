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
    public class GetEmployeeSignaturesQuery : IRequest<BaseResult<IEnumerable<DigitalSignatureDto>>>
    {
        public Guid EmployeeId { get; set; }
    }

    public class GetEmployeeSignaturesHandler : IRequestHandler<GetEmployeeSignaturesQuery, BaseResult<IEnumerable<DigitalSignatureDto>>>
    {
        private readonly IDigitalSignatureRepository _repository;


        public GetEmployeeSignaturesHandler(IDigitalSignatureRepository repository)
        {
            _repository = repository;
        }


        public async Task<BaseResult<IEnumerable<DigitalSignatureDto>>> Handle(GetEmployeeSignaturesQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetByEmployeeAsync(request.EmployeeId);


            var data =  items.Select(i => new DigitalSignatureDto
            {
                Id = i.Id,
                EmployeeId = i.EmployeeId,
                DocumentId = i.DocumentId,
                DocumentName = i.DocumentName,
                DocumentType = i.DocumentType.ToString().ToLower(),
                SignatureData = i.SignatureData,
                SignedAt = i.SignedAt,
                IpAddress = i.IpAddress,
                Status = i.Status.ToString().ToLower(),
                RequiredBy = i.RequiredBy
            });
            return BaseResult<IEnumerable<DigitalSignatureDto>>.Ok(data);
        }
    }
}
