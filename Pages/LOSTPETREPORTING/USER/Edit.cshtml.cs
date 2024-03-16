//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.USER
//{
//    public class EditModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public EditModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//        public Newtest Newtest { get; set; } = default!;

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null || _context.NEWTEST == null)
//            {
//                return NotFound();
//            }

//            var newtest =  await _context.NEWTEST.FirstOrDefaultAsync(m => m.Id == id);
//            if (newtest == null)
//            {
//                return NotFound();
//            }
//            Newtest = newtest;
//           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
//            return Page();
//        }

//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see https://aka.ms/RazorPagesCRUD.
//        public async Task<IActionResult> OnPostAsync()
//        {
//            if (!ModelState.IsValid)
//            {
//                return Page();
//            }

//            _context.Attach(Newtest).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!NewtestExists(Newtest.Id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return RedirectToPage("./Index");
//        }

//        private bool NewtestExists(int id)
//        {
//          return (_context.NEWTEST?.Any(e => e.Id == id)).GetValueOrDefault();
//        }
//    }
//}

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRHDATALIB.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.USER
{
    
    public class EditModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<EditModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public EditModel(ILogger<EditModel> logger, PRHDATALIB.Models.DatabaseContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _signInManager = signInManager;
            _userManager = userManager;
        }

        

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Newtest = await _context.NEWTEST.FirstOrDefaultAsync(m => m.Id == id);
            Newtest = await _context.NEWTEST.FindAsync(id);
            if (Newtest == null)
            {
                return NotFound();
            }

            return Page();
        }

        [BindProperty]
        public Newtest Newtest { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    string userId = user.Id;
                    _logger.LogInformation("User ID: {UserId}", userId);

                    if (string.IsNullOrEmpty(userId))
                    {
                        _logger.LogWarning("User ID is null or empty.");
                        return RedirectToPage("/Account/Login");
                    }

                    Newtest.UserId = userId;
                    _logger.LogInformation("Assigned User ID: {UserId}", Newtest.UserId);

                    // Attach the Newtest entity to the context and mark it as modified
                    _context.Attach(Newtest).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("New test edited successfully.");
                    return RedirectToPage("./Index");
                }
                else
                {
                    _logger.LogError("User not found.");
                    return RedirectToPage("/Account/Login");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error editing test:");
                return RedirectToPage("/Error");
            }
        }
    }
}
