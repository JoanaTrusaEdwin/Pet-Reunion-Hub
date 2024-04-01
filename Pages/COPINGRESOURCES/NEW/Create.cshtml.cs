//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.COPINGRESOURCES.NEW
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
//        public RESOURCE RESOURCE { get; set; } = default!;


//        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
//        public async Task<IActionResult> OnPostAsync()
//        {
//          if (!ModelState.IsValid || _context.RESOURCE == null || RESOURCE == null)
//            {
//                return Page();
//            }

//            _context.RESOURCE.Add(RESOURCE);
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

namespace Pet_Reunion_Hub.Pages.COPINGRESOURCES.NEW
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
            RESOURCE = new RESOURCE { UserId = _userManager.GetUserId(User) };
            return Page();
        }

        [BindProperty]
        public RESOURCE RESOURCE { get; set; } = new RESOURCE();

        //[BindProperty]
        //public IFormFile? photo { get; set;
        [BindProperty]
        public IFormFile? UploadedFile { get; set; }



        public async Task<IActionResult> OnPostAsync(IFormFile UploadedFile)
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

                    RESOURCE.UserId = userId;
                    _logger.LogInformation("Assigned User ID: {UserId}", RESOURCE.UserId);
                    string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");

                    if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
                    {

                        return Page();
                    }
                    if (UploadedFile != null && UploadedFile.Length > 0)
                    {

                        string connectionString = _configuration["AzureBlobStorageConnectionString"];
                        var blobServiceClient = new BlobServiceClient(azureBlobStorageConnectionString);
                        var containerClient = blobServiceClient.GetBlobContainerClient("resources");
                        //var fileName = Guid.NewGuid().ToString() + Path.GetExtension(UploadedFile.FileName
                        var fileName = $"{Guid.NewGuid()}_{DateTime.UtcNow.ToString("yyyyMMddHHmmss")}{Path.GetExtension(UploadedFile.FileName)}";
                        var blobClient = containerClient.GetBlobClient(fileName);
                        using (var stream = UploadedFile.OpenReadStream())
                        {
                            await blobClient.UploadAsync(stream, true);
                        }

                        var fileUrl = blobClient.Uri.ToString();
                        RESOURCE.FILEURL = fileUrl;
                        //var resource = new RESOURCE { FILEURL = fileUrl, FORMAT = GetFORMAT(Path.GetExtension(UploadedFile.FileName)) };
                        RESOURCE.FORMAT = GetFORMAT(Path.GetExtension(UploadedFile.FileName));
                    }

                  

                    _context.RESOURCE.Add(RESOURCE);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("New resource created successfully.");
                    //return RedirectToPage("./Index");
                    RESOURCE.CreatedAt = DateTime.UtcNow;
                }
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating new resource:");
                //return RedirectToPage("/Error");
                throw;
            }
        }
        private string GetFORMAT(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                case ".bmp":
                case ".tiff":
                case ".tif":
                case ".webp":
                    return "Image";

                case ".mp4":
                case ".avi":
                case ".mov":
                case ".wmv":
                case ".flv":
                case ".mkv":
                case ".webm":
                    return "Video";

                case ".mp3":
                case ".wav":
                case ".aiff":
                case ".aif":
                case ".flac":
                case ".aac":
                case ".ogg":
                    return "Audio";

                case ".pdf":
                    return "PDF Document";
                case ".doc":
                case ".docx":
                    return "Microsoft Word Document";
                case ".xls":
                case ".xlsx":
                    return "Microsoft Excel Spreadsheet";
                case ".ppt":
                case ".pptx":
                    return "Microsoft PowerPoint Presentation";
                case ".txt":
                    return "Plain Text File";
                case ".rtf":
                    return "Rich Text Format";
                case ".csv":
                    return "Comma-Separated Values (CSV)";

                default:
                    return "Other";
            }
        }
}
}
