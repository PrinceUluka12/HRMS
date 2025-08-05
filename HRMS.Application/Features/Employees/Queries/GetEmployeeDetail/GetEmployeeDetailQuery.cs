using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Helpers;
using HRMS.Application.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Employees.Queries.GetEmployeeDetail;

public record GetEmployeeDetailQuery(Guid Id) : IRequest<BaseResult<Employee>>;

public class GetEmployeeDetailQueryHandler(
    IEmployeeRepository employeeRepository,
    ITranslator translator,
    ILogger<GetEmployeeDetailQueryHandler> logger)
    : IRequestHandler<GetEmployeeDetailQuery, BaseResult<Employee>>
{
    public async Task<BaseResult<Employee>> Handle(GetEmployeeDetailQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employee = await employeeRepository.GetByIdAsync(request.Id, cancellationToken);

            if (employee is null)
            {
                return BaseResult<Employee>.Failure(new Error(
                    ErrorCode.NotFound,
                    translator.GetString(
                        TranslatorMessages.EmployeeMessages.Employee_NotFound_with_id(request.Id)),
                    nameof(request.Id)
                ));
            }

            return BaseResult<Employee>.Ok(employee);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while retrieving employee with ID {EmployeeId}", request.Id);

            return BaseResult<Employee>.Failure(new Error(
                ErrorCode.Exception,
                translator.GetString(TranslatorMessages.GeneralMessages.Unexpected_Error(ex.Message))
            ));
        }
    }
}