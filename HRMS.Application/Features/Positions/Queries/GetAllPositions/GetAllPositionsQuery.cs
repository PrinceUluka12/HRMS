using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Positions.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using MediatR;

namespace HRMS.Application.Features.Positions.Queries.GetAllPositions;
public class GetAllPositionsQuery : IRequest<List<PositionDto>>
{
}
public class GetAllPositionsQueryHandler(IPositionRepository positionRepository, IMapper mapper, ICurrentUserService currentUser) : IRequestHandler<GetAllPositionsQuery,List<PositionDto>>
{
    public async Task<List<PositionDto>> Handle(GetAllPositionsQuery request, CancellationToken cancellationToken)
    {
        var positions = await positionRepository.GetAllAsync();
        
         var data = mapper.Map<List<PositionDto>>(positions);

         return data;
    }
}