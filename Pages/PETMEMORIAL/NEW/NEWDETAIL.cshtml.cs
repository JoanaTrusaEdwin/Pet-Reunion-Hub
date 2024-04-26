using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pet_Reunion_Hub.Helper;
using PRHDATALIB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Pet_Reunion_Hub.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;


namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
{ 

    [Authorize]
    public class NEWDETAILModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotificationService _notificationService;
        private readonly ILogger<CommunityMemorialsModel> _logger;

        public NEWDETAILModel(ILogger<CommunityMemorialsModel> logger, DatabaseContext context, UserManager<IdentityUser> userManager, INotificationService notificationService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        public Tribute Tribute { get; set; } = default!;
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Tribute == null)
            {
                return NotFound();
            }

            var tribute = await _context.Tribute.Include(t => t.Comments).ThenInclude(c => c.User).FirstOrDefaultAsync(m => m.Id == id);
            if (tribute == null)
            {
                return NotFound();
            }
            Tribute = tribute;
            Comments = tribute.Comments.ToList(); // Populate comments
            return Page();
        }

        public IActionResult OnPost(int tributeId, string commentContent)
        {
            var userId = _userManager.GetUserId(User);

            // Find the tribute to which the comment belongs
            var tribute = _context.Tribute.Include(t => t.User).FirstOrDefault(t => t.Id == tributeId);

            if (tribute != null)
            {
                // Create a new comment
                var newComment = new Comment
                {
                    Content = commentContent,
                    UserId = userId,
                    TributeId = tributeId
                };

                // Add the comment to the database
                _context.Comment.Add(newComment);
                _context.SaveChanges();

                if (tribute.User.Id != userId)
                {
                    var commenter = _context.Users.FirstOrDefault(u => u.Id == newComment.UserId);
                    // Add a notification for the tribute owner
                    var notification = new NEWNOTIFICATION
                    {
                        UserId = tribute.UserId,
                        Content = $"You have a new comment on your tribute '{tribute.PetName}' by {commenter.UserName}",
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    };

                    _context.NEWNOTIFICATION.Add(notification);
                    _context.SaveChanges();
                }
            }

            // Redirect back to the page
            return RedirectToPage(new { id = tributeId });
        }

        public async Task<IActionResult> OnPostRemoveCommentAsync(int commentId)
        {
            try
            {
                var commentToRemove = await _context.Comment.FindAsync(commentId);
                if (commentToRemove != null)
                {
                    _context.Comment.Remove(commentToRemove);
                    await _context.SaveChangesAsync();
                    return new JsonResult(new { success = true }); // Return success response
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing comment:");
                return new JsonResult(new { success = false, error = ex.Message }); // Return error response
            }
        }

    }
}
