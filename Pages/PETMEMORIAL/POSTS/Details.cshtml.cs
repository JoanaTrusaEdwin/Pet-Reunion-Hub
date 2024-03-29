using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.POSTS
{
    public class DetailsModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;

        public DetailsModel(PRHDATALIB.Models.DatabaseContext context)
        {
            _context = context;
        }

      public Post Post { get; set; } = default!;
      public List<IFormFile> MediaFiles { get; set; } = new List<IFormFile>();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Post == null)
            {
                return NotFound();
            }

             Post = await _context.Post
            .Include(cr => cr.Media) // Include the ReportPhotos collection
            .FirstOrDefaultAsync(m => m.Id == id);

            //var post = await _context.Post.FirstOrDefaultAsync(m => m.Id == id);
            //if (post == null)
            //{
            //    return NotFound();
            //}
            //else 
            //{
            //    Post = post;
            //}
            //return Page
            if (Post == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
