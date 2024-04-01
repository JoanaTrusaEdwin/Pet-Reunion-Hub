//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
//{
//    public class CountryReportsModel : PageModel
//    {
//        public void OnGet()
//        {
//        }
//    }
//}

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
{
    [Authorize]
    public class CountryReportsModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CountryReportsModel(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<CreateReport> CreateReport { get; set; }
        public GENERALLOCATION GENERALLOCATION { get; set; }

        public async Task OnGetAsync()
        {
            // Get the current user
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                // Handle case where user is not found
                return;
            }

            // Retrieve the LOCATIONVALUE of the current user
            var userLocationValue = await _context.GENERALLOCATION
                .Where(gl => gl.UserId == currentUser.Id)
                .Select(gl => gl.LOCATIONVALUE)
                .FirstOrDefaultAsync();

            // Extract the country from the user's location value
            var userCountry = userLocationValue?.Split('-').FirstOrDefault();

            // Query reports where the country matches the user's country
            CreateReport = await _context.CreateReport.Include(r => r.ReportPhotos)
                .Where(r => r.GenLoc.StartsWith(userCountry))
                .ToListAsync();

            
        }
    }
}
