using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEWEST
{
    public class DetailsModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public DetailsModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

      public Testing Testing { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Testing == null)
            {
                return NotFound();
            }

            var testing = await _context.Testing.FirstOrDefaultAsync(m => m.Id == id);
            if (testing == null)
            {
                return NotFound();
            }
            else 
            {
                Testing = testing;
            }
            return Page();
        }
    }
}
