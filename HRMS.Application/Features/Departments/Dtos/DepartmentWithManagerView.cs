namespace HRMS.Application.Features.Departments.Dtos;

public class DepartmentWithManagerView
{
    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public string DepartmentCode { get; set; }
    public string DepartmentDescription { get; set; }
    public string? ManagerFullName { get; set; }
}