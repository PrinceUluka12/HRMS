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
    string AzureAdId,
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
        // Verify Azure AD user exists
        await azureAdService.VerifyUserExistsAsync(request.AzureAdId);
        
        var name = new PersonName(request.FirstName, request.LastName);
        var email = new Email(request.Email);
        var phone  =  new PhoneNumber("",request.PhoneNumber);
        var address = new Address("", "", "", "", "");
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
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return mapper.Map<EmployeeDto>(employee);
    }
}