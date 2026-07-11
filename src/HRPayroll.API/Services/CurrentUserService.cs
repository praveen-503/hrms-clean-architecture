using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using HRPayroll.Application.Contracts.Identity;

namespace HRPayroll.API.Services;

/// <summary>
/// Service to resolve identity properties from the active HTTP request.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public string? UserId => 
        _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? 
        _httpContextAccessor.HttpContext?.User?.Identity?.Name;
}
