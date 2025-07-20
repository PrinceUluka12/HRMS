namespace HRMS.Application.Features.Departments.Dtos;

public class DepartmentDto
{
    public DepartmentDto() { }
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public Guid? ManagerId { get; set; }
    public string Description { get; set; }
}
    