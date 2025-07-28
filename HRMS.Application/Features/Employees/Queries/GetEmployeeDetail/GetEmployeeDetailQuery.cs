using AutoMapper;
using HRMS.Application.Common.Exceptions;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Interfaces.Repositories;
using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using MediatR;

namespace HRMS.Application.Features.Employees.Queries.GetEmployeeDetail;

public record GetEmployeeDetailQuery(Guid Id) : IRequest<Employee>;

public class GetEmployeeDetailQueryHandler(
    IEmployeeRepository employeeRepository,
    IMapper mapper,
    ICurrentUserService currentUserService)
    : IRequestHandler<GetEmployeeDetailQuery, Employee>
{
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentUserService _currentUserService = currentUserService;

    public async Task<Employee> Handle(GetEmployeeDetailQuery request, CancellationToken cancellationToken)
    {
        var employee = await employeeRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (employee == null)
        {
            throw new NotFoundException(nameof(Employee), request.Id);
        }

        /*// Check authorization
        if (!_currentUserService.IsInRole("HR.Admin") && 
            employee.AzureAdId != _currentUserService.UserId)
        {
            throw new ForbiddenAccessException();
        }*/

        //return _mapper.Map<EmployeeDetailDto>(employee);
        return employee;
    }
}