using Syncfusion.Blazor.Notifications;

namespace CCAS.BlazorServer.Services;

public class Toast : SfToast, IToast
{
    public Task<bool> Add(string message, string title)
    {
        throw new NotImplementedException();
    }
}
