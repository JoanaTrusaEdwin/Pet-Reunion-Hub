//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.GENERAL_LOCATION
//{
//    public class EditModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public EditModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//        public GENERALLOCATION GENERALLOCATION { get; set; } = default!;

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null || _context.GENERALLOCATION == null)
//            {
//                return NotFound();
//            }

//            var generallocation =  await _context.GENERALLOCATION.FirstOrDefaultAsync(m => m.Id == id);
//            if (generallocation == null)
//            {
//                return NotFound();
//            }
//            GENERALLOCATION = generallocation;
//           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
//            return Page();
//        }

//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see https://aka.ms/RazorPagesCRUD.
//        public async Task<IActionResult> OnPostAsync()
//        {
//            if (!ModelState.IsValid)
//            {
//                return Page();
//            }

//            _context.Attach(GENERALLOCATION).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!GENERALLOCATIONExists(GENERALLOCATION.Id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return RedirectToPage("./Index");
//        }

//        private bool GENERALLOCATIONExists(int id)
//        {
//          return (_context.GENERALLOCATION?.Any(e => e.Id == id)).GetValueOrDefault();
//        }
//    }
//}
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Pet_Reunion_Hub.Helper;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.GENERAL
{

    [Authorize]
    public class EditModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<EditModel> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        public EditModel(ILogger<EditModel> logger, DatabaseContext context, SignInManager<IdentityUser> signInManager, IConfiguration configuration, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public GENERALLOCATION GENERALLOCATION { get;set;}
        

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


        GENERALLOCATION = await _context.GENERALLOCATION
                                .FirstOrDefaultAsync(m => m.Id == id);


            if (GENERALLOCATION == null)
            {
                return NotFound();
            }
           
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {

            try
            {
                //if (!ModelState.IsValid)
                //{
                //    //If model state is invalid, return the page with validation errors
                //    return Page();
                //}

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

                    GENERALLOCATION.UserId = userId;
                    _logger.LogInformation("Assigned User ID: {UserId}", GENERALLOCATION.UserId);
                   

                    if (GENERALLOCATION.Id == 0)
                    {

                        //_context.CreateReport.Add(CreateReport);
                        GENERALLOCATION toBeUpdated = await _context.GENERALLOCATION.FindAsync(GENERALLOCATION.Id);
                        if (toBeUpdated != null)
                        {
                            //Console.WriteLine($"!!!!!!!!!!!!!!!!!!!!tobeupdated: ID: {toBeUpdated.Id}");
                            //await TryUpdateModelAsync<Tribute>(toBeUpdated, "Tribute", c => c.PetName, c => c.DateOfBirth, );
                        }
                        _context.GENERALLOCATION.Update(GENERALLOCATION);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var existingGENERALLOCATION = await _context.GENERALLOCATION.FirstOrDefaultAsync(m => m.Id == GENERALLOCATION.Id);
                        if (existingGENERALLOCATION == null)
                        {
                            return NotFound();
                        }


                        existingGENERALLOCATION.LOCATIONVALUE = GENERALLOCATION.LOCATIONVALUE;
                    

                        _context.Entry(existingGENERALLOCATION).State = EntityState.Modified;

                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Tribute saved successfully.");

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
                _logger.LogError(ex, "Error saving report:");

                throw;
            }

        }
    }
}