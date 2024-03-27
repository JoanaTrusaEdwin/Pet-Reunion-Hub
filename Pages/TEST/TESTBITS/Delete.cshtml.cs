using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.TEST.TESTBITS
{
    public class DeleteModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public DeleteModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
      public TESTBIT TESTBIT { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.TESTBIT_1 == null)
            {
                return NotFound();
            }

            var testbit = await _context.TESTBIT_1.FirstOrDefaultAsync(m => m.Id == id);

            if (testbit == null)
            {
                return NotFound();
            }
            else 
            {
                TESTBIT = testbit;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.TESTBIT_1 == null)
            {
                return NotFound();
            }
            var testbit = await _context.TESTBIT_1.FindAsync(id);

            if (testbit != null)
            {
                TESTBIT = testbit;
                _context.TESTBIT_1.Remove(TESTBIT);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
