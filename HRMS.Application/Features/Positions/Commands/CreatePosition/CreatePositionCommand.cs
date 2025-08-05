using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Positions.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.DepartmentAggregate;
using HRMS.Domain.Aggregates.PositionAggregate;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Positions.Commands.CreatePosition;

public record CreatePositionCommand(
    string Title,
    string Code,
    decimal BaseSalary,
    string Description,
    Guid DepartmentId) : IRequest<BaseResult<PositionDto>>;

public class CreatePositionCommandHandler(
    IPositionRepository positionRepository,
    IDepartmentRepository departmentRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    ITranslator translator,
    ILogger<CreatePositionCommandHandler> logger)
    : IRequestHandler<CreatePositionCommand, BaseResult<PositionDto>>
{
    public async Task<BaseResult<PositionDto>> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var department = await departmentRepository.GetByIdWithIncludesAsync(request.DepartmentId);
            if (department is null)
            {
                return BaseResult<PositionDto>.Failure(new Error(
                    ErrorCode.NotFound,
                    translator.GetString($"Department with ID {request.DepartmentId} not found."),
                    nameof(request.DepartmentId)
                ));
            }

            var position = new Position(
                request.Title,
                request.Code,
                request.BaseSalary,
                request.Description,
                request.DepartmentId);

            await positionRepository.AddAsync(position);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = mapper.Map<PositionDto>(position);
            return BaseResult<PositionDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating position with code {Code}", request.Code);

            return BaseResult<PositionDto>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString("An unexpected error occurred while creating the position.")
            ));
        }
    }
}
