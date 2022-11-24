namespace CCAS.BlazorServer.Services;

public interface IToast
{
    Task<bool> Add(string message, string title);
}
