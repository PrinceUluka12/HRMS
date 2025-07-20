using System.Security.Claims;
using HRMS.Application.Common.Interfaces;
using HRMS.Application.Interfaces.Services.Contracts;
using Microsoft.AspNetCore.Http;

namespace HRMS.Application.Interfaces.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?
        .FindFirstValue(ClaimTypes.NameIdentifier);

    public string? Email => _httpContextAccessor.HttpContext?.User?
        .FindFirstValue(ClaimTypes.Email);

    public bool IsInRole(string role) => _httpContextAccessor.HttpContext?.User?
        .IsInRole(role) ?? false;

    public IEnumerable<string> GetRoles()
    {
        return _httpContextAccessor.HttpContext?.User?
            .FindAll(ClaimTypes.Role)
            .Select(c => c.Value) ?? Enumerable.Empty<string>();
    }

    public bool IsEmployee(Guid employeeId)
    {
        var userId = UserId;
        if (userId == null) return false;

        // In a real implementation, you would check against employee records
        return _httpContextAccessor.HttpContext?.User?
            .HasClaim("EmployeeId", employeeId.ToString()) ?? false;
    }
}