using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Positions.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Positions.Queries.GetAllPositions;

public record GetAllPositionsQuery : IRequest<BaseResult<IEnumerable<PositionDto>>>;

public class GetAllPositionsQueryHandler(
    IPositionRepository positionRepository,
    IMapper mapper,
    ICurrentUserService currentUser,
    ILogger<GetAllPositionsQueryHandler> logger)
    : IRequestHandler<GetAllPositionsQuery, BaseResult<IEnumerable<PositionDto>>>
{
    public async Task<BaseResult<IEnumerable<PositionDto>>> Handle(GetAllPositionsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var positions = await positionRepository.GetAllAsync();

            if (positions == null || !positions.Any())
            {
                return BaseResult<IEnumerable<PositionDto>>.Failure(new Error(
                    ErrorCode.NotFound,
                    "No positions found."
                ));
            }

            var data = positions.Select(position => new PositionDto(
                position.Id,
                position.Title,
                position.Code,
                position.BaseSalary,
                position.Description,
                position.DepartmentId
            )).ToList();

            return BaseResult<IEnumerable<PositionDto>>.Ok(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while retrieving all positions.");
            return BaseResult<IEnumerable<PositionDto>>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while retrieving positions."
            ));
        }
    }
}