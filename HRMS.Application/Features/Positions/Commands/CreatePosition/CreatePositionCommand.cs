using AutoMapper;
using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Positions.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.DepartmentAggregate;
using HRMS.Domain.Aggregates.PositionAggregate;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Positions.Commands.CreatePosition;

public record CreatePositionCommand(
    string Title,
    string Code,
    decimal BaseSalary,
    string Description,
    Guid DepartmentId) : IRequest<PositionDto>;

public class CreatePositionCommandHandler(
    IPositionRepository positionRepository,
    IDepartmentRepository departmentRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePositionCommand, PositionDto>
{
    public async Task<PositionDto> Handle(CreatePositionCommand request, CancellationToken cancellationToken)
    {
        var department = await departmentRepository.GetByIdAsyncIncludeRelationship(request.DepartmentId);
        if (department == null)
        {
            throw new NotFoundException(nameof(Department), request.DepartmentId);
        }

        var position = new Position(
            request.Title,
            request.Code,
            request.BaseSalary,
            request.Description,
            request.DepartmentId);

        await positionRepository.AddAsync(position);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<PositionDto>(position);
    }
}