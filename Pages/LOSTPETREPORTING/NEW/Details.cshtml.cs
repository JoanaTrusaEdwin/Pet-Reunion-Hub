//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
//{
//    public class DetailsModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public DetailsModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//      public CreateReport CreateReport { get; set; } = default!; 

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null || _context.CreateReport == null)
//            {
//                return NotFound();
//            }

//            var createreport = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == id);
//            if (createreport == null)
//            {
//                return NotFound();
//            }
//            else 
//            {
//                CreateReport = createreport;
//            }
//            return Page();
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.LOSTPETREPORTING.NEW
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public DetailsModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

        public CreateReport CreateReport { get; set; } = default!;
        public List<IFormFile> Photos { get; set; } = new List<IFormFile>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            CreateReport = await _context.CreateReport
                .Include(cr => cr.ReportPhotos) // Include the ReportPhotos collection
                .FirstOrDefaultAsync(m => m.Id == id);

            if (CreateReport == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}

