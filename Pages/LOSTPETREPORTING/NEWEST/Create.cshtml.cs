using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEWEST
{
    public class CreateModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public CreateModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Testing Testing { get; set; } = new Testing ();
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //if (!ModelState.IsValid || _context.Testing == null || Testing == null)
            if (!ModelState.IsValid || Testing == null)
            {
                return Page();
            }

            _context.Testing.Add(Testing);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
