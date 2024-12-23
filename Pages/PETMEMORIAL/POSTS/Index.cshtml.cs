﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.POSTS
//{
//    public class IndexModel : PageModel
//    {
//        private readonly PRHDATALIB.Models.DatabaseContext _context;

//        public IndexModel(PRHDATALIB.Models.DatabaseContext context)
//        {
//            _context = context;
//        }

//        public IList<Post> Post { get;set; } = default!;

//        public async Task OnGetAsync()
//        {
//            if (_context.Post != null)
//            {
//                Post = await _context.Post
//                .Include(p => p.Tribute)
//                .Include(p => p.User).ToListAsync();
//            }
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;
using System.Collections.Generic;
using System.Linq;
using Pet_Reunion_Hub.Helper;


namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.POSTS
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly PRHDATALIB.Models.DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(PRHDATALIB.Models.DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Post> Post { get; set; }
        public List<IFormFile> MediaFiles { get; set; } = new List<IFormFile>();
        // = default!;


        public async Task<IActionResult> OnGetAsync()
        {
            //if (_context.CreateReport != null)
            //{
            //    CreateReport = await _context.CreateReport.ToListAsync();
            //}
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

           Post = await _context.Post
                   .Where(n => n.UserId == currentUser.Id)
                   .Include(p => p.Media)
                   .Include(t => t.PostComments)
                     .ThenInclude(c => c.User)
                   .ToListAsync();

            //foreach (var post in Post)
            //{
            //    // Check if the user ID associated with the tribute matches the currently authenticated user
            //    if (post.UserId == currentUser.Id)
            //    {
            //        // Decrypt the tribute properties
            //        post.Title = EncryptionHelper.Decrypt(post.Title);
            //        post.Content = EncryptionHelper.Decrypt(post.Content);

            //    }
            //    else
            //    {
            //        // If the user ID associated with the tribute doesn't match the currently authenticated user, skip decryption
            //        // Alternatively, you can choose to handle this case differently based on your application's requirements
            //    }
            //}

            return Page();
        }
    }
}

