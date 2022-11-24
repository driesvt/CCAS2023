using MudBlazor;

namespace CCAS.BlazorOldServer.Services;

public class AppDialogService : IAppDialogService
{
    private readonly IDialogService dialogService;
    private readonly ISnackbar _snackbar;

    public AppDialogService(IDialogService dialogService, ISnackbar snackbar)
    {
        this.dialogService = dialogService;
        _snackbar = snackbar;
    }

    public async Task<bool> Confirm(string message, string title, string okButton = "Yes", string cancelButton = "No")
    {
        bool? result = await dialogService.ShowMessageBox(
            title,
            message,
            yesText: okButton, cancelText: cancelButton);

        return result == null ? false : (bool)result;
    }

    public Task Error(string message, string title)
    {
        _snackbar.Add(message, Severity.Warning);
        return Task.CompletedTask;
    }
}
