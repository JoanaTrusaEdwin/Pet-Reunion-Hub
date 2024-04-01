
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using PRHDATALIB.Models;
//using Azure.Storage.Blobs;
//using System.Configuration;
//using Microsoft.Extensions.Configuration;
//using Microsoft.AspNetCore.Authorization;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
//{
//    [Authorize(Policy = "RequireLoggedIn")]
//    public class CreateModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;
//        //NEW HERE
//        private readonly IConfiguration _configuration;
//        //NEW HERE

//        public CreateModel(PRHDATALIB.Models.DatabaseContext context, IConfiguration configuration)
//        {
//            _context = context;
//            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
//        }

//        public IActionResult OnGet()
//        {
//            return Page();
//        }

//        [BindProperty]
//        public CreateReport CreateReport { get; set; } = default!;
//        //NEW HERE
//        public IFormFile Photo { get; set; }
//        //NEW HERE
//        public List<IFormFile> Photos { get; set; }

//        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
//        public async Task<IActionResult> OnPostAsync()
//        {


//            if (!ModelState.IsValid || _context.CreateReport == null || CreateReport == null)
//            {
//                return Page();
//            }


//            string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");
//            if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
//            {
//                // Handle the case when connectionString is null or empty
//                // You can log an error or return an appropriate response
//                return Page();
//            }




//            // Check if the connectionString is not null or empty

//            if (Photo != null && Photo.Length > 0)
//            {
//                // Retrieve the connection string for Azure Blob Storage from appsettings.json
//                string connectionString = _configuration["AzureBlobStorageConnectionString"];

//                // Create a BlobServiceClient using the connection string
//                var blobServiceClient = new BlobServiceClient(azureBlobStorageConnectionString);

//                // Get a reference to the container where you want to store the files
//                var containerClient = blobServiceClient.GetBlobContainerClient("newprhcontainer");

//                // Generate a unique filename for the uploaded file
//                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);

//                // Get a reference to the blob in the container
//                var blobClient = containerClient.GetBlobClient(fileName);

//                // Upload the file to Azure Blob Storage
//                using (var stream = Photo.OpenReadStream())
//                {
//                    await blobClient.UploadAsync(stream, true);
//                }

//                // Optionally, you can get the URL of the uploaded file
//                var fileUrl = blobClient.Uri.ToString();
//                CreateReport.MainPhoto = fileUrl;

//                // Now you can save the file URL or other information to your database
//                // For example:
//                // var report = new CreateReport { MainPhotoUrl = fileUrl };
//                // Save the report to your database
//            }

//            _context.CreateReport.Add(CreateReport);
//            await _context.SaveChangesAsync();

//            if (Photos != null && Photos.Count > 0)
//            {
                            //foreach (var photo in Photos)
                            //{
        //                        // Retrieve the connection string for Azure Blob Storage from appsettings.json
        //                        string connectionString = _configuration["AzureBlobStorageConnectionString"];

        //                        // Create a BlobServiceClient using the connection string
        //                        var blobServiceClient = new BlobServiceClient(azureBlobStorageConnectionString);

        //                        // Get a reference to the container where you want to store the files
        //                        var containerClient = blobServiceClient.GetBlobContainerClient("newprhcontainer");

        //                        // Generate a unique filename for the uploaded file
        //                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);

        //                        // Get a reference to the blob in the container
        //                        var blobClient = containerClient.GetBlobClient(fileName);

        //                        // Upload the file to Azure Blob Storage
        //                        using (var stream = photo.OpenReadStream())
        //                        {
        //                            await blobClient.UploadAsync(stream, true);
        //                        }

        //                        // Optionally, you can get the URL of the uploaded file
        //                        var fileUrl = blobClient.Uri.ToString();

        //                        // Save the photo URL to the database
        //                        var reportPhoto = new ReportPhoto { ReportId = CreateReport.Id, PhotoUrl = fileUrl };
        //                        _context.ReportPhoto.Add(reportPhoto);
        //                        await _context.SaveChangesAsync();
        //                    }
//            }


//            //NEW HERE

//            //_context.CreateReport.Add(CreateReport);
//            //await _context.SaveChangesAsync();

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

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
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

        [Authorize]
        public IActionResult OnGet()
        {
            CreateReport = new CreateReport { UserId = _userManager.GetUserId(User) };
            return Page();
        }

        [BindProperty]
        public CreateReport CreateReport { get; set; } = new CreateReport();

        [BindProperty]
        public IFormFile Photo { get; set; }

        [BindProperty]
        public List<IFormFile> Photos { get; set; }

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

                    CreateReport.UserId = userId;
                    _logger.LogInformation("Assigned User ID: {UserId}", CreateReport.UserId);
                    string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");

                    if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
                    {

                        return Page();
                    }

                    //if (Photo != null && Photo.Length > 0)
                    //{

                    //    string connectionString = _configuration["AzureBlobStorageConnectionString"];
                    //    var blobServiceClient = new BlobServiceClient(azureBlobStorageConnectionString);
                    //    var containerClient = blobServiceClient.GetBlobContainerClient("newprhcontainer");
                    //    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                    //    var blobClient = containerClient.GetBlobClient(fileName);
                    //    using (var stream = Photo.OpenReadStream())
                    //    {
                    //        await blobClient.UploadAsync(stream, true);
                    //    }

                    //    var fileUrl = blobClient.Uri.ToString();
                    //    CreateReport.MainPhoto = fileUrl;
                    //}

                    _context.CreateReport.Add(CreateReport);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("New report created successfully.");
                    //return RedirectToPage("./Index");

                    
                    if (Photos != null && Photos.Count > 0)
                    {
                        foreach (var photo in Photos)
                        {
                            string connectionString = _configuration["AzureBlobStorageConnectionString"];                          
                            var blobServiceClient = new BlobServiceClient(azureBlobStorageConnectionString);                           
                            var containerClient = blobServiceClient.GetBlobContainerClient("newprhcontainer");                       
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);                       
                            var blobClient = containerClient.GetBlobClient(fileName);
                            
                            using (var stream = photo.OpenReadStream())
                            {
                                await blobClient.UploadAsync(stream, true);
                            }
                            
                            var fileUrl = blobClient.Uri.ToString();
                           
                            var reportPhoto = new ReportPhoto { ReportId = CreateReport.Id, PhotoUrl = fileUrl };
                            _context.ReportPhoto.Add(reportPhoto);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        _logger.LogError("User not found.");
                        //return RedirectToPage("/Account/Login");

                    }
                    CreateReport.CreatedAt = DateTime.UtcNow;
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

