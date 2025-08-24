namespace HRMS.Application.Interfaces.Services.Dtos;

public class RegisterResponseDto
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public bool EmailConfirmed { get; set; }
    public string Status { get; set; }
    public DateTime RegisteredDate { get; set; }
}