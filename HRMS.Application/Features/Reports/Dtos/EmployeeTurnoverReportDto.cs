namespace HRMS.Application.Features.Reports.Dtos;

public record EmployeeTurnoverReportDto(
    int Year,
    int TotalEmployees,
    int NewHires,
    int Terminations,
    decimal TurnoverRate,
    List<DepartmentTurnoverDto> DepartmentBreakdown);