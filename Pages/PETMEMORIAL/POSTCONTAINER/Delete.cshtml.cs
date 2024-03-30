//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.POSTCONTAINER
//{
//    public class DeleteModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public DeleteModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//      public CONTAINER CONTAINER { get; set; } = default!;

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null || _context.CONTAINER == null)
//            {
//                return NotFound();
//            }

//            var container = await _context.CONTAINER.FirstOrDefaultAsync(m => m.Id == id);

//            if (container == null)
//            {
//                return NotFound();
//            }
//            else 
//            {
//                CONTAINER = container;
//            }
//            return Page();
//        }

//        public async Task<IActionResult> OnPostAsync(int? id)
//        {
//            if (id == null || _context.CONTAINER == null)
//            {
//                return NotFound();
//            }
//            var container = await _context.CONTAINER.FindAsync(id);

//            if (container != null)
//            {
//                CONTAINER = container;
//                _context.CONTAINER.Remove(CONTAINER);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToPage("./Index");
//        }
//    }
//}
