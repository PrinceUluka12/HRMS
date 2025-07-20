namespace HRMS.Application.Interfaces.Services.Contracts;

public interface IAzureAdService
{
    Task VerifyUserExistsAsync(string userId);
    Task SyncEmployeesFromAzureAdAsync();
    Task<bool> IsUserInRoleAsync(string userId, string role);
    Task<string?> GetUserEmailAsync(string userId);
    Task<string?> GetUserDepartmentAsync(string userId);
}