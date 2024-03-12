using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pet_Reunion_Hub.Pages
{
    [Authorize(Policy = "RequireLoggedIn")]
    public class HomePageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
