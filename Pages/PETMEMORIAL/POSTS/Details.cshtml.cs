using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Cms;
using Pet_Reunion_Hub.Helper;
using Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW;

using PRHDATALIB.Models;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.POSTS
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;
    
        private readonly ILogger<CommunityMemorialsModel> _logger;

        public DetailsModel(ILogger<CommunityMemorialsModel> logger, DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            
        }

      public Post Post { get; set; } = default!;
      public List<IFormFile> MediaFiles { get; set; } = new List<IFormFile>();
      public List<POSTCOMMENT> Comments { get; set; } = new List<POSTCOMMENT>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

             Post = await _context.Post
            .Include(cr => cr.Media) // Include the ReportPhotos collection
            .Include(t => t.PostComments).ThenInclude(c => c.User)
            .FirstOrDefaultAsync(m => m.Id == id);

            //var post = await _context.Post.FirstOrDefaultAsync(m => m.Id == id);
            //if (post == null)
            //{
            //    return NotFound();
            //}
            //else 
            //{
            //    Post = post;
            //}
            //return Page
            if (Post == null)
            {
                return NotFound();
            }
            Comments = Post.PostComments.ToList();
            //else
            //{
            //    Post.Title = EncryptionHelper.Decrypt(Post.Title);
            //    Post.Content = EncryptionHelper.Decrypt(Post.Content);
            //}
            return Page();
        }
        public IActionResult OnPost(int postId, string commentContent)
        {
            var userId = _userManager.GetUserId(User);

            // Find the tribute to which the comment belongs
            var post = _context.Post.Include(t => t.User).FirstOrDefault(t => t.Id == postId);

            if (post != null)
            {
                var newComment = new POSTCOMMENT
                {
                    Content = commentContent,
                    UserId = userId,
                    PostId = postId
                };

                // Add the comment to the database
                _context.POSTCOMMENT.Add(newComment);
                _context.SaveChanges();
                var mentionedUserEmail = ExtractMentionedUserEmail(commentContent);
                if (!string.IsNullOrEmpty(mentionedUserEmail))
                {
                    // Find the mentioned user's IdentityUser object
                    var mentionedUser = _context.Users.FirstOrDefault(u => u.Email == mentionedUserEmail);
                    if (mentionedUser != null)
                    {
                        // Add a notification for the mentioned user
                        var notification = new NEWNOTIFICATION
                        {
                            UserId = mentionedUser.Id,
                            Content = $"(Post Comment ID: {newComment.Id}) {_userManager.GetUserName(User)} mentioned you in a comment on {post.User.UserName}'s post '{post.Title}' with Post ID '{post.Id}'",
                            IsRead = false,
                            CreatedAt = DateTime.Now
                        };

                        _context.NEWNOTIFICATION.Add(notification);
                        _context.SaveChanges();
                    }
                }
                // Create a new comment
                //var newComment = new POSTCOMMENT
                //{
                //    Content = commentContent,
                //    UserId = userId,
                //    PostId = postId
                //};

                //// Add the comment to the database
                //_context.POSTCOMMENT.Add(newComment);
                //_context.SaveChanges();

                if (post.User.Id != userId)
                {
                    var commenter = _context.Users.FirstOrDefault(u => u.Id == newComment.UserId);
                    // Add a notification for the tribute owner
                    var notification = new NEWNOTIFICATION
                    {
                        UserId = post.UserId,
                        Content = $"(Post Comment ID: {newComment.Id}) You have a new comment on your post '{post.Title}' with Post ID '{Post.Id}' by {commenter.UserName}",
                        IsRead = false,
                        CreatedAt = DateTime.Now
                    };

                    _context.NEWNOTIFICATION.Add(notification);
                    _context.SaveChanges();
                }
            }

            // Redirect back to the page
            return RedirectToPage(new { id = postId });
        }
        private string ExtractMentionedUserEmail(string commentContent)
        {
            // Implement the logic to extract the mentioned user's email from the comment content
            // For example, you can use a regular expression to find the mentioned user's email
            // In this example, I'm assuming the mentioned user's email is in the format "user@example.com"
            var mentionedUserEmail = Regex.Match(commentContent, @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}").Value;
            return mentionedUserEmail;
        }
        public async Task<IActionResult> OnPostRemoveCommentAsync(int commentId)
        {
            try
            {
                var commentToRemove = await _context.POSTCOMMENT.FindAsync(commentId);
                if (commentToRemove != null)
                {
                    _context.POSTCOMMENT.Remove(commentToRemove);
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
