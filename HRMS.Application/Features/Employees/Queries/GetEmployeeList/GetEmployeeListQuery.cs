using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Employees.Queries.GetEmployeeList;

public record GetEmployeeListQuery : IRequest<BaseResult<List<EmployeeListDto>>>;

public class GetEmployeeListQueryHandler(
    IEmployeeRepository employeeRepository,
    ITranslator translator,
    ILogger<GetEmployeeListQueryHandler> logger)
    : IRequestHandler<GetEmployeeListQuery, BaseResult<List<EmployeeListDto>>>
{
    public async Task<BaseResult<List<EmployeeListDto>>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employees = await employeeRepository.GetListWithDepAndPosAsync(cancellationToken);

            if (employees == null || employees.Count == 0)
            {
                return BaseResult<List<EmployeeListDto>>.Ok(new List<EmployeeListDto>());
            }

            var data = employees.Select(emp => new EmployeeListDto(
                emp.Id,
                emp.EmployeeNumber,
                emp.Name.FirstName,
                emp.Name.LastName,
                emp.JobTitle,
                emp.WorkPhone,
                emp.Department?.Name ?? string.Empty,
                emp.Position?.Title ?? string.Empty,
                emp.Status.ToString(),
                emp.HireDate
            )).ToList();

            return BaseResult<List<EmployeeListDto>>.Ok(data);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve employee list.");
            return BaseResult<List<EmployeeListDto>>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString(TranslatorMessages.GeneralMessages.Unexpected_Error("Employee List"))
            ));
        }
    }
}
