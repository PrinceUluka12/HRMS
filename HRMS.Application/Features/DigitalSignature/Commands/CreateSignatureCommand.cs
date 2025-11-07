using AutoMapper;
using HRMS.Application.Features.DigitalSignature.Dtos;
using HRMS.Application.Features.Payroll.Dtos;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;




namespace HRMS.Application.Features.DigitalSignature.Commands
{
    public class CreateSignatureCommand : IRequest<BaseResult<DigitalSignatureDto>>
    {
        public Guid EmployeeId { get; set; }
        public Guid DocumentId { get; set; }
        public string SignatureData { get; set; }
        public string IpAddress { get; set; }
        public string DocumentName { get; set; }
        public string DocumentType { get; set; }
        public DateTime? RequiredBy { get; set; }
    }


    public class CreateSignatureCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ITranslator translator,
    IDigitalSignatureRepository _repository, ILogger<CreateSignatureCommandHandler> logger) : IRequestHandler<CreateSignatureCommand, BaseResult<DigitalSignatureDto>>
    {
        public async Task<BaseResult<DigitalSignatureDto>> Handle(CreateSignatureCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Map string doc type to enum safely
                if (!Enum.TryParse<DocumentType>(request.DocumentType, true, out var docType))
                {
                    docType = DocumentType.Other;
                }
                var signature = new HRMS.Domain.Aggregates.DigitalSignatureAggregate.DigitalSignature(request.EmployeeId, request.DocumentId, request.DocumentName ?? string.Empty, docType, request.SignatureData, request.IpAddress, request.RequiredBy);

                var added = await _repository.AddAsync(signature);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                // Map to DTO
                var dto = new DigitalSignatureDto
                {
                    Id = added.Id,
                    EmployeeId = added.EmployeeId,
                    DocumentId = added.DocumentId,
                    DocumentName = added.DocumentName,
                    DocumentType = added.DocumentType.ToString().ToLower(),
                    SignatureData = added.SignatureData,
                    SignedAt = added.SignedAt,
                    IpAddress = added.IpAddress,
                    Status = added.Status.ToString().ToLower(),
                    RequiredBy = added.RequiredBy
                };
                return BaseResult<DigitalSignatureDto>.Ok(dto);
            }
            catch (Exception ex)
            {

                logger.LogError(ex, "Error adding Digital Signature");
                return BaseResult<DigitalSignatureDto>.Failure(new Error(
                    ErrorCode.Exception,
                    translator.GetString("An unexpected error occurred while adding the Digital Signature.")
                ));
            }
        }
    }


}
