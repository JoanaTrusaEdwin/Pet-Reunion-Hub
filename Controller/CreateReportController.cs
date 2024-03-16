//ORIGINAL
//using Microsoft.AspNetCore.Mvc;

//namespace Pet_Reunion_Hub.Controller
//{
//    public class CreateReportController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}
//ORIGINAL

//using Microsoft.AspNetCore.Mvc;
//using PRHDATALIB.Models;
//using System;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore;

//namespace Pet_Reunion_Hub.Controllers
//{
//    public class CreateReportController : Controller
//    {
//        private readonly DatabaseContext _context;

//        public CreateReportController(DatabaseContext context)
//        {
//            _context = context;
//        }

//        // GET: CreateReport
//        public async Task<IActionResult> Index()
//        {
//            var reports = await _context.CreateReport.ToListAsync();
//            return View(reports);
//        }

//        // GET: CreateReport/Details/5
//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var report = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == id);
//            if (report == null)
//            {
//                return NotFound();
//            }

//            return View(report);
//        }

//        // GET: CreateReport/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: CreateReport/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create([Bind("Id,PetName,PetBreed,PetGender,DateOfBirth,PetMicrochipID,LastSeenLocation,LastSeenTime,MainPhoto,ContactInformation")] CreateReport report)
//        {
//            if (ModelState.IsValid)
//            {
//                _context.Add(report);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            return View(report);
//        }

//        // GET: CreateReport/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var report = await _context.CreateReport.FindAsync(id);
//            if (report == null)
//            {
//                return NotFound();
//            }
//            return View(report);
//        }

//        // POST: CreateReport/Edit/5
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,PetName,PetBreed,PetGender,DateOfBirth,PetMicrochipID,LastSeenLocation,LastSeenTime,MainPhoto,ContactInformation")] CreateReport report)
//        {
//            if (id != report.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(report);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ReportExists(report.Id))
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
//            return View(report);
//        }

//        // GET: CreateReport/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var report = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == id);
//            if (report == null)
//            {
//                return NotFound();
//            }

//            return View(report);
//        }

//        // POST: CreateReport/Delete/5
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var report = await _context.CreateReport.FindAsync(id);
//            _context.CreateReport.Remove(report);
//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool ReportExists(int id)
//        {
//            return _context.CreateReport.Any(e => e.Id == id);
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using PRHDATALIB.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Pet_Reunion_Hub.Controllers
{
    public class CreateReportController : Controller
    {
        private readonly DatabaseContext _context;

        public CreateReportController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: CreateReport
        public async Task<IActionResult> Index()
        {
            var reports = await _context.CreateReport.ToListAsync();
            return View(reports);
        }

        // GET: CreateReport/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // GET: CreateReport/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CreateReport/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetName,PetBreed,PetGender,DateOfBirth,PetMicrochipID,LastSeenLocation,LastSeenTime,MainPhoto,ContactInformation")] CreateReport report)
        {
            if (ModelState.IsValid)
            {

                report.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                _context.Add(report);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // GET: CreateReport/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.CreateReport.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }

        // POST: CreateReport/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PetName,PetBreed,PetGender,DateOfBirth,PetMicrochipID,LastSeenLocation,LastSeenTime,MainPhoto,ContactInformation")] CreateReport report)
        {
            if (id != report.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(report);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReportExists(report.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(report);
        }

        // GET: CreateReport/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var report = await _context.CreateReport.FirstOrDefaultAsync(m => m.Id == id);
            if (report == null)
            {
                return NotFound();
            }

            return View(report);
        }

        // POST: CreateReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var report = await _context.CreateReport.FindAsync(id);
            _context.CreateReport.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReportExists(int id)
        {
            return _context.CreateReport.Any(e => e.Id == id);
        }
    }
}
