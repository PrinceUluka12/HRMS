using System.Security.Claims;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Interfaces.Services.Contracts;
using Microsoft.AspNetCore.Http;

namespace HRMS.Application.Interfaces.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => User?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Email => User?.FindFirstValue(ClaimTypes.Email);

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;

    public IEnumerable<string> GetRoles() =>
        User?.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? Enumerable.Empty<string>();

    public bool IsEmployee(Guid employeeId)
    {
        if (UserId == null) return false;

        // Match against a custom "EmployeeId" claim if available
        var employeeClaim = User?.FindFirst("EmployeeId")?.Value;
        return Guid.TryParse(employeeClaim, out var claimId) && claimId == employeeId;
    }

    public Guid? GetEmployeeId()
    {
        var claimValue = User?.FindFirst("EmployeeId")?.Value;
        return Guid.TryParse(claimValue, out var guid) ? guid : null;
    }

    public string? FullName =>
        User?.FindFirst("FullName")?.Value ?? // custom claim
        $"{User?.FindFirst("given_name")?.Value} {User?.FindFirst("family_name")?.Value}".Trim();
}