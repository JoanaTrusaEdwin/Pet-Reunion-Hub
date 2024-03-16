using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEWEST
{
    public class EditModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public EditModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Testing Testing { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Testing == null)
            {
                return NotFound();
            }

            var testing =  await _context.Testing.FirstOrDefaultAsync(m => m.Id == id);
            if (testing == null)
            {
                return NotFound();
            }
            Testing = testing;
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

            _context.Attach(Testing).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TestingExists(Testing.Id))
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

        private bool TestingExists(int id)
        {
          return (_context.Testing?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
