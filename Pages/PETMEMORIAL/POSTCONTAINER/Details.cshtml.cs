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
//    public class DetailsModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public DetailsModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

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
//    }
//}
