using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pet_Reunion_Hub.Helper;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.COPINGRESOURCES
{
    [Authorize]
    public class BooksModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BooksModel(DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<RESOURCE> RESOURCE { get; set; }


        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);

            // Query for public tributes
            RESOURCE = _context.RESOURCE
                .Where(t => t.TYPE == "Book")
                .OrderByDescending(t => t.Id)
                .ToList();

            Console.WriteLine($"{RESOURCE.Count}");

            return Page();
        }

        public IActionResult OnPost(int resourceId)
        {
            var userId = _userManager.GetUserId(User);

            // Find the tribute to which the comment belongs
            var resource = _context.RESOURCE.Include(t => t.User).FirstOrDefault(t => t.Id == resourceId);

            if (resource != null)
            {
                var RESOURCEFAVE = new RESOURCEFAVE
                {
                    UserId = userId,
                    RESOURCEId = resourceId
                };

                // Add the comment to the database
                _context.RESOURCEFAVE.Add(RESOURCEFAVE);
                _context.SaveChanges();
  
            }

            // Redirect back to the page
            return RedirectToPage(new { id = resourceId });
        }
    }
}

