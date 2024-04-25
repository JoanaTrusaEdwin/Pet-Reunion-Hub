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

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.POSTS
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<DeleteModel> _logger;

        public DeleteModel(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager, ILogger<DeleteModel> logger)
        {
            
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        [BindProperty]
        public Post Post { get; set; } = default!;

        //public Media Media { get; set; }
        public List<IFormFile> MediaFiles { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

            var post = await _context.Post.Include(p => p.Media).FirstOrDefaultAsync(m => m.Id == id);

            if (post == null)
            {
                return NotFound();
            }
            else 
            {
                Post = post;
                //post.Title = EncryptionHelper.Decrypt(post.Title);
                //post.Content = EncryptionHelper.Decrypt(post.Content);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            //if (id == null || _context.Post == null)
            //{
            //    return NotFound();
            //}
            //var post = await _context.Post.FindAsync(id);

            //if (post != null)
            //{
            //    Post = post;
            //    _context.Post.Remove(Post);
            //    await _context.SaveChangesAsync();
            //}

            //return RedirectToPage("./Index
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

                    Post = await _context.Post.FindAsync(id);

                    if (Post == null)
                    {
                        return NotFound();
                    }

                    var postMedia = await _context.Media.Where(rp => rp.PostId == Post.Id).ToListAsync();
                    foreach (var media in postMedia)
                    {
                        _context.Remove(media);
                    }

                    _context.Post.Remove(Post);
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
