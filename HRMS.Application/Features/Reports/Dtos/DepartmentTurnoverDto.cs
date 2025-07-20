namespace HRMS.Application.Features.Reports.Dtos;

public record DepartmentTurnoverDto(
    Guid DepartmentId,
    string DepartmentName,
    int TotalEmployees,
    int NewHires,
    int Terminations,
    decimal TurnoverRate);