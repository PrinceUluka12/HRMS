using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Departments.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Domain.Aggregates.DepartmentAggregate;
using MediatR;

namespace HRMS.Application.Features.Departments.Queries.GetAllDepartments;

public sealed record GetAllDepartmentsQuery: IRequest<List<DepartmentDto>>;


public class GetAllDepartmentsQueryHandler(IDepartmentRepository departmentRepository, IMapper mapper) : IRequestHandler<GetAllDepartmentsQuery, List<DepartmentDto>>
{
    public async Task<List<DepartmentDto>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        var departments  =  await departmentRepository.GetAllAsync();
        
        var data  =  mapper.Map<List<DepartmentDto>>(departments);
        return data;
    }
}