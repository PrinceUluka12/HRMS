using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Departments.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.DepartmentAggregate;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(
    string Name,
    string Code,
    string Description,
    Guid? ManagerId) : IRequest<DepartmentDto>;

public class CreateDepartmentCommandHandler(
    IDepartmentRepository departmentRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateDepartmentCommand, DepartmentDto>
{
    public async Task<DepartmentDto> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        var department = new Department(
            request.Name,
            request.Code,
            request.Description,
            request.ManagerId);

        await departmentRepository.AddAsync(department);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<DepartmentDto>(department);
    }
}