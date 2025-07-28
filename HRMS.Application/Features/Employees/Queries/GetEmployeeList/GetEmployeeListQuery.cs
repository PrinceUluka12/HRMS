using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Interfaces.Repositories;
using MediatR;

namespace HRMS.Application.Features.Employees.Queries.GetEmployeeList;

public record GetEmployeeListQuery : IRequest<List<EmployeeListDto>>;

public class GetEmployeeListQueryHandler(IEmployeeRepository employeeRepository)
    : IRequestHandler<GetEmployeeListQuery, List<EmployeeListDto>>
{
    public async Task<List<EmployeeListDto>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var employees = await employeeRepository.GetListWithDepAndPosAsync(cancellationToken);
            if (employees.Count != 0)
            {
                var data = employees.Select(emp => new EmployeeListDto(
                    emp.Id,
                    emp.EmployeeNumber,
                    emp.Name.FirstName,
                    emp.Name.LastName,
                    emp.JobTitle,
                    emp.WorkPhone,
                    emp.Department.Name,
                    emp.Position.Title,
                    emp.Status.ToString(),
                    emp.HireDate
                )).ToList();
                return data;
            }
            else
            {
                return new List<EmployeeListDto>(); 
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new List<EmployeeListDto>(); 
        }
    }
}