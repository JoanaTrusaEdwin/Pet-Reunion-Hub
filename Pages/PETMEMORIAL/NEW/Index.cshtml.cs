using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
{
    public class IndexModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public IndexModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IList<Tribute> Tribute { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Tribute != null)
            {
                Tribute = await _context.Tribute
                .Include(t => t.User).ToListAsync();
            }
        }
    }
}
