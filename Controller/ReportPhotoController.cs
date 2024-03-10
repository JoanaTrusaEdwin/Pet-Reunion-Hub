////using Microsoft.AspNetCore.Mvc;

////namespace Pet_Reunion_Hub.Controller
////{
////    public class ReportPhotoController : Controller
////    {
////        public IActionResult Index()
////        {
////            return View();
////        }
////    }
////}

//// ReportPhotosController.cs
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using PRHDATALIB.Models;
//using System.Linq;
//using System.Threading.Tasks;

//namespace YourNamespace.Controllers
//{
//    public class ReportPhotosController : Controller
//    {
//        private readonly DatabaseContext _context;

//        public ReportPhotosController(DatabaseContext context)
//        {
//            _context = context;
//        }

//        // GET: ReportPhotos
//        public async Task<IActionResult> Index()
//        {
//            return View(await _context.ReportPhoto.ToListAsync());
//        }

//        // GET: ReportPhotos/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: ReportPhotos/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("PhotoId,ReportId,PhotoUrl")] ReportPhoto reportPhoto)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(reportPhoto);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(reportPhoto);
//        }

//        // GET: ReportPhotos/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var reportPhoto = await _context.ReportPhoto.FindAsync(id);
//            if (reportPhoto == null)
//            {
//                return NotFound();
//            }
//            return View(reportPhoto);
//        }

//        // POST: ReportPhotos/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("PhotoId,ReportId,PhotoUrl")] ReportPhoto reportPhoto)
//        {
//            if (id != reportPhoto.PhotoId)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(reportPhoto);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ReportPhotoExists(reportPhoto.PhotoId))
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
//            return View(reportPhoto);
//        }

//        // GET: ReportPhotos/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var reportPhoto = await _context.ReportPhoto
//                .FirstOrDefaultAsync(m => m.PhotoId == id);
//            if (reportPhoto == null)
//            {
//                return NotFound();
//            }

//            return View(reportPhoto);
//        }

//        // POST: ReportPhotos/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var reportPhoto = await _context.ReportPhoto.FindAsync(id);
//            _context.ReportPhoto.Remove(reportPhoto);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool ReportPhotoExists(int id)
//        {
//            return _context.ReportPhoto.Any(e => e.PhotoId == id);
//        }
//    }
//}

