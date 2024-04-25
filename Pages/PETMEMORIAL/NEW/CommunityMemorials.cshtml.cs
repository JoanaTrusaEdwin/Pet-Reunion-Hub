


using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
//using Pet_Reunion_Hub.Helper;
using PRHDATALIB.Models;
using Pet_Reunion_Hub.Services;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
{
    [Authorize]
    public class CommunityMemorialsModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotificationService _notificationService;
        private readonly ILogger<CommunityMemorialsModel> _logger;

        public CommunityMemorialsModel(ILogger<CommunityMemorialsModel> logger,DatabaseContext context, UserManager<IdentityUser> userManager, INotificationService notificationService)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        public List<Tribute> PublicTributes { get; set; }
        public List<Post> PublicPosts { get; set; }


        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);

            // Query for public tributes
            //PublicTributes = _context.Tribute
            //    .Where(t => t.Visibility == "Public")
            //    .Include(t => t.Comments)
            //    .ThenInclude(c => c.User)
            //    .OrderByDescending(t => t.Id)
            //    .ToList();
            PublicTributes = _context.Tribute
                    .Where(t => t.Visibility == "Public")
                    .Include(t => t.User) // Include the User navigation property
                    .Include(t => t.Comments)
                    .ThenInclude(c => c.User)
                    .OrderByDescending(t => t.Id)
                    .ToList();

            PublicPosts = _context.Post
               .Where(p => p.IsPublic == "Public") // Filter by IsPublic column
               .Include(t => t.Media)
               .OrderByDescending(p => p.CreatedAt)
               .ToList();

            //foreach (var tribute in PublicTributes)
            //{
            //    tribute.PetName = EncryptionHelper.Decrypt(tribute.PetName);
            //    tribute.PetType = EncryptionHelper.Decrypt(tribute.PetType);
            //    tribute.PetBreed = EncryptionHelper.Decrypt(tribute.PetBreed);
            //    tribute.PetSex = EncryptionHelper.Decrypt(tribute.PetSex);
            //    tribute.Cause = EncryptionHelper.Decrypt(tribute.Cause);
            //    tribute.TributeText = EncryptionHelper.Decrypt(tribute.TributeText);
            //}




            Console.WriteLine($"Number of public tributes: {PublicTributes.Count}");
            Console.WriteLine($"Number of public posts: {PublicPosts.Count}");


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
                //_notificationService.AddNotificationAsync(userId, tributeId, newComment.Id).Wait();
            }

            // Redirect back to the page
            return RedirectToPage();
        }

        //public async Task<IActionResult> OnPostRemoveCommentAsync(int CommentId)
        //{

        //    try
        //    {
        //        _logger.LogInformation("Received media ID: {MediaId}", CommentId);
        //        var commentToRemove = await _context.Comment.FindAsync(CommentId);
        //        if (commentToRemove != null)
        //        {
        //            _context.Comment.Remove(commentToRemove);
        //            await _context.SaveChangesAsync();
        //            _logger.LogInformation("Comment removed successfully.");
        //            return new JsonResult(new { success = true });


        //        }
        //        return new JsonResult(new { success = false, error = "Could not remove comment." });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error removing comment:");
        //        return new JsonResult(new { success = false, error = ex.Message });
        //    }
        //}
        public async Task<IActionResult> OnPostRemoveCommentAsync(int CommentId)
        {
            try
            {
                _logger.LogInformation("Received media ID: {MediaId}", CommentId);

                // Find the comment to remove
                var commentToRemove = await _context.Comment.FindAsync(CommentId);
                if (commentToRemove == null)
                {
                    return new JsonResult(new { success = false, error = "Comment not found." });
                }

                // Remove related TributeNotification entries
                var notificationsToRemove = _context.TributeNotification
                    .Where(n => n.CommentId == CommentId)
                    .ToList();
                _context.TributeNotification.RemoveRange(notificationsToRemove);

                // Remove the comment
                _context.Comment.Remove(commentToRemove);

                // Save changes to the database
                await _context.SaveChangesAsync();

                _logger.LogInformation("Comment removed successfully.");
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing comment:");
                return new JsonResult(new { success = false, error = ex.Message });
            }
        }

    }
}

