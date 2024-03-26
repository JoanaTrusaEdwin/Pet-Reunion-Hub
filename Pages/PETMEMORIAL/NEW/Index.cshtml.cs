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

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
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

        public IList<Tribute> Tribute { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            //if (_context.Tribute != null)
            //{
            //    Tribute = await _context.Tribute
            //    .Include(t => t.User).ToListAsync();
            //}

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Tribute = await _context.Tribute
                   .Where(n => n.UserId == currentUser.Id)
                   .ToListAsync();


            return Page();
        }
    }
}
