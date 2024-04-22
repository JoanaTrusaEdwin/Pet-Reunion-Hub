using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pet_Reunion_Hub.Helper;
using PRHDATALIB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
{ 

    [Authorize]
    public class NEWDETAILModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public NEWDETAILModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

        public Tribute Tribute { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Tribute == null)
            {
                return NotFound();
            }

            var tribute = await _context.Tribute.Include(t => t.Comments).ThenInclude(c => c.User).FirstOrDefaultAsync(m => m.Id == id);
            if (tribute == null)
            {
                return NotFound();
            }
            Tribute = tribute;
            return Page();
        }
    }
}
