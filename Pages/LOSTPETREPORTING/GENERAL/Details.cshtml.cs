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
    public class DetailsModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public DetailsModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

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
    }
}
