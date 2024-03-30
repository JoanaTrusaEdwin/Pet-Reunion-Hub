//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.TEST.TESTBITS
//{
//    public class CreateModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public CreateModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        public IActionResult OnGet()
//        {
//        ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
//            return Page();
//        }

//        [BindProperty]
//        public TESTBIT TESTBIT { get; set; } = default!;
        

//        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
//        public async Task<IActionResult> OnPostAsync()
//        {
//          if (!ModelState.IsValid || _context.TESTBIT_1 == null || TESTBIT == null)
//            {
//                return Page();
//            }

//            _context.TESTBIT_1.Add(TESTBIT);
//            await _context.SaveChangesAsync();

//            return RedirectToPage("./Index");
//        }
//    }
//}
