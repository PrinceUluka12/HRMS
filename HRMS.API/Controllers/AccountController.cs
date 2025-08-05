using System;
using System.Security.Claims;
using System.Threading.Tasks;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(
    IEmployeeRepository employeeRepository,
    ILogger<AccountController> logger)
    : ControllerBase
{
    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<EmployeeProfileDto>> GetProfile()
    {
        var azureAdId = GetAzureAdUserId();
        if (azureAdId == null)
            return Unauthorized("User ID not found or invalid in token.");

        try
        {
            var employee = await employeeRepository.GetByAzureAdIdAsync(azureAdId.Value);
            if (employee == null)
                return NotFound($"Employee with Azure AD ID '{azureAdId}' not found.");

            var profile = new EmployeeProfileDto
            {
                EmployeeId = employee.Id,
                Name = $"{employee.Name.FirstName} {employee.Name.LastName}",
                Email = employee.Email.Value,
                Department = employee.Department?.Name ?? string.Empty,
                Position = employee.Position?.Title ?? string.Empty
            };

            return Ok(profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving profile for Azure AD ID {AzureAdId}", azureAdId);
            return StatusCode(500, "An unexpected error occurred while retrieving the profile.");
        }
    }

    private Guid? GetAzureAdUserId()
    {
        var userIdStr = User.FindFirstValue("http://schemas.microsoft.com/identity/claims/objectidentifier")
                        ?? User.FindFirstValue("oid");

        return Guid.TryParse(userIdStr, out var id) ? id : null;
    }
}

public record EmployeeProfileDto
{
    public Guid EmployeeId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Department { get; init; } = string.Empty;
    public string Position { get; init; } = string.Empty;
}