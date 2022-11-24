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

            options.AddPolicy("Supervisor", policy =>
                policy.AddRequirements(new GroupMembershipRequirement("cia-svisor"))
            );

            options.AddPolicy("Staff", policy =>
                policy.AddRequirements(new GroupMembershipRequirement("cia-staff"))
            );

            options.AddPolicy("Student", policy =>
                policy.AddRequirements(new GroupMembershipRequirement("cia-student"))
            );

            options.AddPolicy("Guest", policy =>
                policy.AddRequirements(new GroupMembershipRequirement("cia-guest"))
            );

            options.AddPolicy("math111", policy =>
                policy.AddRequirements(new GroupMembershipRequirement("cia-math111"))
            );

            options.AddPolicy("phse111", policy =>
                policy.AddRequirements(new GroupMembershipRequirement("cia-phse111"))
            );

            options.AddPolicy("ecnm111", policy =>
                policy.AddRequirements(new GroupMembershipRequirement("cia-ecnm111"))
            );
        });

        services.AddSingleton<IAuthorizationHandler, GroupMembershipHandler>();
        return services;
    }
}
