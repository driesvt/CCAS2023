using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace CCAS.BlazorOldServer.Pages;

[AllowAnonymous]
public class LoginModel : PageModel
{
    public IActionResult OnGet()
    {
        if (!User.Identity!.IsAuthenticated)
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = "/"
            });
        else
            return Redirect("/");        
    }
}
