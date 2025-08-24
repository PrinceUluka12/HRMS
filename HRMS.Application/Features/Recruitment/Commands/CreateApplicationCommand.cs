using HRMS.Application.Features.Recruitment.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Aggregates.RecruitmentAggregates;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HRMS.Application.Features.Recruitment.Commands;

public record CreateApplicationCommand(
    Guid JobVacancyId,
    string? Note,
    CandidateDto candidate,
    IFormFile Resume) : IRequest<BaseResult<Guid>>;

public class CreateApplicationCommandHandler(
    IAzureBlobStorageService storageService,
    IUnitOfWork unitOfWork,
    IApplicationRepository applicationRepository,
    ICandidateRepository candidateRepository) : IRequestHandler<CreateApplicationCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            using var stream = request.Resume.OpenReadStream();

            var fileUrl =
                await storageService.UploadFileAsync(stream, request.Resume.FileName, request.Resume.ContentType);

            var personName = new PersonName(request.candidate.FirstName, request.candidate.LastName);
            

            var candidateInfo = new Candidate(request.JobVacancyId, personName, request.candidate.Email,
                fileUrl,
                request.candidate.PhoneNumber);

            var applicationData =
                new HRMS.Domain.Aggregates.RecruitmentAggregates.Application(request.JobVacancyId, candidateInfo);

            var attachment = new Attachment(request.Resume.FileName, request.Resume.ContentType, fileUrl);

            applicationData.AddAttachment(attachment);

            if (!string.IsNullOrEmpty(request.Note))
            {
                applicationData.AddNote(request.Note);
            }

            await applicationRepository.AddAsync(applicationData);
            
            //await candidateRepository.AddAsync(candidateInfo);

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return BaseResult<Guid>.Ok(applicationData.Id);
        }
        catch (Exception e)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            return BaseResult<Guid>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while creating the application."
            ));
        }
    }
}