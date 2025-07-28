using AutoMapper;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Enums;
using HRMS.Domain.Interfaces;
using MediatR;

namespace HRMS.Application.Features.Employees.Commands.CreateEmployee;

public record CreateEmployeeCommand(
    Guid AzureAdId,
    string EmployeeNumber,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    DateTime DateOfBirth,
    Guid DepartmentId,
    Guid PositionId,
    Address Address) : IRequest<EmployeeDto>;

public class CreateEmployeeCommandHandler(
    IEmployeeRepository employeeRepository,
    IAzureAdService azureAdService,
    IMapper mapper,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateEmployeeCommand, EmployeeDto>
{
    public async Task<EmployeeDto> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            if (await azureAdService.VerifyUserExistsAsync(request.AzureAdId))
            {
                // Verify Azure AD user exists
                var name = new PersonName(request.FirstName, request.LastName);
                var email = new Email(request.Email);
                var phone = request.PhoneNumber;
                var address = request.Address;
                var bank = new BankDetails("", "", "");


                var employee = new Employee(
                    request.AzureAdId,
                    request.EmployeeNumber,
                    "",
                    "",
                    name,
                    request.DateOfBirth,
                    Gender.Female,
                    MaritalStatus.Single,
                    email,
                    phone,
                    phone,
                    address,
                    DateTime.UtcNow,
                    EmploymentType.Permanent,
                    true,
                    request.DepartmentId,
                    request.PositionId,
                    "",
                    0,
                    PayFrequency.SemiMonthly,
                    bank);

                await employeeRepository.AddAsync(employee);
                await unitOfWork.CommitTransactionAsync(cancellationToken);
                return mapper.Map<EmployeeDto>(employee);
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}