using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pet_Reunion_Hub.Helper;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Tribute> Tribute { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            //if (_context.Tribute != null)
            //{
            //    Tribute = await _context.Tribute
            //    .Include(t => t.User).ToListAsync();
            //}

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            Tribute = await _context.Tribute
                   .Where(n => n.UserId == currentUser.Id)
                   .ToListAsync();

            //foreach (var tribute in Tribute)
            //{
            //    // Check if the user ID associated with the tribute matches the currently authenticated user
            //    if (tribute.UserId == currentUser.Id)
            //    {
            //        // Decrypt the tribute properties
            //        tribute.PetName = EncryptionHelper.Decrypt(tribute.PetName);
            //        tribute.PetType = EncryptionHelper.Decrypt(tribute.PetType);
            //        tribute.PetBreed = EncryptionHelper.Decrypt(tribute.PetBreed);
            //        tribute.PetSex = EncryptionHelper.Decrypt(tribute.PetSex);
            //        tribute.Cause = EncryptionHelper.Decrypt(tribute.Cause);
            //        tribute.TributeText = EncryptionHelper.Decrypt(tribute.TributeText);
            //        //tribute.TributePhoto = EncryptionHelper.Decrypt(tribute.TributePhoto);
            //        //tribute.Visibility = EncryptionHelper.Decrypt(tribute.Visibility);
            //        //tribute. = EncryptionHelper.Decrypt(tribute.);
            //        //tribute. = EncryptionHelper.Decrypt(tribute.);
            //        // Decrypt other properties as needed
            //    }
            //    else
            //    {
            //        // If the user ID associated with the tribute doesn't match the currently authenticated user, skip decryption
            //        // Alternatively, you can choose to handle this case differently based on your application's requirements
            //    }
            //}


            return Page();
        }
    }
}
