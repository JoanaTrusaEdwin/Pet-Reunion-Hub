using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
{
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
