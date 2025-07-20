using AutoMapper;
using HRMS.Application.Features.Departments.Dtos;
using HRMS.Application.Features.Employees.Commands.CreateEmployee;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Features.Payroll.Dtos;
using HRMS.Application.Features.Positions.Dtos;
using HRMS.Domain.Aggregates.DepartmentAggregate;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Aggregates.PayrollAggregate;
using HRMS.Domain.Aggregates.PositionAggregate;
using HRMS.Domain.Enums;

namespace HRMS.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Employee, EmployeeDto>();
        CreateMap<Employee, EmployeeDetailDto>();
        CreateMap<Payroll, PayrollDto>();
        CreateMap<Department, DepartmentDto>();
        CreateMap<Position, PositionDto>();
        
        CreateMap<CreateEmployeeCommand, Employee>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => EmploymentStatus.Active))
            .ForMember(dest => dest.HireDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}