using AutoMapper;
using HRMS.Application.Features.BuddyPair.Dtos;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;

namespace HRMS.Application.Features.BuddyPairs.Queries.GetCheckinsByPairId;

public record GetCheckinsByPairIdQuery(Guid PairId) : IRequest<BaseResult<IEnumerable<BuddyCheckInDto>>>;

public class GetCheckinsByPairIdQueryHandler(
    IBuddyCheckInRepository repository,
    IMapper mapper,
    ITranslator translator)
    : IRequestHandler<GetCheckinsByPairIdQuery, BaseResult<IEnumerable<BuddyCheckInDto>>>
{
    public async Task<BaseResult<IEnumerable<BuddyCheckInDto>>> Handle(GetCheckinsByPairIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var buddyCheckIn = await repository.GetCheckinsByPairIdAsync(request.PairId);

            var dto = mapper.Map<IEnumerable<BuddyCheckInDto>>(buddyCheckIn);

            return BaseResult<IEnumerable<BuddyCheckInDto>>.Ok(dto);
        }
        catch (Exception ex)
        {
            return BaseResult<IEnumerable<BuddyCheckInDto>>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while fetching buddy checkins."
            ));
        }
    }
}