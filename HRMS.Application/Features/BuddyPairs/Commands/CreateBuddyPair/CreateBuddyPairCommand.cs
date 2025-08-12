using AutoMapper;
using HRMS.Application.Features.BuddyPair.Dtos;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.BuddyPair.Commands.CreateBuddyPair;

public record CreateBuddyPairCommand(
    Guid mentorId,
    Guid menteeId,
    DateTime startDate,
    Guid assignedBy,
    string? notes = null) : IRequest<BaseResult<BuddyPairDto>>;

public class CreateBuddyPairCommandHandler(
    IMapper mapper,
    IBuddyPairRepository buddyPairRepository,
    ITranslator translator,
    ILogger<CreateBuddyPairCommandHandler> logger,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateBuddyPairCommand, BaseResult<BuddyPairDto>>
{
    public async Task<BaseResult<BuddyPairDto>> Handle(CreateBuddyPairCommand request,
        CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var buddyPair = Domain.Aggregates.OnboardingAggregate.BuddyPair.Create(request.mentorId, request.menteeId,
                request.startDate, request.assignedBy);
            await buddyPairRepository.AddAsync(buddyPair);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            var dto = mapper.Map<BuddyPairDto>(buddyPair);
            return BaseResult<BuddyPairDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            return BaseResult<BuddyPairDto>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred. Please try again later."
            ));
        }
    }
}