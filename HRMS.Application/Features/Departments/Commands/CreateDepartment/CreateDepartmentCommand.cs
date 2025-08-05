using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Departments.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Wrappers;
using HRMS.Domain.Aggregates.DepartmentAggregate;
using HRMS.Domain.Exceptions;
using HRMS.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRMS.Application.Features.Departments.Commands.CreateDepartment;

public record CreateDepartmentCommand(
    string Name,
    string Code,
    string Description,
    Guid? ManagerId) : IRequest<BaseResult<DepartmentDto>>;

public class CreateDepartmentCommandHandler(
    IDepartmentRepository departmentRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    ILogger<CreateDepartmentCommandHandler> logger)
    : IRequestHandler<CreateDepartmentCommand, BaseResult<DepartmentDto>>
{
    public async Task<BaseResult<DepartmentDto>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Optional business rule: check for unique Code
            var existing = await departmentRepository.GetByCodeAsync(request.Code);
            if (existing is not null)
            {
                return BaseResult<DepartmentDto>.Failure(new Error(
                    ErrorCode.FieldDataInvalid,
                    $"A department with the code '{request.Code}' already exists.",
                    nameof(request.Code)
                ));
            }

            var department = new Department(
                request.Name,
                request.Code,
                request.Description,
                request.ManagerId);

            await departmentRepository.AddAsync(department);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var dto = mapper.Map<DepartmentDto>(department);
            return BaseResult<DepartmentDto>.Ok(dto);
        }
        catch (DomainException dex)
        {
            logger.LogWarning(dex, "Domain validation failed for department creation.");

            return BaseResult<DepartmentDto>.Failure(new Error(
                ErrorCode.FieldDataInvalid,
                dex.Message
            ));
        }
        catch (ArgumentException aex)
        {
            logger.LogWarning(aex, "Invalid input detected during department creation.");

            return BaseResult<DepartmentDto>.Failure(new Error(
                ErrorCode.ModelStateNotValid,
                aex.Message
            ));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred while creating department.");

            return BaseResult<DepartmentDto>.Failure(new Error(
                ErrorCode.Exception,
                "An unexpected error occurred. Please try again later."
            ));
        }

    }
}