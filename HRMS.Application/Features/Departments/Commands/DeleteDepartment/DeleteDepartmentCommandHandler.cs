using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.Features.Departments.Commands.DeleteDepartment;

/// <summary>
/// Handles deletion of a department. Note: this does not cascade delete dependent entities; ensure
/// relationships are handled appropriately upstream (e.g., reassign employees before deletion).
/// </summary>
public class DeleteDepartmentCommandHandler(
    IDepartmentRepository departmentRepository,
    IUnitOfWork unitOfWork,
    ILogger<DeleteDepartmentCommandHandler> logger)
    : IRequestHandler<DeleteDepartmentCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(DeleteDepartmentCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var department = await departmentRepository.GetByIdAsync(request.Id);
            if (department is null)
            {
                return BaseResult<Guid>.Failure(new Error(
                    ErrorCode.NotFound,
                    $"Department with ID '{request.Id}' was not found.",
                    nameof(request.Id)
                ));
            }
            await departmentRepository.Delete(department);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return BaseResult<Guid>.Ok(department.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Failed to delete department with ID {DepartmentId}", request.Id);
            return BaseResult<Guid>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while deleting the department."
            ));
        }
    }
}