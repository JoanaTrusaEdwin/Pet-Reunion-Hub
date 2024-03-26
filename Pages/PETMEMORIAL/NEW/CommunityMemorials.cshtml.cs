////using Microsoft.AspNetCore.Mvc;
////using Microsoft.AspNetCore.Mvc.RazorPages;



//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
//{
//    [Authorize]
//    public class CommunityMemorialsModel : PageModel
//    {
//        private readonly DatabaseContext _context;

//        public CommunityMemorialsModel(DatabaseContext context)
//        {
//            _context = context;
//        }

//        public List<Tribute>? PublicTributes { get; set; }
//        public Dictionary<int, List<Comment>> TributeComments { get; set; }
//        public Dictionary<int, int> TributeHeartsCount { get; set; }
//        public Dictionary<int, bool> UserHeartsStatus { get; set; }

//        public async Task<IActionResult> OnGetAsync()
//        {
//            // Fetching public tributes
//            PublicTributes = await _context.Tribute.Where(t => t.IsPublic==true).ToListAsync();

//            // Fetching comments for public tributes
//            TributeComments = new Dictionary<int, List<Comment>>();
//            foreach (var tribute in PublicTributes)
//            {
//                var comments = await _context.Comment.Where(c => c.TributeId == tribute.Id).ToListAsync();
//                TributeComments.Add(tribute.Id, comments);
//            }

//            // Fetching hearts count for public tributes
//            TributeHeartsCount = new Dictionary<int, int>();
//            foreach (var tribute in PublicTributes)
//            {
//                var heartsCount = _context.Heart.Count(h => h.TributeId == tribute.Id);
//                TributeHeartsCount.Add(tribute.Id, heartsCount);
//            }

//            // Fetching user hearts status for public tributes
//            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
//            UserHeartsStatus = new Dictionary<int, bool>();
//            foreach (var tribute in PublicTributes)
//            {
//                var userHeart = _context.Heart.FirstOrDefault(h => h.TributeId == tribute.Id && h.UserId == userId);
//                UserHeartsStatus.Add(tribute.Id, userHeart != null);
//            }
//            return Page();
//        }

//        public async Task<IActionResult> OnPostHeart(int tributeId)
//        {
//            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

//            var existingHeart = _context.Heart.FirstOrDefault(h => h.TributeId == tributeId && h.UserId == userId);
//            if (existingHeart == null)
//            {
//                var newHeart = new Heart { TributeId = tributeId, UserId = userId };
//                _context.Heart.Add(newHeart);
//                await _context.SaveChangesAsync();
//            }
//            else
//            {
//                _context.Heart.Remove(existingHeart);
//                await _context.SaveChangesAsync();
//            }
//            return RedirectToPage();
//        }

//        public async Task<IActionResult> OnPostComment(int tributeId, string content)
//        {
//            try
//            {
//                if (PublicTributes == null)
//                {
//                    return NotFound();
//                }

//                var tribute = PublicTributes.FirstOrDefault(t => t.Id == tributeId);
//                if (tribute == null)
//                {
//                    // Handle the missing tribute object (e.g. return a not found error)
//                    return NotFound();
//                }
//                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
//                var newComment = new Comment { TributeId = tributeId, UserId = userId, Content = content };
//                _context.Comment.Add(newComment);
//                await _context.SaveChangesAsync();


//                TributeComments[tributeId] = await _context.Comment
//                    .Where(c => c.TributeId == tributeId)
//                    .Include(c => c.User)
//                    .ToListAsync();

//                tribute = await _context.Tribute.FindAsync(tributeId);

//                if (tribute == null)
//                {
//                    return NotFound();
//                }



//                return Page();
//            }
//            catch (Exception ex)
//            {
//                // Log or handle the exception
//                return Page();
//            }

//        }
//    }
//}

//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
//{
//    [Authorize]
//    public class CommunityMemorialsModel : PageModel
//    {
//        private readonly DatabaseContext _context;
//        private readonly UserManager<IdentityUser> _userManager;

//        public CommunityMemorialsModel(DatabaseContext context, UserManager<IdentityUser> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//        }

//        public List<Tribute> PublicTributes { get; set; }
//        public Dictionary<int, List<Comment>> TributeComments { get; set; }

//        public IActionResult OnGet()
//        {
//            PublicTributes = _context.Tribute.Where(t => t.IsPublic==true).ToList();
//            TributeComments = new Dictionary<int, List<Comment>>();

//            foreach (var tribute in PublicTributes)
//            {

//                TributeComments[tribute.Id] = new List<Comment>();
//                // Retrieve comments for the tribute
//                var comments = _context.Comment
//                    .Where(c => c.TributeId == tribute.Id)
//                    .Include(c => c.User)
//                    .ToList();

//                if (comments == null)
//                {

//                    comments = new List<Comment>();
//                }


//                    // Add comments to the dictionary
//                TributeComments[tribute.Id] = comments;


//            }

//            return Page();
//        }

//        public IActionResult OnPost(int tributeId, string comment) 
//        {

//            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//            var tribute = _context.Tribute.FirstOrDefault(t => t.Id == tributeId);



//            var newComment = new Comment
//            {
//                Content = comment,
//                TributeId = tributeId,
//                UserId = userId,

//            };


//            _context.Comment.Add(newComment);
//            _context.SaveChanges();


//            return RedirectToPage();
//        }
//    }
//}

using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;

namespace Pet_Reunion_Hub.Pages.PETMEMORIAL.NEW
{
    [Authorize]
    public class CommunityMemorialsModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public CommunityMemorialsModel(DatabaseContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public List<Tribute> PublicTributes { get; set; }
       

        public IActionResult OnGet()
        {
            PublicTributes = _context.Tribute.Where(t => t.IsPublic == true).ToList();
            return Page();
        }
    }
}

