using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pet_Reunion_Hub.Pages
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class LOGOUTPAGEModel : PageModel
    {
        public bool IsUserAuthenticated { get; private set; }
        public void OnGet()
        {
            IsUserAuthenticated = User.Identity.IsAuthenticated;
        }
    }
}
