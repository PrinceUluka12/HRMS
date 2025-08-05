using AutoMapper;
using HRMS.Application.Features.Departments.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Departments.Queries.GetAllDepartments;

public sealed record GetAllDepartmentsQuery : IRequest<BaseResult<List<DepartmentWithManagerView>>>;

public class GetAllDepartmentsQueryHandler(
    IDepartmentRepository departmentRepository,
    IMapper mapper,
    ILogger<GetAllDepartmentsQueryHandler> logger)
    : IRequestHandler<GetAllDepartmentsQuery, BaseResult<List<DepartmentWithManagerView>>>
{
    public async Task<BaseResult<List<DepartmentWithManagerView>>> Handle(GetAllDepartmentsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var departments = await departmentRepository.GetWithManagerViewAsync(cancellationToken);

            if (departments is null || !departments.Any())
            {
                return BaseResult<List<DepartmentWithManagerView>>.Failure(new Error(
                    ErrorCode.NotFound,
                    "No departments were found."
                ));
            }

            //var data = mapper.Map<List<DepartmentDto>>(departments);
            return BaseResult<List<DepartmentWithManagerView>>.Ok(departments);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve departments.");
            return BaseResult<List<DepartmentWithManagerView>>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while fetching departments."
            ));
        }
    }
}