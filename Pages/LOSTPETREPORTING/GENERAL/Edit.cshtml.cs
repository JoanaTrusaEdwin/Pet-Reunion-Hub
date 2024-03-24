using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.GENERAL_LOCATION
{
    public class EditModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public EditModel(PRHDATALIB.Models.DatabaseContext context)
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

            var generallocation =  await _context.GENERALLOCATION.FirstOrDefaultAsync(m => m.Id == id);
            if (generallocation == null)
            {
                return NotFound();
            }
            GENERALLOCATION = generallocation;
           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(GENERALLOCATION).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GENERALLOCATIONExists(GENERALLOCATION.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool GENERALLOCATIONExists(int id)
        {
          return (_context.GENERALLOCATION?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
