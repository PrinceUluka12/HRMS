using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Interfaces.Repositories;
using MediatR;

namespace HRMS.Application.Features.Employees.Queries.GetEmployeeByAzureId;

public record GetEmployeeByAzureIdQuery (Guid Id):  IRequest<EmployeeListDto>;

public class GetEmployeeByAzureIdQueryHandler(IEmployeeRepository employeeRepository) 
    : IRequestHandler<GetEmployeeByAzureIdQuery,EmployeeListDto>
{
    public async Task<EmployeeListDto> Handle(GetEmployeeByAzureIdQuery request, CancellationToken cancellationToken)
    {
        try
        { 
            var employee = await employeeRepository.GetByAzureAdIdAsync(request.Id);
            if (employee != null)
            {
                var data = new EmployeeListDto(
                    employee.Id,
                    employee.EmployeeNumber,
                    employee.Name.FirstName,
                    employee.Name.LastName,
                    employee.Email,
                    employee.WorkPhone, 
                    employee.Department.Name,
                    employee.Position.Title ,
                    employee.Status.ToString(),
                    employee.HireDate
                );
                return data;
            }
            else
            {
                return null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}