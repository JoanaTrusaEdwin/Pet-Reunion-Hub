


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

        public CommunityMemorialsModel(DatabaseContext context, UserManager<IdentityUser> userManager, INotificationService notificationService)
        {
            _context = context;
            _userManager = userManager;
            _notificationService = notificationService;
        }

        public List<Tribute> PublicTributes { get; set; }


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

            return Page();
        }

        public IActionResult OnPost(int tributeId, string commentContent)
        {
            var userId = _userManager.GetUserId(User);

            // Find the tribute to which the comment belongs
            var tribute = _context.Tribute.FirstOrDefault(t => t.Id == tributeId);

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

                _notificationService.AddNotificationAsync(userId, tributeId, newComment.Id).Wait();
            }

            // Redirect back to the page
            return RedirectToPage();
        }

    }
}

