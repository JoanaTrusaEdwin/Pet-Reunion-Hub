using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRHDATALIB.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using System.Configuration;


namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.USER
{
    public class CreateModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ILogger<CreateModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public CreateModel(ILogger<CreateModel> logger, PRHDATALIB.Models.DatabaseContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            Newtest = new Newtest { UserId = _userManager.GetUserId(User) };
            return Page();
        }

        [BindProperty]
        public Newtest Newtest { get; set; } = default!;
        

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
                        return RedirectToPage("/Account/Login");
                    }

                    Newtest.UserId = userId;
                    _logger.LogInformation("Assigned User ID: {UserId}", Newtest.UserId);

                    _context.NEWTEST.Add(Newtest);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("New test created successfully.");
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
                _logger.LogError(ex, "Error creating new test:");
                throw;
            }

            //_logger.LogInformation("Attempting to create new test...");
            //try
            //{
            //    // Your database operations here
            //    _context.NEWTEST.Add(Newtest);
            //    await _context.SaveChangesAsync();
            //    _logger.LogInformation("New test created successfully.");
            //    return RedirectToPage("./Index");
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error creating new test:");
            //    throw;
            //}

            //if (!ModelState.IsValid || _context.NEWTEST == null || Newtest == null)
            //{
            //    return Page();
            //}

            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            //// Populate the user ID property of the Newtest entity
            //Newtest.UserId = userId;

            //_context.NEWTEST.Add(Newtest);
            //await _context.SaveChangesAsync();

            //return RedirectToPage("./Index");
        }
    }
}
