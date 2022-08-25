using CCAS.BlazorServer.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Serilog;

namespace CCAS.BlazorServer;

public static class DependencyInjection
{
    public static IServiceCollection AddOIDCAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options => {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddOpenIdConnect(o =>
        {
            o.ClientId = configuration["OIDC:ClientID"];
            o.ClientSecret = configuration["OIDC:ClientSecret"]; // for code flow
            o.Authority = configuration["OIDC:Authority"];
            o.UseTokenLifetime = false; //???
            o.CallbackPath = !string.IsNullOrWhiteSpace(configuration["OIDC:SigninCallbackPath"]) ? configuration["OIDC:SigninCallbackPath"] : "/signin-adfs";

            o.ResponseType = OpenIdConnectResponseType.CodeIdToken;
            o.SaveTokens = true;
            o.Resource = configuration["OIDC:ClientID"];
            o.Scope.Add("email");

            o.RequireHttpsMetadata = false;
        }
        );
        return services;
    }

    public static IServiceCollection AddFakeAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication("Fake")
            .AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>("Fake", options => { });

        return services;
    }

    public static WebApplicationBuilder AddCustomLogging(this WebApplicationBuilder builder, IConfiguration configuration, string stack)
    {
        builder.Host.UseSerilog((ctx, lc) => lc
            .Enrich.WithProperty("Service", !String.IsNullOrWhiteSpace(configuration["Seq:Service"]) ? configuration["Seq:Service"] : typeof(DependencyInjection).Namespace)
            .Enrich.WithProperty("Stack", stack)
            .WriteTo.Console()
            .WriteTo.Seq(configuration["Seq:Uri"], apiKey: configuration["Seq:ApiKey"]));

        return builder;
    }
}
