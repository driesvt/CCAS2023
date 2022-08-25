namespace CCAS.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    string? Email { get; }
    string? FirstName { get; }
    string? LastName { get; }
    string? DisplayName { get; }
    bool IsAuthenticated { get; }

    Task<bool> IsInRoleAsync(string role);

    Task<bool> AuthorizeAsync(string policyName);

    Task<bool> IsInGroupAsync(string group);
}
