//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.USER
//{
//    public class IndexModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public IndexModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        public IList<Newtest> Newtest { get;set; } = default!;

//        public async Task OnGetAsync()
//        {
//            if (_context.NEWTEST != null)
//            {
//                Newtest = await _context.NEWTEST
//                .Include(n => n.User).ToListAsync();
//            }
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PRHDATALIB.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.USER
{
    [Authorize] // Restrict access to authenticated users
    public class IndexModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Newtest> Newtest { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Filter data to include only records associated with the current user
            Newtest = _context.NEWTEST.Where(n => n.UserId == currentUser.Id).ToList();

            return Page();
        }
    }
}

