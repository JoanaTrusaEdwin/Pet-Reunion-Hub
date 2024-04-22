//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Pet_Reunion_Hub.Helper;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
//{
//    public class CommunityMemorialsModel : PageModel
//    {
//        public void OnGet()
//        {
//        }
//    }
//}


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

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
{
    [Authorize]
    public class CommunityMemorialsModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CommunityMemorialsModel(DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Tribute> PublicTributes { get; set; }


        public IActionResult OnGet()
        {
            var userId = _userManager.GetUserId(User);

            // Query for public tributes
            PublicTributes = _context.Tribute
                .Where(t => t.Visibility == "Public")
                .OrderByDescending(t => t.Id)
                .ToList();

            foreach (var tribute in PublicTributes)
            {
                tribute.PetName = EncryptionHelper.Decrypt(tribute.PetName);
                tribute.PetType = EncryptionHelper.Decrypt(tribute.PetType);
                tribute.PetBreed = EncryptionHelper.Decrypt(tribute.PetBreed);
                tribute.PetSex = EncryptionHelper.Decrypt(tribute.PetSex);
                tribute.Cause = EncryptionHelper.Decrypt(tribute.Cause);
                tribute.TributeText = EncryptionHelper.Decrypt(tribute.TributeText);
            }




            Console.WriteLine($"Number of public tributes: {PublicTributes.Count}");

            return Page();
        }
    }
}