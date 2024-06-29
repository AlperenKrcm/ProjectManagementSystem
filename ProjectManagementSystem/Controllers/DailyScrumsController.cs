using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public class DailyScrumsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DailyScrumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DailyScrums
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.dailyScrums.Include(d => d.Scrum);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DailyScrums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.dailyScrums == null)
            {
                return NotFound();
            }

            var dailyScrum = await _context.dailyScrums
                .Include(d => d.Scrum)
                .FirstOrDefaultAsync(m => m.dailyScrumID == id);
            if (dailyScrum == null)
            {
                return NotFound();
            }

            return View(dailyScrum);
        }

        // GET: DailyScrums/Create
        public IActionResult Create()
        {
            ViewData["scrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID");
            return View();
        }

        // POST: DailyScrums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("dailyScrumID,scrumID,description")] DailyScrum dailyScrum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyScrum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["scrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID", dailyScrum.scrumID);
            return View(dailyScrum);
        }

        // GET: DailyScrums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.dailyScrums == null)
            {
                return NotFound();
            }

            var dailyScrum = await _context.dailyScrums.FindAsync(id);
            if (dailyScrum == null)
            {
                return NotFound();
            }
            ViewData["scrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID", dailyScrum.scrumID);
            return View(dailyScrum);
        }

        // POST: DailyScrums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("dailyScrumID,scrumID,description")] DailyScrum dailyScrum)
        {
            if (id != dailyScrum.dailyScrumID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyScrum);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyScrumExists(dailyScrum.dailyScrumID))
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
            ViewData["scrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID", dailyScrum.scrumID);
            return View(dailyScrum);
        }

        // GET: DailyScrums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.dailyScrums == null)
            {
                return NotFound();
            }

            var dailyScrum = await _context.dailyScrums
                .Include(d => d.Scrum)
                .FirstOrDefaultAsync(m => m.dailyScrumID == id);
            if (dailyScrum == null)
            {
                return NotFound();
            }

            return View(dailyScrum);
        }

        // POST: DailyScrums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.dailyScrums == null)
            {
                return Problem("Entity set 'ApplicationDbContext.dailyScrums'  is null.");
            }
            var dailyScrum = await _context.dailyScrums.FindAsync(id);
            if (dailyScrum != null)
            {
                _context.dailyScrums.Remove(dailyScrum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyScrumExists(int id)
        {
          return (_context.dailyScrums?.Any(e => e.dailyScrumID == id)).GetValueOrDefault();
        }
    }
}
