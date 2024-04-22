using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pet_Reunion_Hub.Helper;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<DeleteModel> _logger;

        //public DeleteModel(PRHDATALIB.Models.DatabaseContext context)
        //{
        //    _context = context;
        //}

        public DeleteModel(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager, ILogger<DeleteModel> logger)
        {

            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
          public Tribute Tribute { get; set; } = default!;

          public List<Comment> Comments { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Tribute == null)
            {
                return NotFound();
            }

            var tribute = await _context.Tribute.Include(p => p.Comments).FirstOrDefaultAsync(m => m.Id == id);

            if (tribute == null)
            {
                return NotFound();
            }
            else
            {
                Tribute = tribute;
            }
           
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            //if (id == null || _context.Tribute == null)
            //{
            //    return NotFound();
            //}
            //var tribute = await _context.Tribute.FindAsync(id);

            //if (tribute != null)
            //{
            //    Tribute = tribute;
            //    _context.Tribute.Remove(Tribute);
            //    await _context.SaveChangesAsync();
            //}

            //return RedirectToPage("./Index");
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

                    if (id == null)
                    {
                        return NotFound();
                    }

                    Tribute = await _context.Tribute.FindAsync(id);

                    if (Tribute == null)
                    {
                        return NotFound();
                    }

                    var comments = await _context.Comment.Where(rp => rp.TributeId == Tribute.Id).ToListAsync();
                    foreach (var comment in comments)
                    {
                        _context.Remove(comment);
                    }

                    var notifications = await _context.TributeNotification.Where(n => n.TributeId == Tribute.Id).ToListAsync();
                    foreach (var notification in notifications)
                    {
                        _context.Remove(notification);
                    }

                    _context.Tribute.Remove(Tribute);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Report deleted successfully.");
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
                _logger.LogError(ex, "Error deleting report:");
                //return RedirectToPage("/Error");
                throw;
            }
        }
    }
    
}
