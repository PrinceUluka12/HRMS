using System;
using System.Security.Claims;
using System.Threading.Tasks;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HRMS.API.Controllers;

[AllowAnonymous]
[ApiController]
[Route("api/[controller]")]
public class AccountController(
    //IAzureAdService azureAdService,
    IEmployeeRepository employeeRepository,
    ILogger<AccountController> logger)
    : ControllerBase
{
    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<EmployeeProfileDto>> GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.Email);    
        if (string.IsNullOrWhiteSpace(userId))
        {
            return Unauthorized("User ID not found in token.");
        }

        try
        {
            var employee = await employeeRepository.GetByAzureAdIdAsync(userId);
            if (employee is null)
            {
                return NotFound($"Employee with Azure AD ID '{userId}' not found.");
            }

            var profile = new EmployeeProfileDto
            {
                EmployeeId = employee.Id,
                Name = $"{employee.Name.FirstName} {employee.Name.LastName}",
                Email = employee.Email.Value,
                Department = employee.Department?.Name,
                Position = employee.Position?.Title
            };

            return Ok(profile);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving profile for user {UserId}", userId);
            return StatusCode(500, "An unexpected error occurred while retrieving the profile.");
        }
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