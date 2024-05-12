using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages
{
    //[Authorize(Policy = "RequireLoggedIn")]
    [Authorize]
    public class HomePageModel : PageModel
    {
        //public bool IsUserAuthenticated { get; private set; }
        //public void OnGet()
        //{
        //    IsUserAuthenticated = User.Identity.IsAuthenticated;
        //}

        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomePageModel(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
       

        public IList<CreateReport>? CreateReport { get; set; }
        public GENERALLOCATION? GENERALLOCATION { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get the current user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                // Handle case where user is not found
                return RedirectToPage("/Identity/Account/Login");
            }

            // Retrieve the LOCATIONVALUE of the current user
            var userLocationValue = await _context.GENERALLOCATION
                .Where(gl => gl.UserId == currentUser.Id)
                .FirstOrDefaultAsync();
            if (userLocationValue == null)
            {
                // Redirect the user to a location setting page
                return RedirectToPage("/LOSTPETREPORTING/GENERAL/Create");
            }
            // Set the GENERALLOCATION property with the retrieved value
            GENERALLOCATION = userLocationValue;

            // Query reports where GenLoc matches the user's LOCATIONVALUE
            CreateReport = await _context.CreateReport
                .Include(r => r.ReportPhotos)
                .Where(r => r.GenLoc == userLocationValue.LOCATIONVALUE)
                .ToListAsync();

            return Page();
        }

    }
}
