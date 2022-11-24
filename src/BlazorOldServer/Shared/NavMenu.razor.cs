using CCAS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Components;

namespace CCAS.BlazorOldServer.Shared;

public partial class NavMenu : ComponentBase
{
    [Inject]
    public ICurrentUserService? currentUserService { get; set; }

    public bool IsAdministrator { get; set; }

    protected override async Task OnInitializedAsync()
    {
        IsAdministrator = await currentUserService!.AuthorizeAsync("Administrator");

        await base.OnInitializedAsync();

    }
}
