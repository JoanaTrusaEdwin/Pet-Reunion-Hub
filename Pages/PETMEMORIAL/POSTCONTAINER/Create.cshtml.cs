﻿////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Threading.Tasks;
////using Microsoft.AspNetCore.Mvc;
////using Microsoft.AspNetCore.Mvc.RazorPages;
////using Microsoft.AspNetCore.Mvc.Rendering;
////using PRHDATALIB.Models;

////namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.POSTCONTAINER
////{
////    public class CreateModel : PageModel
////    {
////        private readonly PRHDATALIB.Models.DatabaseContext _context;

////        public CreateModel(PRHDATALIB.Models.DatabaseContext context)
////        {
////            _context = context;
////        }

////        public IActionResult OnGet()
////        {
////        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
////            return Page();
////        }

////        [BindProperty]
////        public CONTAINER CONTAINER { get; set; } = default!;


////        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
////        public async Task<IActionResult> OnPostAsync()
////        {
////          if (!ModelState.IsValid || _context.CONTAINER == null || CONTAINER == null)
////            {
////                return Page();
////            }

////            _context.CONTAINER.Add(CONTAINER);
////            await _context.SaveChangesAsync();

////            return RedirectToPage("./Index");
////        }
////    }
////}
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.Extensions.Configuration;
//using PRHDATALIB.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.Extensions.Logging;
//using System.Configuration;
//using Azure.Storage.Blobs;
//using Microsoft.AspNetCore.Authorization;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using NuGet.Protocol.Plugins;
//using System.Security.Principal;
//using Pet_Reunion_Hub.Helper;

//namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.POSTCONTAINER
//{
//    [Authorize]
//    public class CreateModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;
//        private readonly IConfiguration _configuration;
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly SignInManager<IdentityUser> _signInManager;
//        private readonly ILogger<CreateModel> _logger;

//        public CreateModel(ILogger<CreateModel> logger, PRHDATALIB.Models.DatabaseContext context, IConfiguration configuration,
//                           UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
//        {
//            _logger = logger;
//            _context = context;
//            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
//            _userManager = userManager;
//            _signInManager = signInManager;
//        }

//        [Authorize]
//        public IActionResult OnGet()
//        {
//            CONTAINER = new CONTAINER { UserId = _userManager.GetUserId(User) };
//            return Page();
//        }

//        [BindProperty]
//        public CONTAINER CONTAINER { get; set; } = new CONTAINER();

//        [BindProperty]
//        public IFormFileCollection Post { get; set; }

//        public async Task<IActionResult> OnPostAsync()
//        {
//            try
//            {
//                var user = await _userManager.GetUserAsync(User);
//                if (user != null)
//                {
//                    string userId = user.Id;
//                    _logger.LogInformation("User ID: {UserId}", userId);

//                    if (string.IsNullOrEmpty(userId))
//                    {
//                        _logger.LogWarning("User ID is null or empty.");
//                        //return RedirectToPage("/Account/Login");
//                        //return RedirectToPage("/Identity/Account/Login");

//                    }

//                    CONTAINER.UserId = userId;
//                    _logger.LogInformation("Assigned User ID: {UserId}", CONTAINER.UserId);
//                    string azureBlobStorageConnectionString = _configuration.GetConnectionString("AzureBlobStorageConnectionString");

//                    if (string.IsNullOrEmpty(azureBlobStorageConnectionString))
//                    {
//                        _logger.LogError("Azure Blob Storage connection string is null or empty.");
//                        return Page();
//                    }

//                    _context.Post.Add(CONTAINER);
//                    await _context.SaveChangesAsync();
//                    _logger.LogInformation("New post created successfully.");
//                    //return RedirectToPage("./Index");

//                    if (MediaFiles != null && MediaFiles.Count > 0)
//                    {
//                        foreach (var file in MediaFiles)
//                        {
//                            string connectionString = _configuration["AzureBlobStorageConnectionString"];
//                            var blobServiceClient = new BlobServiceClient(azureBlobStorageConnectionString);
//                            var containerClient = blobServiceClient.GetBlobContainerClient("posts");
//                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
//                            var blobClient = containerClient.GetBlobClient(fileName);

//                            using (var stream = file.OpenReadStream())
//                            {
//                                await blobClient.UploadAsync(stream, true);
//                            }

//                            var fileUrl = blobClient.Uri.ToString();

//                            var media = new Media { PostId = Post.Id, MediaUrl = fileUrl, MediaType = GetMediaType(Path.GetExtension(file.FileName)) };
//                            _context.Media.Add(media);
//                            //await _context.SaveChangesAsync();
//                        }
//                        await _context.SaveChangesAsync();
//                    }
//                    else
//                    {
//                        _logger.LogError("User not found.");
//                        //return RedirectToPage("/Account/Login");

//                    }
//                    //_context.Post.Add(Post);
//                    //await _context.SaveChangesAsync();
//                    //_logger.LogInformation("New post created successfully.");
//                    ////return RedirectToPage("./Index");
//                }
//                return RedirectToPage("./Index");
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error creating new report:");
//                //return RedirectToPage("/Error");
//                throw;
//            }
//        }

//        private string GetMediaType(string fileExtension)
//        {
//            switch (fileExtension.ToLower())
//            {
//                case ".jpg":
//                case ".jpeg":
//                case ".png":
//                case ".gif":
//                    return "Image";
//                case ".mp4":
//                case ".avi":
//                case ".mov":
//                    return "Video";
//                case ".mp3":
//                case ".wav":
//                    return "Audio";
//                default:
//                    return "Other";
//            }
//        }
//    }
//}

