using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.USER
{
    public class DetailsModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public DetailsModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

      public Newtest Newtest { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.NEWTEST == null)
            {
                return NotFound();
            }

            var newtest = await _context.NEWTEST.FirstOrDefaultAsync(m => m.Id == id);
            if (newtest == null)
            {
                return NotFound();
            }
            else 
            {
                Newtest = newtest;
            }
            return Page();
        }
    }
}
