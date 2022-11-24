namespace BlazorServer.Shared;

using CCAS.Application.Common.Interfaces;
using Microsoft.AspNetCore.Components;

public partial class MainLayout
{
    [Inject]
    public ICurrentUserService currentUserService { get; set; }

    private bool _drawerOpen { get; set; } = true;

    void DrawerToggle()
    {
        _drawerOpen = !_drawerOpen;
    }
}
