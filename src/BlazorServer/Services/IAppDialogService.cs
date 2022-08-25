namespace CCAS.BlazorServer.Services;

public interface IAppDialogService
{
    Task<bool> Confirm(string message, string title, string okButton = "Yes", string cancelButton = "No");
    Task Error(string message, string title);
}
