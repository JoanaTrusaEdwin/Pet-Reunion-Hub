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
using System.Collections.Generic;
using System.Linq;


namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
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

        public IList<CreateReport> CreateReport { get;set; } 
           // = default!;

        
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

            CreateReport = await _context.CreateReport
                   .Where(n => n.UserId == currentUser.Id)
                   .ToListAsync();


            return Page();
        }
    }
}
