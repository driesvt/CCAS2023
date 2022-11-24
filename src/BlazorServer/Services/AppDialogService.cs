using Syncfusion.Blazor.Notifications;
using Syncfusion.Blazor.Popups;

namespace CCAS.BlazorServer.Services;

public class AppDialogService : IAppDialogService
{
    private readonly SfDialogService dialogService;
    private readonly IToast toast;

    public AppDialogService(SfDialogService dialogService, IToast toast)
    {
        this.dialogService = dialogService;
        this.toast = toast;
    }

    public async Task<bool> Confirm(string message, string title)
    {
        bool? result = await dialogService.ConfirmAsync(
            message, title
            );

        return result == null ? false : (bool)result;
    }

    public Task Error(string message, string title)
    {
        toast.Add(message, title);
        return Task.CompletedTask;
    }
}
