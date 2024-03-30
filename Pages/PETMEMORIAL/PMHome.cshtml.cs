using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL
{
    [Authorize]
    public class PMHomeModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
