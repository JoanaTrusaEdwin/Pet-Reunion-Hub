using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.COPINGRESOURCES.NEW
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
      public RESOURCE RESOURCE { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.RESOURCE == null)
            {
                return NotFound();
            }

            var resource = await _context.RESOURCE.FirstOrDefaultAsync(m => m.Id == id);

            if (resource == null)
            {
                return NotFound();
            }
            else 
            {
                RESOURCE = resource;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.RESOURCE == null)
            {
                return NotFound();
            }
            var resource = await _context.RESOURCE.FindAsync(id);

            if (resource != null)
            {
                //var fav = await _context.RESOURCEFAVE.Where(rp => rp.RESOURCEId == resource.Id).ToListAsync();
                //foreach (var saved in fav)
                //{
                //    _context.Remove(saved);
                //}
                RESOURCE = resource;
                _context.RESOURCE.Remove(RESOURCE);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
