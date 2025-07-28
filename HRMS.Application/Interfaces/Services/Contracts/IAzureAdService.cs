namespace HRMS.Application.Interfaces.Services.Contracts;

public interface IAzureAdService
{
    Task <bool>VerifyUserExistsAsync(Guid userId);
    Task SyncEmployeesFromAzureAdAsync();
    Task<bool> IsUserInRoleAsync(string userId, string role);
    Task<string?> GetUserEmailAsync(Guid userId);
    Task<string?> GetUserDepartmentAsync(string userId);
}