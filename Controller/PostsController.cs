//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;

//namespace Pet_Reunion_Hub.Controllers
//{
//    public class PostsController : Controller
//    {
//        private readonly DatabaseContext _context;

//        public PostsController(DatabaseContext context)
//        {
//            _context = context;
//        }

//        // GET: Posts
//        public async Task<IActionResult> Index()
//        {
//            var databaseContext = _context.Post.Include(p => p.CONTAINER).Include(p => p.Tribute).Include(p => p.User);
//            return View(await databaseContext.ToListAsync());
//        }

//        // GET: Posts/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null || _context.Post == null)
//            {
//                return NotFound();
//            }

//            var post = await _context.Post
//                .Include(p => p.CONTAINER)
//                .Include(p => p.Tribute)
//                .Include(p => p.User)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (post == null)
//            {
//                return NotFound();
//            }

//            return View(post);
//        }

//        // GET: Posts/Create
//        public IActionResult Create()
//        {
//            ViewData["ContainerId"] = new SelectList(_context.CONTAINER, "Id", "UserId");
//            ViewData["TributeId"] = new SelectList(_context.Tribute, "Id", "UserId");
//            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
//            return View();
//        }

//        // POST: Posts/Create
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,Content,IsPublic,TributeId,ContainerId,UserId,CreatedAt")] Post post)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(post);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["ContainerId"] = new SelectList(_context.CONTAINER, "Id", "UserId", post.ContainerId);
//            ViewData["TributeId"] = new SelectList(_context.Tribute, "Id", "UserId", post.TributeId);
//            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
//            return View(post);
//        }
//        // POST: Posts/RemoveImage
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> RemoveImage(int mediaId)
//        {
//            var media = await _context.Media.FindAsync(mediaId);
//            if (media != null)
//            {
//                _context.Media.Remove(media);
//                await _context.SaveChangesAsync();
//                return Ok();
//            }
//            else
//            {
//                return NotFound();
//            }
//        }

//        // GET: Posts/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null || _context.Post == null)
//            {
//                return NotFound();
//            }

//            var post = await _context.Post.FindAsync(id);
//            if (post == null)
//            {
//                return NotFound();
//            }
//            ViewData["ContainerId"] = new SelectList(_context.CONTAINER, "Id", "UserId", post.ContainerId);
//            ViewData["TributeId"] = new SelectList(_context.Tribute, "Id", "UserId", post.TributeId);
//            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
//            return View(post);
//        }

//        // POST: Posts/Edit/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to.
//        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,IsPublic,TributeId,ContainerId,UserId,CreatedAt")] Post post)
//        {
//            if (id != post.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(post);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!PostExists(post.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["ContainerId"] = new SelectList(_context.CONTAINER, "Id", "UserId", post.ContainerId);
//            ViewData["TributeId"] = new SelectList(_context.Tribute, "Id", "UserId", post.TributeId);
//            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", post.UserId);
//            return View(post);
//        }

//        // GET: Posts/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null || _context.Post == null)
//            {
//                return NotFound();
//            }

//            var post = await _context.Post
//                .Include(p => p.CONTAINER)
//                .Include(p => p.Tribute)
//                .Include(p => p.User)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (post == null)
//            {
//                return NotFound();
//            }

//            return View(post);
//        }

//        // POST: Posts/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            if (_context.Post == null)
//            {
//                return Problem("Entity set 'DatabaseContext.Post'  is null.");
//            }
//            var post = await _context.Post.FindAsync(id);
//            if (post != null)
//            {
//                _context.Post.Remove(post);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool PostExists(int id)
//        {
//            return (_context.Post?.Any(e => e.Id == id)).GetValueOrDefault();
//        }
//    }
//}

////using Microsoft.AspNetCore.Mvc;
////using Microsoft.EntityFrameworkCore;
////using PRHDATALIB.Models;
////using Microsoft.AspNetCore.Identity;

////namespace Pet_Reunion_Hub.Controllers
////{
////    [Route("PRHAPI/[controller]")]
////    [ApiController]
////    public class PostsController : Controller
////    {
////        private readonly DatabaseContext _dbContext;
////        private readonly UserManager<IdentityUser> _userManager;

////        public PostsController(DatabaseContext dbContext, UserManager<IdentityUser> userManager)
////        {
////            _dbContext = dbContext;
////            _userManager = userManager;
////        }

////        [HttpPost("{MediaId}")]
////        public async Task<ActionResult> RemoveMedia(int MediaId)
////        {
////            try
////            {
////                if (_dbContext.Media == null)
////                {
////                    return Problem("Media is null");
////                }

////                var mediaToRemove = await _dbContext.Media.FindAsync(MediaId);
////                if (mediaToRemove != null)
////                {
////                    _dbContext.Media.Remove(mediaToRemove);
////                    await _dbContext.SaveChangesAsync();
////                    return NoContent();
////                }
////                return Problem("No such media!");
////            }
////            catch(Exception ex)
////            {
////                return Problem(ex.Message);
////            }
////        }
////    }
////}