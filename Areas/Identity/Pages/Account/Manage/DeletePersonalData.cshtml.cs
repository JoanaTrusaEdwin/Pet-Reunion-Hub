// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Pet_Reunion_Hub.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly PRHDATALIB.Models.DatabaseContext _dbContext; // Up

        public DeletePersonalDataModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<DeletePersonalDataModel> logger,
           PRHDATALIB.Models.DatabaseContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _dbContext = dbContext;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            //var result = await _userManager.DeleteAsync(user);
            //var userId = await _userManager.GetUserIdAsync(user);

            // Delete related records
            var createReports = await _dbContext.CreateReport.Where(r => r.UserId == user.Id).ToListAsync();
            _dbContext.CreateReport.RemoveRange(createReports);

            var tributes = await _dbContext.Tribute.Where(t => t.UserId == user.Id).ToListAsync();
            _dbContext.Tribute.RemoveRange(tributes);

            var posts = await _dbContext.Post.Where(p => p.UserId == user.Id).ToListAsync();
            _dbContext.Post.RemoveRange(posts);

            var comments = await _dbContext.Comment.Where(rp => rp.TributeId != null && rp.Tribute.UserId == user.Id).ToListAsync();
            _dbContext.Comment.RemoveRange(comments);

            var userCommentsOnOtherTributes = await _dbContext.Comment
    .Include(c => c.Tribute)
    .Where(c => c.UserId == user.Id && c.Tribute.UserId != user.Id)
    .ToListAsync();
            _dbContext.Comment.RemoveRange(userCommentsOnOtherTributes);

            var postComments = await _dbContext.POSTCOMMENT.Where(rp => rp.PostId != null && rp.Post.UserId == user.Id).ToListAsync();
            _dbContext.POSTCOMMENT.RemoveRange(postComments);


            var userCommentsOnOtherPosts = await _dbContext.POSTCOMMENT
    .Include(c => c.Post)
    .Where(c => c.UserId == user.Id && c.Post.UserId != user.Id)
    .ToListAsync();
            _dbContext.POSTCOMMENT.RemoveRange(userCommentsOnOtherPosts);


            var generalLocations = await _dbContext.GENERALLOCATION.Where(gl => gl.UserId == user.Id).ToListAsync();
            _dbContext.GENERALLOCATION.RemoveRange(generalLocations);

            var newNotifications = await _dbContext.NEWNOTIFICATION.Where(nn => nn.UserId == user.Id).ToListAsync();
            _dbContext.NEWNOTIFICATION.RemoveRange(newNotifications);

            var resources = await _dbContext.RESOURCE.Where(r => r.UserId == user.Id).ToListAsync();
            _dbContext.RESOURCE.RemoveRange(resources);

            //var reportPhotos = await _dbContext.ReportPhoto.Where(rp => rp.ReportId.HasValue && rp.CreateReport.UserId == user.Id).ToListAsync();
            //_dbContext.ReportPhoto.RemoveRange(reportPhotos);

            var reportPhotos = await _dbContext.ReportPhoto.Where(rp => rp.ReportId != null && rp.CreateReport.UserId == user.Id).ToListAsync();
            _dbContext.ReportPhoto.RemoveRange(reportPhotos);

            var media = await _dbContext.Media.Where(m => m.Post.UserId == user.Id).ToListAsync();
            _dbContext.Media.RemoveRange(media);

            await _dbContext.SaveChangesAsync();

            var result = await _userManager.DeleteAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);

            //await _dbContext.SaveChangesAsync();
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }
    }
}
