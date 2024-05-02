using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW;

using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages
{
    public class NEWNOTIFICATIONModel : PageModel
    {
        public List<NEWNOTIFICATION> Notifications { get; set; }
        private readonly DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        
        private readonly ILogger<CommunityMemorialsModel> _logger;

        public NEWNOTIFICATIONModel(ILogger<CommunityMemorialsModel> logger, DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            
        }
      
        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            Notifications = await _context.NEWNOTIFICATION
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            if (Notifications == null)
            {
                Notifications = new List<NEWNOTIFICATION>(); // Initialize Notifications if null
            }
            return Page();
        }

        //public async Task<IActionResult> OnPostMarkAsReadAsync(int notificationId)
        //{
        //    var notification = await _context.NEWNOTIFICATION.FindAsync(notificationId);
        //    if (notification != null)
        //    {
        //        notification.IsRead = true;
        //        await _context.SaveChangesAsync();
        //        return RedirectToPage();
        //    }
        //    return NotFound();
        //}


    }
}
