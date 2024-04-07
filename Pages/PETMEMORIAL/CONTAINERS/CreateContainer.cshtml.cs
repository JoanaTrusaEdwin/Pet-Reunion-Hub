using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using PRHDATALIB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System.Configuration;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Protocol.Plugins;
using System.Security.Principal;
using Pet_Reunion_Hub.Helper;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.CONTAINERS
{
    [Authorize]
    public class CreateContainerModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<CreateContainerModel> _logger;

        public CreateContainerModel(ILogger<CreateContainerModel> logger, PRHDATALIB.Models.DatabaseContext context, IConfiguration configuration,
                           UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [BindProperty]
        public CONTAINER CONTAINER { get; set; } = new CONTAINER();

        public IActionResult OnGet()
        {
            CONTAINER = new CONTAINER { UserId = _userManager.GetUserId(User) };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
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
                        //return RedirectToPage("/Account/Login");
                        //return RedirectToPage("/Identity/Account/Login");

                    }

                    CONTAINER.UserId = userId;
                    _logger.LogInformation("Assigned User ID: {UserId}", CONTAINER.UserId);
                   
                    _context.CONTAINER.Add(CONTAINER);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("New post created successfully.");
                    //return RedirectToPage("./Index");

                 

                    //_context.Post.Add(Post);
                    //await _context.SaveChangesAsync();
                    //_logger.LogInformation("New post created successfully.");
                    ////return RedirectToPage("./Index");
                }
                return RedirectToPage("./Containers");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new report:");
                //return RedirectToPage("/Error");
                throw;
            }
        }

    }
}
