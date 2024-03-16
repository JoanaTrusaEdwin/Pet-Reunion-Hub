//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
//{
//    public class EditModel : PageModel
//   {
//       private readonly PRHDATALIB.Models.DatabaseContext _context;

//       public EditModel(PRHDATALIB.Models.DatabaseContext context)
//       {
//           _context = context;
//       }

//        [BindProperty]
//        public CreateReport CreateReport { get; set; } = default!;


//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null || _context.CreateReport == null)
//           {
//                return NotFound();
//           }

//           var createreport =  await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == id);
//           if (createreport == null)
//           {
//               return NotFound();
//           }
//           CreateReport = createreport;
//            return Page();
//       }

//       // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see https://aka.ms/RazorPagesCRUD.
//       public async Task<IActionResult> OnPostAsync()
//       {
//           if (!ModelState.IsValid)
//           {
//               return Page();
//           }

//           _context.Attach(CreateReport).State = EntityState.Modified;

//           try
//           {
//               await _context.SaveChangesAsync();
//           }
//           catch (DbUpdateConcurrencyException)
//           {
//               if (!CreateReportExists(CreateReport.Id))
//               {
//                   return NotFound();
//               }
//               else
//               {
//                    throw;
//                }
//            }

//           return RedirectToPage("./Index");
//       }

//        private bool CreateReportExists(int id)
//        {
//         return (_context.CreateReport?.Any(e => e.Id == id)).GetValueOrDefault();
//        }
//    }
//}

//using System;
//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;
//using Azure.Storage.Blobs;
//using Microsoft.Extensions.Configuration;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
//{
//    public class EditModel : PageModel
//    {
//        private readonly DatabaseContext _context;
//        private readonly IConfiguration _configuration;

//        public EditModel(DatabaseContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
//        }

//        [BindProperty]
//        public CreateReport CreateReport { get; set; }

//        [BindProperty]
//        public IFormFile Photo { get; set; }

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            CreateReport = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == id);

//            if (CreateReport == null)
//            {
//                return NotFound();
//            }

//            return Page();
//        }

//        public async Task<IActionResult> OnPostAsync()
//        {
//            if (!ModelState.IsValid)
//            {
//                return Page();
//            }

//            var existingReport = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == CreateReport.Id);

//            if (existingReport == null)
//            {
//                return NotFound();
//            }

//            existingReport.PetName = CreateReport.PetName;
//            existingReport.PetBreed = CreateReport.PetBreed;
//            // Update other properties similarly...

//            try
//            {
//                if (Photo != null)
//                {
//                    Console.WriteLine($"Uploaded file: {Photo.FileName}, Size: {Photo.Length} bytes");

//                    string fileUrl = await UploadPhotoToBlobStorage(Photo);
//                    existingReport.MainPhoto = fileUrl;
//                }
//                else
//                {
//                    Console.WriteLine("No file uploaded");
//                }

//                _context.CreateReport.Update(existingReport);
//                await _context.SaveChangesAsync();

//                return RedirectToPage("./Index");
//            }
//            catch (Exception ex)
//            {
//                // Log any exceptions that occur during the photo upload process
//                Console.WriteLine($"An error occurred: {ex.Message}");
//                return Page();
//            }
//        }

//        private async Task<string> UploadPhotoToBlobStorage(IFormFile photo)
//        {
//            string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");
//            if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
//            {
//                return null;
//            }

//            string containerName = "newprhcontainer";

//            var blobServiceClient = new BlobServiceClient(azureBlobStorageConnectionString);
//            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

//            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
//            var blobClient = containerClient.GetBlobClient(fileName);

//            using (var stream = photo.OpenReadStream())
//            {
//                await blobClient.UploadAsync(stream, true);
//            }

//            return blobClient.Uri.ToString();
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

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
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
        public CreateReport CreateReport { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

        [BindProperty]
        public List<IFormFile> Photos { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateReport = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == id);

            if (CreateReport == null)
            {
                return NotFound();
            }

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
                        return RedirectToPage("/Account/Login");
                    }

                    CreateReport.UserId = userId;
                    _logger.LogInformation("Assigned User ID: {UserId}", CreateReport.UserId);
                    string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");

                    if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
                    {
                        return Page();
                    }

                    if (Photo != null && Photo.Length > 0)
                    {
                        string fileUrl = await UploadPhotoToBlobStorage(Photo);
                        CreateReport.MainPhoto = fileUrl;
                    }

                    _context.CreateReport.Add(CreateReport);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("New report created successfully.");

                    // Save additional photos
                    if (Photos != null && Photos.Count > 0)
                    {
                        foreach (var photo in Photos)
                        {
                            string fileUrl = await UploadPhotoToBlobStorage(photo);

                            var reportPhoto = new ReportPhoto { ReportId = CreateReport.Id, PhotoUrl = fileUrl };
                            _context.ReportPhoto.Add(reportPhoto);
                            await _context.SaveChangesAsync();
                        }
                    }

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
                _logger.LogError(ex, "Error creating new report:");
                //return RedirectToPage("/Error");
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
