//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
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
//        public Tribute Tribute { get; set; } = default!;


//        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
//        public async Task<IActionResult> OnPostAsync()
//        {
//          if (!ModelState.IsValid || _context.Tribute == null || Tribute == null)
//            {
//                return Page();
//            }

//            _context.Tribute.Add(Tribute);
//            await _context.SaveChangesAsync();

//            return RedirectToPage("./Index");
//        }
//    }
//}
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

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
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
            Tribute = new Tribute { UserId = _userManager.GetUserId(User) };
            return Page();
        }

        [BindProperty]
        public Tribute Tribute { get; set; } = new Tribute();

        [BindProperty]
        public IFormFile? photo { get; set; }


        public async Task<IActionResult> OnPostAsync(IFormFile photo)
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

                    Tribute.UserId = userId;
                    _logger.LogInformation("Assigned User ID: {UserId}", Tribute.UserId);
                    string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");

                    if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
                    {

                        return Page();
                    }
                    if (photo != null && photo.Length > 0)
                    {

                        string connectionString = _configuration["AzureBlobStorageConnectionString"];
                        var blobServiceClient = new BlobServiceClient(azureBlobStorageConnectionString);
                        var containerClient = blobServiceClient.GetBlobContainerClient("tributes");
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                        var blobClient = containerClient.GetBlobClient(fileName);
                        using (var stream = photo.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, true);
                        }

                        var fileUrl = blobClient.Uri.ToString();
                        Tribute.TributePhoto = fileUrl;
                    }

                    Tribute.PetName = EncryptionHelper.Encrypt(Tribute.PetName);
                    Tribute.PetType = EncryptionHelper.Encrypt(Tribute.PetType);
                    Tribute.PetBreed = EncryptionHelper.Encrypt(Tribute.PetBreed);
                    Tribute.PetSex = EncryptionHelper.Encrypt(Tribute.PetSex);
                    //Tribute.DateOfBirth = EncryptionHelper.Encrypt(Tribute.DateOfBirth);
                    //Tribute.DateOfAdoption = EncryptionHelper.Encrypt(Tribute.DateOfAdoption);
                    //Tribute.DateOfDeparture = EncryptionHelper.Encrypt(Tribute.DateOfDeparture);
                    Tribute.Cause = EncryptionHelper.Encrypt(Tribute.Cause);
                    Tribute.TributeText = EncryptionHelper.Encrypt(Tribute.TributeText);
                    //Tribute.TributePhoto = EncryptionHelper.Encrypt(Tribute.TributePhoto);
                    //Tribute.Visibility = EncryptionHelper.Encrypt(Tribute.Visibility);

                    _context.Tribute.Add(Tribute);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("New tribute created successfully.");
                    //return RedirectToPage("./Index");
                }
                return RedirectToPage("./Index");
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