using Microsoft.AspNetCore.Authorization;

namespace CCAS.BlazorServer.Authorization;

public static class AuthorizationDependancyInjection
{
    public static IServiceCollection AddApplicationAuthorization(this IServiceCollection services)
    {

        services.AddAuthorization(options =>
        {
            options.AddPolicy("Administrator", policy =>
                policy.AddRequirements(new GroupMembershipRequirement("aql-wifi", "aql-staff"))
            );
        });

        services.AddSingleton<IAuthorizationHandler, GroupMembershipHandler>();
        return services;
    }
}
