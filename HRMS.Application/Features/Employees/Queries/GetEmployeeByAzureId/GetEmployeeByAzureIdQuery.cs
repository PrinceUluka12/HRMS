using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Employees.Queries.GetEmployeeByAzureId;

public record GetEmployeeByAzureIdQuery(Guid Id) : IRequest<BaseResult<EmployeeListDto>>;

public class GetEmployeeByAzureIdQueryHandler(
    IEmployeeRepository employeeRepository,
    ITranslator translator,
    ILogger<GetEmployeeByAzureIdQueryHandler> logger)
    : IRequestHandler<GetEmployeeByAzureIdQuery, BaseResult<EmployeeListDto>>
{
    public async Task<BaseResult<EmployeeListDto>> Handle(GetEmployeeByAzureIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await employeeRepository.GetByAzureAdIdAsync(request.Id);
            if (employee is null)
            {
                return BaseResult<EmployeeListDto>.Failure(new Error(
                    ErrorCode.NotFound,
                    translator.GetString(
                        TranslatorMessages.EmployeeMessages.Employee_NotFound_with_AzureEmployeeId(request.Id)),
                    nameof(request.Id)
                ));
            }

            var dto = new EmployeeListDto(
                employee.Id,
                employee.EmployeeNumber,
                employee.Name.FirstName,
                employee.Name.LastName,
                employee.Email,
                employee.WorkPhone,
                employee.Department?.Name ?? string.Empty,
                employee.Position?.Title ?? string.Empty,
                employee.Status.ToString(),
                employee.HireDate
            );

            return BaseResult<EmployeeListDto>.Ok(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while fetching employee by Azure AD ID: {AzureAdId}", request.Id);

            return BaseResult<EmployeeListDto>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString(TranslatorMessages.GeneralMessages.Unexpected_Error(ex.Message))
            ));
        }
    }
}
