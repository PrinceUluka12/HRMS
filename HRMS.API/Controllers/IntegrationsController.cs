using HRMS.Application.Common.Interfaces;
using HRMS.Application.Interfaces.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Authorize(Roles = "HR.Admin")]
[ApiController]
[Route("api/integrations")]
public class IntegrationsController : ControllerBase
{
    private readonly IAzureAdService _azureAdService;
    private readonly ITeamsIntegrationService _teamsService;
    private readonly ILogger<IntegrationsController> _logger;

    public IntegrationsController(
        IAzureAdService azureAdService,
        ITeamsIntegrationService teamsService,
        ILogger<IntegrationsController> logger)
    {
        _azureAdService = azureAdService;
        _teamsService = teamsService;
        _logger = logger;
    }

    [HttpPost("sync-employees")]
    public async Task<IActionResult> SyncEmployeesFromAzureAd()
    {
        try
        {
            await _azureAdService.SyncEmployeesFromAzureAdAsync();
            return Ok(new { Message = "Employee sync completed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during employee sync from Azure AD");
            return StatusCode(500, new { Error = "Failed to sync employees" });
        }
    }

    [HttpPost("teams/notify")]
    public async Task<IActionResult> SendTeamsNotification(
        [FromBody] TeamsNotificationRequest request)
    {
        try
        {
            await _teamsService.SendTeamsNotificationAsync(
                request.TeamId,
                request.ChannelId,
                request.Message);
            
            return Ok(new { Message = "Notification sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Teams notification");
            return StatusCode(500, new { Error = "Failed to send notification" });
        }
    }
}

public record TeamsNotificationRequest(
    string TeamId,
    string ChannelId,
    string Message);