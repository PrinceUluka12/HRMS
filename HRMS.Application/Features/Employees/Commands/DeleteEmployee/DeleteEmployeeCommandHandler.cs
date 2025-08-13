using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.Features.Employees.Commands.DeleteEmployee;

/// <summary>
/// Handles deletion of an employee aggregate.
/// </summary>
public class DeleteEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteEmployeeCommandHandler> logger)
    : IRequestHandler<DeleteEmployeeCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var employee = await employeeRepository.GetByIdAsync(request.Id);
            if (employee is null)
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"Employee with ID '{request.Id}' was not found.",
                    nameof(request.Id)
                ));
            }

            await employeeRepository.Delete(employee);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return BaseResult<Guid>.Ok(employee.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Failed to delete employee with ID {EmployeeId}", request.Id);
            return BaseResult<Guid>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while deleting the employee."
            ));
        }
    }
}