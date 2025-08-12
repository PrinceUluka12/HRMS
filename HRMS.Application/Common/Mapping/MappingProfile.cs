using AutoMapper;
using HRMS.Application.Features.BuddyPair.Dtos;
using HRMS.Application.Features.Departments.Dtos;
using HRMS.Application.Features.Employees.Commands.CreateEmployee;
using HRMS.Application.Features.Employees.Dtos;
using HRMS.Application.Features.Equipments.Dtos;
using HRMS.Application.Features.Leave.Dtos;
using HRMS.Application.Features.Onboarding.Dtos;
using HRMS.Application.Features.Payroll.Dtos;
using HRMS.Application.Features.Positions.Dtos;
using HRMS.Domain.Aggregates.DepartmentAggregate;
using HRMS.Domain.Aggregates.EmployeeAggregate;
using HRMS.Domain.Aggregates.LeaveAggregate;
using HRMS.Domain.Aggregates.OnboardingAggregate;
using HRMS.Domain.Aggregates.PayrollAggregate;
using HRMS.Domain.Aggregates.PositionAggregate;
using HRMS.Domain.Enums;
using OnboardingStageDto = HRMS.Application.Features.Employees.Dtos.OnboardingStageDto;
using OnboardingTaskDto = HRMS.Application.Features.Employees.Dtos.OnboardingTaskDto;

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
        CreateMap<LeaveRequest, LeaveRequestDto>();
        CreateMap<Address,  AddressDto>();
        CreateMap<AddressDto,  Address>();
        CreateMap<BankDetails,  BankDetailsDto>();
        
        CreateMap<BankDetailsDto,  BankDetails>();
        
        CreateMap<OnboardingDocument, OnboardingDocumentDto>();
        CreateMap<OnboardingDocumentDto, OnboardingDocument>();
        
        CreateMap<Equipment, EquipmentDto>();
        CreateMap<EquipmentDto,  Equipment>();
        
        CreateMap<EquipmentAssignmentDto , EquipmentAssignment>();
        CreateMap<EquipmentAssignment, EquipmentAssignmentDto>();

        CreateMap<BuddyCheckInDto, BuddyCheckIn>();
        CreateMap<BuddyCheckIn, BuddyCheckInDto>();
        
        // Map OnboardingStage → OnboardingStageDto
        CreateMap<OnboardingStage, OnboardingStageDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())) // Convert enum to string
            .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks)); // AutoMapper will handle the collection if OnboardingTask → OnboardingTaskDto is defined

        // Map OnboardingTask → OnboardingTaskDto (assuming you have this DTO)
        CreateMap<OnboardingTask, OnboardingTaskDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString())); // If it also has an enum → string mapping
        
        
        CreateMap<CreateEmployeeCommand, Employee>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => EmploymentStatus.Active))
            .ForMember(dest => dest.HireDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}