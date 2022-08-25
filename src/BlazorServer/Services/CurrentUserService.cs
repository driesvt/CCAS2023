using CCAS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CCAS.BlazorServer.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAuthorizationService _authorizationService;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor, IAuthorizationService authorizationService)
    {
        _httpContextAccessor = httpContextAccessor;
        _authorizationService = authorizationService;
    }

    public string? UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    public string? UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
    public string? Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    public string? FirstName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.GivenName);
    public string? LastName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Surname);

    public string? DisplayName => $"{FirstName} {LastName}";

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

    public async Task<bool> AuthorizeAsync(string policyName)
    {       
        var principal = _httpContextAccessor.HttpContext?.User;
        if (principal == null)
            return false;
        
        var result = await _authorizationService.AuthorizeAsync(principal, policyName);

        return result.Succeeded;

    }

    public Task<bool> IsInRoleAsync(string role)
    {
        var principal = _httpContextAccessor.HttpContext?.User;
        if (principal == null)
            return Task.FromResult(false);

        return Task.FromResult(principal.IsInRole(role)); //ClaimTypes.Role
    }

    public Task<bool> IsInGroupAsync(string group)
    {
        var principal = _httpContextAccessor.HttpContext?.User;
        if (principal == null)
            return Task.FromResult(false);

        return Task.FromResult(principal.HasClaim("http://schemas.xmlsoap.org/claims/Group", group));     }
}
