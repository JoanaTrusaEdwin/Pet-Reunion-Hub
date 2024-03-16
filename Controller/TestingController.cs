//using Microsoft.AspNetCore.Mvc;

//namespace Pet_Reunion_Hub.Controller
//{
//    public class TestingController : Controller
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

namespace Pet_Reunion_Hub.Controllers
{
    public class TestingController : Controller
    {
        private readonly DatabaseContext _context;

        public TestingController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: Testing
        public async Task<IActionResult> Index()
        {
            var testingItems = await _context.Testing.ToListAsync();
            return View(testingItems);
        }

        // GET: Testing/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testingItem = await _context.Testing.FirstOrDefaultAsync(m => m.Id == id);
            if (testingItem == null)
            {
                return NotFound();
            }

            return View(testingItem);
        }

        // GET: Testing/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Testing/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Test")] Testing testingItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(testingItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(testingItem);
        }

        // GET: Testing/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testingItem = await _context.Testing.FindAsync(id);
            if (testingItem == null)
            {
                return NotFound();
            }
            return View(testingItem);
        }

        // POST: Testing/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Test")] Testing testingItem)
        {
            if (id != testingItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(testingItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TestingItemExists(testingItem.Id))
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
            return View(testingItem);
        }

        // GET: Testing/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var testingItem = await _context.Testing.FirstOrDefaultAsync(m => m.Id == id);
            if (testingItem == null)
            {
                return NotFound();
            }

            return View(testingItem);
        }

        // POST: Testing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var testingItem = await _context.Testing.FindAsync(id);
            _context.Testing.Remove(testingItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TestingItemExists(int id)
        {
            return _context.Testing.Any(e => e.Id == id);
        }
    }
}

