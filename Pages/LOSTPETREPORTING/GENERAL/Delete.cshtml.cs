using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.GENERAL_LOCATION
{
    public class DeleteModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public DeleteModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
      public GENERALLOCATION GENERALLOCATION { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.GENERALLOCATION == null)
            {
                return NotFound();
            }

            var generallocation = await _context.GENERALLOCATION.FirstOrDefaultAsync(m => m.Id == id);

            if (generallocation == null)
            {
                return NotFound();
            }
            else 
            {
                GENERALLOCATION = generallocation;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.GENERALLOCATION == null)
            {
                return NotFound();
            }
            var generallocation = await _context.GENERALLOCATION.FindAsync(id);

            if (generallocation != null)
            {
                GENERALLOCATION = generallocation;
                _context.GENERALLOCATION.Remove(GENERALLOCATION);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
