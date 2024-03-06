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

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
{
    public class EditModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public EditModel(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        [BindProperty]
        public CreateReport CreateReport { get; set; }

        [BindProperty]
        public IFormFile Photo { get; set; }

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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingReport = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == CreateReport.Id);

            if (existingReport == null)
            {
                return NotFound();
            }

            existingReport.PetName = CreateReport.PetName;
            existingReport.PetBreed = CreateReport.PetBreed;
            // Update other properties similarly...

            try
            {
                if (Photo != null)
                {
                    Console.WriteLine($"Uploaded file: {Photo.FileName}, Size: {Photo.Length} bytes");

                    string fileUrl = await UploadPhotoToBlobStorage(Photo);
                    existingReport.MainPhoto = fileUrl;
                }
                else
                {
                    Console.WriteLine("No file uploaded");
                }

                _context.CreateReport.Update(existingReport);
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Log any exceptions that occur during the photo upload process
                Console.WriteLine($"An error occurred: {ex.Message}");
                return Page();
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
