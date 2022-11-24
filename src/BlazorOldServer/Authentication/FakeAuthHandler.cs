using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace CCAS.BlazorOldServer.Authentication;

public class FakeAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public FakeAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, "DOM\\JoeBlogs"),
            new Claim(ClaimTypes.Email, "joe@test.test"),
            new Claim(ClaimTypes.GivenName, "Joe"),
            new Claim(ClaimTypes.Surname, "Blogs"),
            new Claim("http://schemas.xmlsoap.org/claims/Group", "aql-staff"),
            new Claim("http://schemas.xmlsoap.org/claims/Group", "cia-staff"),
            new Claim("http://schemas.xmlsoap.org/claims/Group", "cia-svisor"),
        };
        var identity = new ClaimsIdentity(claims, "Fake");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "Fake");

        var result = AuthenticateResult.Success(ticket);

        return Task.FromResult(result);
    }
}
