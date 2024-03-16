//using Microsoft.AspNetCore.Mvc;

//namespace Pet_Reunion_Hub.Areas.Identity.Pages
//{
//    public class NEWTESTController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PRHDATALIB.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Pet_Reunion_Hub.Areas.Identity.Pages
{
    public class NEWTESTController : Controller
    {
        private readonly DatabaseContext _context;

        public NEWTESTController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: /NEWTEST/Index
        public async Task<IActionResult> Index()
        {
            var newtests = await _context.NEWTEST.ToListAsync();
            return View(newtests);
        }

        // GET: /NEWTEST/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /NEWTEST/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Newtest")] Newtest newtest)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newtest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newtest);
        }

        // GET: /NEWTEST/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newtest = await _context.NEWTEST.FindAsync(id);
            if (newtest == null)
            {
                return NotFound();
            }
            return View(newtest);
        }

        // POST: /NEWTEST/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Newtest")] Newtest newtest)
        {
            if (id != newtest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newtest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewtestExists(newtest.Id))
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
            return View(newtest);
        }

        // GET: /NEWTEST/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newtest = await _context.NEWTEST
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newtest == null)
            {
                return NotFound();
            }

            return View(newtest);
        }

        // POST: /NEWTEST/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newtest = await _context.NEWTEST.FindAsync(id);
            _context.NEWTEST.Remove(newtest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewtestExists(int id)
        {
            return _context.NEWTEST.Any(e => e.Id == id);
        }
    }
}

