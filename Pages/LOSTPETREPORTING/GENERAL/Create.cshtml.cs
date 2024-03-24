//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.GENERAL_LOCATION
//{
//    public class CreateModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public CreateModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        public IActionResult OnGet()
//        {
//        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
//            return Page();
//        }

//        [BindProperty]
//        public GENERALLOCATION GENERALLOCATION { get; set; } = default!;


//        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
//        public async Task<IActionResult> OnPostAsync()
//        {
//          if (!ModelState.IsValid || _context.GENERALLOCATION == null || GENERALLOCATION == null)
//            {
//                return Page();
//            }

//            _context.GENERALLOCATION.Add(GENERALLOCATION);
//            await _context.SaveChangesAsync();

//            return RedirectToPage("./Index");
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRHDATALIB.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Configuration;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Protocol.Plugins;
using System.Security.Principal;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.GENERAL_LOCATION
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ILogger<CreateModel> logger, PRHDATALIB.Models.DatabaseContext context, IConfiguration configuration,
                             UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            GENERALLOCATION = new GENERALLOCATION { UserId = _userManager.GetUserId(User) };
            return Page();
        }

        [BindProperty]
        public GENERALLOCATION GENERALLOCATION { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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
                        ////return RedirectToPage("/Account/Login");
                        //return RedirectToPage("/Identity/Account/Login");

                    }

                    GENERALLOCATION.UserId = userId;
                    _logger.LogInformation("Assigned User ID: {UserId}", GENERALLOCATION.UserId);
                    

                    
                    _context.GENERALLOCATION.Add(GENERALLOCATION);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("Location saved successfully.");
             
                }
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new report:");
                //return RedirectToPage("/Error");
                throw;
            }

            _context.GENERALLOCATION.Add(GENERALLOCATION);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
