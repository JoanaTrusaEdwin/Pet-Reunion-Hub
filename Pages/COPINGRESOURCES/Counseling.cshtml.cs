using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pet_Reunion_Hub.Helper;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.COPINGRESOURCES
{
    [Authorize]
    public class CounselingModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CounselingModel(DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<RESOURCE> RESOURCE { get; set; }


        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);

            // Query for public tributes
            RESOURCE = _context.RESOURCE
                .Where(t => t.TYPE == "Counselling and Therapy")
                .OrderByDescending(t => t.Id)
                .ToList();

            //foreach (var tribute in PublicTributes)
            //{
            //    tribute.PetName = EncryptionHelper.Decrypt(tribute.PetName);
            //    tribute.PetType = EncryptionHelper.Decrypt(tribute.PetType);
            //    tribute.PetBreed = EncryptionHelper.Decrypt(tribute.PetBreed);
            //    tribute.PetSex = EncryptionHelper.Decrypt(tribute.PetSex);
            //    tribute.Cause = EncryptionHelper.Decrypt(tribute.Cause);
            //    tribute.TributeText = EncryptionHelper.Decrypt(tribute.TributeText);
            //}




            Console.WriteLine($"Number of public tributes: {RESOURCE.Count}");

            return Page();
        }
    }
}

