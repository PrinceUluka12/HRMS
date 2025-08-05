using HRMS.Application.Common.Interfaces;
using HRMS.Application.Interfaces.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.API.Controllers;

[Authorize(Roles = "HR.Admin")]
[ApiController]
[Route("api/integrations")]
public class IntegrationsController(
    IAzureAdService azureAdService,
    ITeamsIntegrationService teamsService,
    ILogger<IntegrationsController> logger)
    : ControllerBase
{
    [HttpPost("sync-employees")]
    public async Task<IActionResult> SyncEmployeesFromAzureAd()
    {
        try
        {
            await azureAdService.SyncEmployeesFromAzureAdAsync();
            return Ok(new { Message = "Employee sync completed successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during employee sync from Azure AD");
            return StatusCode(500, new { Error = "Failed to sync employees" });
        }
    }

    [HttpPost("teams/notify")]
    public async Task<IActionResult> SendTeamsNotification(
        [FromBody] TeamsNotificationRequest request)
    {
        try
        {
            await teamsService.SendTeamsNotificationAsync(
                request.TeamId,
                request.ChannelId,
                request.Message);
            
            return Ok(new { Message = "Notification sent successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending Teams notification");
            return StatusCode(500, new { Error = "Failed to send notification" });
        }
    }
}

public record TeamsNotificationRequest(
    string TeamId,
    string ChannelId,
    string Message);