using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
{
    public class DeleteModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public DeleteModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

        [BindProperty]
      public CreateReport CreateReport { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.CreateReport == null)
            {
                return NotFound();
            }

            var createreport = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == id);

            if (createreport == null)
            {
                return NotFound();
            }
            else 
            {
                CreateReport = createreport;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.CreateReport == null)
            {
                return NotFound();
            }
            var createreport = await _context.CreateReport.FindAsync(id);

            if (createreport != null)
            {
                CreateReport = createreport;
                _context.CreateReport.Remove(CreateReport);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
