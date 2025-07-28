using HRMS.Application.Interfaces.Services.Contracts;
using HRMS.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;

namespace HRMS.Application.Interfaces.Services;

/// <summary>
/// Service to interact with Azure Active Directory using Microsoft Graph API.
/// </summary>
public class AzureAdService(GraphServiceClient graphServiceClient, ILogger<AzureAdService> logger)
    : IAzureAdService
{
    private readonly GraphServiceClient _graphServiceClient = graphServiceClient ?? throw new ArgumentNullException(nameof(graphServiceClient));
    private readonly ILogger<AzureAdService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Verifies if a user exists in Azure AD.
    /// </summary>
    public async Task<bool> VerifyUserExistsAsync(Guid userId)
    {
        try
        {
            var user = await _graphServiceClient.Users[userId.ToString()].GetAsync(requestConfig =>
            {
                requestConfig.QueryParameters.Select = new[] { "id" };
            });


            if (user == null)
            {
                throw new AzureAdException($"Azure AD user '{userId}' not found.");
                return false;
            }

            return true;
        }
        
        catch (Microsoft.Graph.Models.ODataErrors.ODataError ex)
        {
            _logger.LogError(ex, "Failed to verify Azure AD user '{UserId}'.", userId);
            throw new AzureAdException($"Failed to verify Azure AD user '{userId}'.", ex);
            return false;
        }
    }

    /// <summary>
    /// Synchronizes employees from Azure AD.
    /// </summary>
    public async Task SyncEmployeesFromAzureAdAsync()
    {
        try
        {
            var users = await _graphServiceClient.Users
                .GetAsync(requestConfig =>
                {
                    requestConfig.QueryParameters.Select = new[] { "id", "givenName", "surname", "mail", "jobTitle", "department" };
                });

            // TODO: Sync with your internal DB
            foreach (var user in users.Value)
            {
                // Process each user
            }
        }
        catch (Microsoft.Graph.Models.ODataErrors.ODataError ex)
        {
            _logger.LogError("Error syncing employees from Azure AD: {Error}", ex.Error?.Message);
            throw new AzureAdException("Error syncing employees from Azure AD.", ex);
        }   
    }

    /// <summary>
    /// Checks whether the user has a specific App Role.
    /// </summary>
    public async Task<bool> IsUserInRoleAsync(string userId, string roleId)
    {
        try
        {
            var assignments = await _graphServiceClient.Users[userId].AppRoleAssignments
                .GetAsync();

            return assignments?.Value?.Any(a => a.AppRoleId?.ToString() == roleId) ?? false;
        }
        catch (Microsoft.Graph.Models.ODataErrors.ODataError ex)
        {
            _logger.LogError("Error checking role '{RoleId}' for user '{UserId}': {Error}", roleId, userId, ex.Error?.Message);
            throw new AzureAdException($"Error checking role '{roleId}' for user '{userId}'.", ex);
        }
    }

    /// <summary>
    /// Retrieves the email address of a user.
    /// </summary>
    public async Task<string?> GetUserEmailAsync(Guid userId)
    {
        try
        {
            var user = await _graphServiceClient.Users[userId.ToString()]
                .GetAsync(requestConfig =>
                {
                    requestConfig.QueryParameters.Select = new[] { "mail" };
                });

            return user?.Mail;
        }
        catch (Microsoft.Graph.Models.ODataErrors.ODataError ex)
        {
            _logger.LogError("Error getting email for user '{UserId}': {Error}", userId, ex.Error?.Message);
            throw new AzureAdException($"Error getting email for user '{userId}'.", ex);
        }
    }

    /// <summary>
    /// Retrieves the department of a user.
    /// </summary>
    public async Task<string?> GetUserDepartmentAsync(string userId)
    {
        try
        {
            var user = await _graphServiceClient.Users[userId]
                .GetAsync(requestConfig =>
                {
                    requestConfig.QueryParameters.Select = new[] { "department" };
                });

            return user?.Department;
        }
        catch (Microsoft.Graph.Models.ODataErrors.ODataError ex)
        {
            _logger.LogError("Error getting department for user '{UserId}': {Error}", userId, ex.Error?.Message);
            throw new AzureAdException($"Error getting department for user '{userId}'.", ex);
        }
    }
}
