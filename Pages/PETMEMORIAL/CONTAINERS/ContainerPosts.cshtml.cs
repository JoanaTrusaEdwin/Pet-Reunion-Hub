using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;
using System.Collections.Generic;
using System.Linq;


namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.CONTAINERS
{
    [Authorize]
    public class ContainerPostsModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ContainerPostsModel(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public int ContainerId { get; set; }
        

        public IList<Post> Posts { get; set; }
        public IList<Media> Media { get; set; }
        public Dictionary<Post, string> PostEditLinks { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ContainerId = id;
            Posts = await _context.Post
                .Where(p => p.ContainerId == id)
                .Include(p => p.User)
                 .Include(p => p.Media)
                 .ToListAsync();

        //Media = _context.Media
        //    .Where(m => m.PostId == id)
        //    .ToList();
        //        .ToList();

            Media = Posts.SelectMany(p => p.Media).ToList();

            PostEditLinks = new Dictionary<Post, string>();
            foreach (var post in Posts)
            {
                PostEditLinks[post] = Url.Page("/POSTS/Edit", new { id = post.Id });
            }

            return Page();
        }
    }
}
