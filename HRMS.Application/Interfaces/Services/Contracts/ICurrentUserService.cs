namespace HRMS.Application.Interfaces.Services.Contracts;

public interface ICurrentUserService
{
    string? UserId { get; }
    bool IsInRole(string role);
}