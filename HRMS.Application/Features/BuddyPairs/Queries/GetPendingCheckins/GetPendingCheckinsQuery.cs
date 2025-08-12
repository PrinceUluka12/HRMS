using AutoMapper;
using HRMS.Application.Features.BuddyPair.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;

namespace HRMS.Application.Features.BuddyPairs.Queries.GetPendingCheckins;

public record GetPendingCheckinsQuery(Guid EmployeeId) : IRequest<BaseResult<IEnumerable<BuddyPairDto>>>;

public class GetPendingCheckinsQueryHandler(IBuddyPairRepository repository, IMapper mapper)
    : IRequestHandler<GetPendingCheckinsQuery, BaseResult<IEnumerable<BuddyPairDto>>>
{
    public async Task<BaseResult<IEnumerable<BuddyPairDto>>> Handle(GetPendingCheckinsQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var buddyPair = await repository.GetPendingCheckinsByEmployee(request.EmployeeId);

            var dto = mapper.Map<IEnumerable<BuddyPairDto>>(buddyPair);

            return BaseResult<IEnumerable<BuddyPairDto>>.Ok(dto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}