//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.POSTS
//{
//    public class EditModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public EditModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//        public Post Post { get; set; } = default!;

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null || _context.Post == null)
//            {
//                return NotFound();
//            }

//            var post =  await _context.Post.FirstOrDefaultAsync(m => m.Id == id);
//            if (post == null)
//            {
//                return NotFound();
//            }
//            Post = post;
//           ViewData["TributeId"] = new SelectList(_context.Tribute, "Id", "UserId");
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

//            _context.Attach(Post).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!PostExists(Post.Id))
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

//        private bool PostExists(int id)
//        {
//          return (_context.Post?.Any(e => e.Id == id)).GetValueOrDefault();
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
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.Threading.Tasks;
//using Microsoft.Extensions.Logging;
//using Microsoft.AspNetCore.Authorization;

//namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.POSTS
//{
//    public class EditModel : PageModel
//    {
//        private readonly DatabaseContext _context;
//        private readonly IConfiguration _configuration;
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly ILogger<EditModel> _logger;
//        private readonly SignInManager<IdentityUser> _signInManager;

//        public EditModel(ILogger<EditModel> logger, DatabaseContext context, SignInManager<IdentityUser> signInManager, IConfiguration configuration, UserManager<IdentityUser> userManager)
//        {
//            _logger = logger;
//            _context = context;
//            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
//            _userManager = userManager;
//            _signInManager = signInManager;
//        }

//        [BindProperty]
//        public Post Post { get; set; }

//        [BindProperty]
//        public List<IFormFile> MediaFiles { get; set; }

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }


//            Post = await _context.Post
//                                .Include(cr => cr.Media) // Include the ReportPhotos collection
//                                .FirstOrDefaultAsync(m => m.Id == id);

//            if (Post == null)
//            {
//                return NotFound();
//            }

//            return Page();
//        }


//        public async Task<IActionResult> OnPostAsync()
//        {

//            try
//            {
//                //if (!ModelState.IsValid)
//                //{
//                //    //If model state is invalid, return the page with validation errors
//                //    return Page();
//                //}

//                var user = await _userManager.GetUserAsync(User);
//                if (user != null)
//                {
//                    string userId = user.Id;
//                    _logger.LogInformation("User ID: {UserId}", userId);

//                    if (string.IsNullOrEmpty(userId))
//                    {
//                        _logger.LogWarning("User ID is null or empty.");
//                        return RedirectToPage("/Account/Login");
//                    }

//                   Post.UserId = userId;
//                    _logger.LogInformation("Assigned User ID: {UserId}", Post.UserId);
//                    string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");

//                    if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
//                    {
//                        return Page();
//                    }

//                    if (Post.Id == 0)
//                    {

//                        //_context.CreateReport.Add(CreateReport);
//                        Post toBeUpdated = await _context.Post.FindAsync(Post.Id);
//                        if (toBeUpdated != null)
//                        {
//                            //Console.WriteLine($"!!!!!!!!!!!!!!!!!!!!tobeupdated: ID: {toBeUpdated.Id}");
//                            //await TryUpdateModelAsync<CreateReport>(toBeUpdated, "CreateReport", c => c.PetName, c => c.PetBreed, c => c.PetMicrochipID, c => c.LastSeenLocation, c => c.LastSeenTime, c => c.DateOfBirth, c => c.Age, c => c.GenLoc, c => c.AdditionalDescription);
//                        }
//                        _context.CreateReport.Update(Post);
//                        _context.SaveChanges();
//                    }
//                    else
//                    {
//                        var existingPost = await _context.Post.FirstOrDefaultAsync(m => m.Id == Post.Id);
//                        if (existingPost == null)
//                        {
//                            return NotFound();
//                        }



//                        existingPost.Content = Post.Content;
//                        existingPost.IsPublic = Post.IsPublic;
//                        existingPost.TributeId = Post.TributeId;


//                        await UpdateOrCreateReportPhotos(existingPost, MediaFiles);
//                        _context.Entry(existingPost).State = EntityState.Modified;


//                    }

//                    await _context.SaveChangesAsync();
//                    _logger.LogInformation("Report saved successfully.");

//                    return RedirectToPage("./Index");
//                }
//                else
//                {
//                    _logger.LogError("User not found.");
//                    return RedirectToPage("/Account/Login");
//                }
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error saving report:");

//                throw;
//            }


//        }


//        private async Task UpdateOrCreateReportPhotos(CreateReport existingPost, List<IFormFile> MediaFiles)
//        {
//            // Clear existing photos if any
//            existingPost.ReportPhotos.Clear();

//            // Add new photos
//            foreach (var file in MediaFiles)
//            {
//                string fileUrl = await UploadPhotoToBlobStorage(file);

//                // Create a new ReportPhoto object and associate it with the report
//                var reportPhoto = new ReportPhoto { PhotoUrl = fileUrl };
//                existingPost.ReportPhotos.Add(reportPhoto);
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
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pet_Reunion_Hub.Helper;
using PRHDATALIB.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.POSTS
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<EditModel> _logger;
        private readonly SignInManager<IdentityUser> _signInManager;

        public EditModel(ILogger<EditModel> logger, DatabaseContext context, IConfiguration configuration,
                           UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public Post Post { get; set; }

        [BindProperty]
        public List<IFormFile> MediaFiles { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Post = await _context.Post.FirstOrDefaultAsync(m => m.Id == id);

            Post = await _context.Post
                              .Include(cr => cr.Media) // Include the ReportPhotos collection
                              .FirstOrDefaultAsync(m => m.Id == id);

            if (Post == null)
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

                    Post.UserId = userId;
                    _logger.LogInformation("Assigned User ID: {UserId}", Post.UserId);
                    string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");

                    if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
                    {
                        _logger.LogError("Azure Blob Storage connection string is null or empty.");
                        return Page();
                    }

                    if (Post.Id == 0)
                    {
                        _context.Post.Add(Post);
                    }

                    else
                    {
                        var existingPost = await _context.Post.FirstOrDefaultAsync(m => m.Id == Post.Id);
                        if (existingPost == null)
                        {
                            return NotFound();
                        }

                        existingPost.Content = Post.Content;
                        existingPost.TributeId = Post.TributeId;

                        await UpdateOrCreateMedia(existingPost, MediaFiles);
                        _context.Entry(existingPost).State = EntityState.Modified;


                    }

                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Post saved successfully.");

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
                _logger.LogError(ex, "Error saving post:");
                throw;
            }
        }

        private async Task UpdateOrCreateMedia(Post existingPost,  List<IFormFile> mediaFiles)
        {
            // Clear existing media if any
            existingPost.Media.Clear();

            // Add new media
            if (mediaFiles != null && mediaFiles.Count > 0)
            {
                foreach (var file in mediaFiles)
                {
                    string fileUrl = await UploadMediaToBlobStorage(file);

                    // Create a new Media object and associate it with the post
                    var media = new Media { MediaUrl = fileUrl, MediaType = GetMediaType(Path.GetExtension(file.FileName)) };
                    existingPost.Media.Add(media);
                }
            }
        }

        private async Task<string> UploadMediaToBlobStorage(IFormFile mediaFile)
        {
            string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");
            if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
            {
                return null;
            }

            string containerName = "posts";

            var blobServiceClient = new BlobServiceClient(azureBlobStorageConnectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(mediaFile.FileName);
            var blobClient = containerClient.GetBlobClient(fileName);

            using (var stream = mediaFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.ToString();
        }

        private string GetMediaType(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                case ".png":
                case ".gif":
                    return "Image";
                case ".mp4":
                case ".avi":
                case ".mov":
                    return "Video";
                case ".mp3":
                case ".wav":
                    return "Audio";
                default:
                    return "Other";
            }
        }
    }
}





