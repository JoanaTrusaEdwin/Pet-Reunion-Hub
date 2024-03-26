//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
//{
//    public class EditModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public EditModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//        public Tribute Tribute { get; set; } = default!;

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null || _context.Tribute == null)
//            {
//                return NotFound();
//            }

//            var tribute =  await _context.Tribute.FirstOrDefaultAsync(m => m.Id == id);
//            if (tribute == null)
//            {
//                return NotFound();
//            }
//            Tribute = tribute;
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

//            _context.Attach(Tribute).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!TributeExists(Tribute.Id))
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

//        private bool TributeExists(int id)
//        {
//          return (_context.Tribute?.Any(e => e.Id == id)).GetValueOrDefault();
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

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
{
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
        public Tribute Tribute { get; set; }
        [BindProperty]
        public IFormFile? photo { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            Tribute = await _context.Tribute
                                .FirstOrDefaultAsync(m => m.Id == id);

            if (Tribute == null)
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

                    Tribute.UserId = userId;
                    _logger.LogInformation("Assigned User ID: {UserId}", Tribute.UserId);
                    string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");

                    if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
                    {
                        return Page();
                    }

                    if (Tribute.Id == 0)
                    {
                        if (photo != null && photo.Length > 0)
                        {
                            string fileUrl = await UploadPhotoToBlobStorage(photo);
                            Tribute.TributePhoto = fileUrl;
                        }
                        //_context.CreateReport.Add(CreateReport);
                        Tribute toBeUpdated = await _context.Tribute.FindAsync(Tribute.Id);
                        if (toBeUpdated != null)
                        {
                            //Console.WriteLine($"!!!!!!!!!!!!!!!!!!!!tobeupdated: ID: {toBeUpdated.Id}");
                            //await TryUpdateModelAsync<Tribute>(toBeUpdated, "Tribute", c => c.PetName, c => c.DateOfBirth, );
                        }
                        _context.Tribute.Update(Tribute);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var existingTribute = await _context.Tribute.FirstOrDefaultAsync(m => m.Id == Tribute.Id);
                        if (existingTribute == null)
                        {
                            return NotFound();
                        }


                        existingTribute.PetName = Tribute.PetName;
                        existingTribute.PetType = Tribute.PetType;
                        existingTribute.PetBreed = Tribute.PetBreed;
                        existingTribute.PetSex = Tribute.PetSex;
                        existingTribute.DateOfBirth = Tribute.DateOfBirth;
                        existingTribute.DateOfAdoption = Tribute.DateOfAdoption;
                        existingTribute.DateOfDeparture = Tribute.DateOfDeparture;
                        existingTribute.Cause = Tribute.Cause;
                        existingTribute.TributeText = Tribute.TributeText;
                        //existingTribute.TributePhoto = Tribute.TributePhoto;
                        existingTribute.IsPublic = Tribute.IsPublic;

                        if (photo != null && photo.Length > 0)
                        {
                            string fileUrl = await UploadPhotoToBlobStorage(photo);
                            existingTribute.TributePhoto = fileUrl;
                        }
                        _context.Entry(existingTribute).State = EntityState.Modified;

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

        private async Task<string> UploadPhotoToBlobStorage(IFormFile photo)
        {
            string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");
            if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
            {
                return null;
            }

            string containerName = "newprhcontainer";

            var blobServiceClient = new BlobServiceClient(azureBlobStorageConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
            var blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = photo.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.ToString();
        }
    }
}
