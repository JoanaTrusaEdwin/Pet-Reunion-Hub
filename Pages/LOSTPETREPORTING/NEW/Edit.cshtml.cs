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

                    if (CreateReport.Id == 0)
                    {

                        if (Photo != null && Photo.Length > 0)
                        {
                            string fileUrl = await UploadPhotoToBlobStorage(Photo);
                            CreateReport.MainPhoto = fileUrl;
                        }

                        //_context.CreateReport.Add(CreateReport);
                        CreateReport toBeUpdated = await _context.CreateReport.FindAsync(CreateReport.Id);
                        if (toBeUpdated != null)
                        {
                            Console.WriteLine($"!!!!!!!!!!!!!!!!!!!!tobeupdated: ID: {toBeUpdated.Id}");
                            await TryUpdateModelAsync<CreateReport>(toBeUpdated, "CreateReport", c => c.PetName, c => c.PetBreed, c => c.PetMicrochipID, c => c.LastSeenLocation, c => c.LastSeenTime, c => c.DateOfBirth);
                        }
                        _context.CreateReport.Update(CreateReport);
                        _context.SaveChanges();
                    }
                    else
                    {
                        var existingReport = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == CreateReport.Id);
                        if (existingReport == null)
                        {
                            return NotFound();
                        }



                        existingReport.PetName = CreateReport.PetName;
                        existingReport.PetBreed = CreateReport.PetBreed;


                        if (Photo != null && Photo.Length > 0)
                        {
                            string fileUrl = await UploadPhotoToBlobStorage(Photo);
                            existingReport.MainPhoto = fileUrl;
                        }


                        await UpdateOrCreateReportPhotos(existingReport, Photos);
                        _context.Entry(existingReport).State = EntityState.Modified;
                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Report saved successfully.");

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
                //return RedirectToPage("/Error");
                throw;
            }
        }


        private async Task UpdateOrCreateReportPhotos(CreateReport existingReport, List<IFormFile> photos)
        {
            // Clear existing photos if any
            existingReport.ReportPhotos.Clear();

            // Add new photos
            foreach (var photo in photos)
            {
                string fileUrl = await UploadPhotoToBlobStorage(photo);

                // Create a new ReportPhoto object and associate it with the report
                var reportPhoto = new ReportPhoto { PhotoUrl = fileUrl };
                existingReport.ReportPhotos.Add(reportPhoto);
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



//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Logging;
//using PRHDATALIB.Models;
//using Azure.Storage.Blobs;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
//{
//    public class EditModel : PageModel
//    {
//        private readonly DatabaseContext _context;
//        private readonly IConfiguration _configuration;
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly ILogger<EditModel> _logger;

//        public EditModel(ILogger<EditModel> logger, DatabaseContext context, IConfiguration configuration, UserManager<IdentityUser> userManager)
//        {
//            _logger = logger;
//            _context = context;
//            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
//            _userManager = userManager;
//        }

//        [BindProperty]
//        public CreateReport CreateReport { get; set; }

//        [BindProperty]
//        public IFormFile MainPhoto { get; set; }

//        [BindProperty]
//        public List<IFormFile> Photos { get; set; }

//        public List<ReportPhoto> ExistingPhotos { get; set; }

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            CreateReport = await _context.CreateReport
//                .Include(c => c.ReportPhotos)
//                .FirstOrDefaultAsync(m => m.Id == id);

//            if (CreateReport == null)
//            {
//                return NotFound();
//            }

//            // Populate existing photos
//            ExistingPhotos = CreateReport.ReportPhotos.ToList();

//            return Page();
//        }

//        public async Task<IActionResult> OnPostAsync()
//        {
//            try
//            {
//                var user = await _userManager.GetUserAsync(User);
//                if (user == null)
//                {
//                    _logger.LogError("User not found.");
//                    return RedirectToPage("/Account/Login");
//                }

//                // Ensure user ID is assigned to the report
//                string userId = user.Id;
//                CreateReport.UserId = userId;

//                // Handle main photo upload and update
//                if (MainPhoto != null && MainPhoto.Length > 0)
//                {
//                    string mainPhotoUrl = await UploadPhotoToBlobStorage(MainPhoto, _configuration.GetConnectionString("AzureBlobStorageConnectionString"));
//                    CreateReport.MainPhoto = mainPhotoUrl;
//                }

//                // Handle additional photo upload and update
//                await HandlePhotoUpload();

//                // Update the report in the database
//                _context.Update(CreateReport);
//                await _context.SaveChangesAsync();

//                _logger.LogInformation("Report saved successfully.");
//                return RedirectToPage("./Index");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error saving report:");
//                throw;
//            }
//        }

//        private async Task HandlePhotoUpload()
//        {
//            string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");

//            // Iterate through existing photos and remove those marked for deletion
//            foreach (var photo in ExistingPhotos)
//            {
//                if (Photos.Any(p => p.FileName == photo.PhotoUrl && p.Length == 0))
//                {
//                    // Photo marked for deletion
//                    CreateReport.ReportPhotos.Remove(photo);
//                }
//            }

//            // Add new photos
//            foreach (var photo in Photos)
//            {
//                if (photo != null && photo.Length > 0)
//                {
//                    string fileUrl = await UploadPhotoToBlobStorage(photo, azureBlobStorageConnectionString);
//                    var reportPhoto = new ReportPhoto { PhotoUrl = fileUrl };

//                    // Add the uploaded photo to the report
//                    CreateReport.ReportPhotos.Add(reportPhoto);
//                }
//            }
//        }

//        private async Task<string> UploadPhotoToBlobStorage(IFormFile photo, string connectionString)
//        {
//            if (string.IsNullOrEmpty(connectionString))
//            {
//                return null;
//            }

//            string containerName = "newprhcontainer";

//            var blobServiceClient = new BlobServiceClient(connectionString);
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
