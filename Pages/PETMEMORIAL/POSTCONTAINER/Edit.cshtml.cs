//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.POSTCONTAINER
//{
//    public class EditModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public EditModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        [BindProperty]
//        public CONTAINER CONTAINER { get; set; } = default!;

//        public async Task<IActionResult> OnGetAsync(int? id)
//        {
//            if (id == null || _context.CONTAINER == null)
//            {
//                return NotFound();
//            }

//            var container =  await _context.CONTAINER.FirstOrDefaultAsync(m => m.Id == id);
//            if (container == null)
//            {
//                return NotFound();
//            }
//            CONTAINER = container;
//           ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
//            return Page();
//        }

//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see https://aka.ms/RazorPagesCRUD.
//        public async Task<IActionResult> OnPostAsync()
//        {
//            if (!ModelState.IsValid)
//            {
//                return Page();
//            }

//            _context.Attach(CONTAINER).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!CONTAINERExists(CONTAINER.Id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return RedirectToPage("./Index");
//        }

//        private bool CONTAINERExists(int id)
//        {
//          return (_context.CONTAINER?.Any(e => e.Id == id)).GetValueOrDefault();
//        }
//    }
//}
