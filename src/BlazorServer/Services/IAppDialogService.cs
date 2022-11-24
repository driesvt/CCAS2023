namespace CCAS.BlazorServer.Services;

public interface IAppDialogService
{
    Task<bool> Confirm(string message, string title);
    //Task Error(string message, string title);
}
