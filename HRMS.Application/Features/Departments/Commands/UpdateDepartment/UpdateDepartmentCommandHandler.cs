using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HRMS.Application.Features.Departments.Commands.UpdateDepartment;

/// <summary>
/// Handles updates to department entities.
/// </summary>
public class UpdateDepartmentCommandHandler(
    IDepartmentRepository departmentRepository,
    IUnitOfWork unitOfWork,
    ILogger<UpdateDepartmentCommandHandler> logger)
    : IRequestHandler<UpdateDepartmentCommand, BaseResult<Guid>>
{
    public async Task<BaseResult<Guid>> Handle(UpdateDepartmentCommand request, CancellationToken cancellationToken)
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

            // update details
            department.UpdateDetails(request.Name, request.Code, request.Description);
            if (request.ManagerId.HasValue)
            {
                department.AssignManager(request.ManagerId.Value);
            }

            await departmentRepository.Update(department);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return BaseResult<Guid>.Ok(department.Id);
        }
        catch (Exception ex)
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            logger.LogError(ex, "Failed to update department with ID {DepartmentId}", request.Id);
            return BaseResult<Guid>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred while updating the department."
            ));
        }
    }
}