using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pet_Reunion_Hub.Helper;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public DeleteModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Tribute Tribute { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Tribute == null)
            {
                return NotFound();
            }

            var tribute = await _context.Tribute.FirstOrDefaultAsync(m => m.Id == id);

            if (tribute == null)
            {
                return NotFound();
            }
            else 
            {
                Tribute = tribute;
                tribute.PetName = EncryptionHelper.Decrypt(tribute.PetName);
                tribute.PetType = EncryptionHelper.Decrypt(tribute.PetType);
                tribute.PetBreed = EncryptionHelper.Decrypt(tribute.PetBreed);
                tribute.PetSex = EncryptionHelper.Decrypt(tribute.PetSex);
                tribute.Cause = EncryptionHelper.Decrypt(tribute.Cause);
                tribute.TributeText = EncryptionHelper.Decrypt(tribute.TributeText);
                //tribute.TributePhoto = EncryptionHelper.Decrypt(tribute.TributePhoto);
                //tribute.Visibility = EncryptionHelper.Decrypt(tribute.Visibility);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Tribute == null)
            {
                return NotFound();
            }
            var tribute = await _context.Tribute.FindAsync(id);

            if (tribute != null)
            {
                Tribute = tribute;
                _context.Tribute.Remove(Tribute);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
