using Microsoft.AspNetCore.Authorization;

namespace CCAS.BlazorServer.Authorization;

public class GroupMembershipRequirement : IAuthorizationRequirement
{
    public List<string> Groups { get; }

    public GroupMembershipRequirement(params string[] groups)
    {
        Groups = new List<string>();
        Groups.AddRange(groups);
    }

    public GroupMembershipRequirement(IEnumerable<string> groups)
    {
        Groups = new List<string>();
        Groups.AddRange(groups);
    }

}

public class GroupMembershipHandler : AuthorizationHandler<GroupMembershipRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupMembershipRequirement requirement)
    {
        // Is the user in any of these groups?
        var inAllGroups = requirement.Groups.Aggregate(false, (working, current) => 
            working || 
            (
                context.User.HasClaim("http://schemas.xmlsoap.org/claims/Group", current) ||
                context.User.HasClaim("group", current)
            ));

        if (inAllGroups)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
