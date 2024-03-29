//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
//{
//    public class DeleteModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public DeleteModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//      public CreateReport CreateReport { get; set; } = default!;

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null || _context.CreateReport == null)
//            {
//                return NotFound();
//            }

//            var createreport = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == id);

//            if (createreport == null)
//            {
//                return NotFound();
//            }
//            else 
//            {
//                CreateReport = createreport;
//            }
//            return Page();
//        }

//        public async Task<IActionResult> OnPostAsync(int? id)
//        {
//            if (id == null || _context.CreateReport == null)
//            {
//                return NotFound();
//            }
//            var createreport = await _context.CreateReport.FindAsync(id);

//            if (createreport != null)
//            {
//                CreateReport = createreport;
//                _context.CreateReport.Remove(CreateReport);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToPage("./Index");
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
{
    public class DeleteModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(DatabaseContext context, UserManager<IdentityUser> userManager, ILogger<DeleteModel> logger)
        {
            _context = context;
;            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public CreateReport CreateReport { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateReport = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == id);

            if (CreateReport == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
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

                    CreateReport = await _context.CreateReport.FindAsync(id);

                    if (CreateReport == null)
                    {
                        return NotFound();
                    }

                    var reportPhotos = await _context.ReportPhoto.Where(rp => rp.ReportId == CreateReport.Id).ToListAsync();
                    foreach (var reportPhoto in reportPhotos)
                    {
                        _context.ReportPhoto.Remove(reportPhoto);
                    }

                    _context.CreateReport.Remove(CreateReport);
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

