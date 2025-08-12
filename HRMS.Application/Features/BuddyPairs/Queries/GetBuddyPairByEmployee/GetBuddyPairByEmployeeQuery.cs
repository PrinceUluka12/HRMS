using AutoMapper;
using HRMS.Application.Features.BuddyPair.Dtos;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.BuddyPair.Queries.GetBuddyPairByEmployee;

public record GetBuddyPairByEmployeeQuery(Guid EmployeeId) : IRequest<BaseResult<IEnumerable<BuddyPairDto>>>;

public class
    GetBuddyPairByEmployeeQueryHandler(
        IBuddyPairRepository buddyPairRepository,
        IMapper mapper,
        ILogger<GetBuddyPairByEmployeeQueryHandler> logger,
        ITranslator translator) : IRequestHandler<GetBuddyPairByEmployeeQuery,
    BaseResult<IEnumerable<BuddyPairDto>>>
{
    public async Task<BaseResult<IEnumerable<BuddyPairDto>>> Handle(GetBuddyPairByEmployeeQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var buddyPair = await buddyPairRepository.GetAllByEmployee(request.EmployeeId);
            var dto = mapper.Map<IEnumerable<BuddyPairDto>>(buddyPair);
            return BaseResult<IEnumerable<BuddyPairDto>>.Ok(dto);
        }
        catch (Exception ex)
        {
            return BaseResult<IEnumerable<BuddyPairDto>>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while fetching buddy pairs."
            ));
        }
    }
}