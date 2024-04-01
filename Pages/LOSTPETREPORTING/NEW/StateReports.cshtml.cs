//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
//{
//    public class StateReportsModel : PageModel
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
    public class StateReportsModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public StateReportsModel(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager)
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

            // Extract the country and state from the user's location value
            var parts = userLocationValue?.Split('-');
            var userCountry = parts?.FirstOrDefault();
            var userState = parts?.Length > 1 ? parts?[1] : null;

            // Query reports where the country and state match the user's location
            CreateReport = await _context.CreateReport.Include(r => r.ReportPhotos)
                .Where(r => r.GenLoc.StartsWith($"{userCountry}-{userState}"))
                .ToListAsync();
        }
    }
}
