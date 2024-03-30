//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.COPINGRESOURCES.NEW
//{
//    public class EditModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public EditModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//        public RESOURCE RESOURCE { get; set; } = default!;

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null || _context.RESOURCE == null)
//            {
//                return NotFound();
//            }

//            var resource =  await _context.RESOURCE.FirstOrDefaultAsync(m => m.Id == id);
//            if (resource == null)
//            {
//                return NotFound();
//            }
//            RESOURCE = resource;
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

//            _context.Attach(RESOURCE).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!RESOURCEExists(RESOURCE.Id))
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

//        private bool RESOURCEExists(int id)
//        {
//          return (_context.RESOURCE?.Any(e => e.Id == id)).GetValueOrDefault();
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

namespace Pet_Reunion_Hub.Pages.COPINGRESOURCES.NEW
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
        public RESOURCE RESOURCE { get; set; }
        [BindProperty]
        public IFormFile? UploadedFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            RESOURCE = await _context.RESOURCE
                                .FirstOrDefaultAsync(m => m.Id == id);


            if (RESOURCE == null)
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

                    RESOURCE.UserId = userId;
                    _logger.LogInformation("Assigned User ID: {UserId}", RESOURCE.UserId);
                    string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");

                    if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
                    {
                        return Page();
                    }

                    if (RESOURCE.Id == 0)
                    {
                        if (UploadedFile != null && UploadedFile.Length > 0)
                        {
                            string fileUrl = await UploadPhotoToBlobStorage(UploadedFile);
                            RESOURCE.FILEURL = fileUrl;
                        }
                        //_context.CreateReport.Add(CreateReport);
                        RESOURCE toBeUpdated = await _context.RESOURCE.FindAsync(RESOURCE.Id);
                        if (toBeUpdated != null)
                        {
                            //Console.WriteLine($"!!!!!!!!!!!!!!!!!!!!tobeupdated: ID: {toBeUpdated.Id}");
                            //await TryUpdateModelAsync<Tribute>(toBeUpdated, "Tribute", c => c.PetName, c => c.DateOfBirth, );
                        }
                        _context.RESOURCE.Update(RESOURCE);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var existingRESOURCE = await _context.RESOURCE.FirstOrDefaultAsync(m => m.Id == RESOURCE.Id);
                        if (existingRESOURCE == null)
                        {
                            return NotFound();
                        }

                        existingRESOURCE.TITLE = RESOURCE.TITLE;
                        existingRESOURCE.DESCRIPTION = RESOURCE.DESCRIPTION;
                        existingRESOURCE.LINK = RESOURCE.LINK;
                        existingRESOURCE.FILEURL = RESOURCE.FILEURL;
                        existingRESOURCE.TYPE = RESOURCE.TYPE;
                        existingRESOURCE.FORMAT = RESOURCE.FORMAT;


                        if (UploadedFile != null && UploadedFile.Length > 0)
                        {
                            string fileUrl = await UploadPhotoToBlobStorage(UploadedFile);
                            existingRESOURCE.FILEURL = fileUrl;
                        }

                        //existingTribute.PetName = EncryptionHelper.Encrypt(existingTribute.PetName);
                        //existingTribute.PetType = EncryptionHelper.Encrypt(existingTribute.PetType);
                        //existingTribute.PetBreed = EncryptionHelper.Encrypt(existingTribute.PetBreed);
                        //existingTribute.PetSex = EncryptionHelper.Encrypt(existingTribute.PetSex);
                        //existingTribute.Cause = EncryptionHelper.Encrypt(existingTribute.Cause);
                        //existingTribute.TributeText = EncryptionHelper.Encrypt(existingTribute.TributeText);
                        //existingTribute.Visibility = EncryptionHelper.Encrypt(existingTribute.Visibility);

                        _context.Entry(existingRESOURCE).State = EntityState.Modified;

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

        private async Task<string> UploadPhotoToBlobStorage(IFormFile UploadedFile)
        {
            string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");
            if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
            {
                return null;
            }

            string containerName = "tributes";

            var blobServiceClient = new BlobServiceClient(azureBlobStorageConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(UploadedFile.FileName);
            var blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = UploadedFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.ToString();
        }
    }
}