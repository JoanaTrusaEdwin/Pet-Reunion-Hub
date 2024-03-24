//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.GENERAL_LOCATION
//{

//    public class IndexModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public IndexModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        public IList<GENERALLOCATION> GENERALLOCATION { get;set; } = default!;

//        public async Task OnGetAsync()
//        {
//            if (_context.GENERALLOCATION != null)
//            {
//                GENERALLOCATION = await _context.GENERALLOCATION
//                .Include(g => g.User).ToListAsync();
//            }
//        }
//    }
//}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.GENERAL_LOCATION
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<GENERALLOCATION> GENERALLOCATION { get; set; } = default!;

        [Authorize]
        public async Task<IActionResult> OnGetAsync()
        {
            //if (_context.CreateReport != null)
            //{
            //    CreateReport = await _context.CreateReport.ToListAsync();
            //}
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Filter data to include only records associated with the current user
            GENERALLOCATION = _context.GENERALLOCATION.Where(n => n.UserId == currentUser.Id).ToList();

            return Page();
        }
    }
}

