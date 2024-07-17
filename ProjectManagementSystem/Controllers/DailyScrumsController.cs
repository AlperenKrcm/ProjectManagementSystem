using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DailyScrumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DailyScrums
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var userProjectIds = _context.projectTeams
                               .Where(pt => pt.UserID == userId)
                               .Select(pt => pt.ProjectID)
                               .ToList();
            var scrums = _context.scrums.Include(s => s.project)
                                     .Where(s => userProjectIds.Contains(s.ProjectID))
                                     .ToList();
            var dailyscrums = _context.dailyScrumsTable.Include(s => s.scrum)
                                     .Where(s => userProjectIds.Contains(s.ScrumID))
                                     .ToList();
            return View(dailyscrums);
        }

        // GET: DailyScrums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.dailyScrumsTable == null)
            {
                return NotFound();
            }

            var dailyScrum = await _context.dailyScrumsTable
                .Include(d => d.scrum)
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
            ViewData["ScrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID");
            return View();
        }

        // POST: DailyScrums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("dailyScrumID,dailyScrumNumber,ScrumID,dailyScrumTime,description")] DailyScrum dailyScrum)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyScrum);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID", dailyScrum.ScrumID);
            return View(dailyScrum);
        }

        // GET: DailyScrums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.dailyScrumsTable == null)
            {
                return NotFound();
            }

            var dailyScrum = await _context.dailyScrumsTable.FindAsync(id);
            if (dailyScrum == null)
            {
                return NotFound();
            }
            ViewData["ScrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID", dailyScrum.ScrumID);
            return View(dailyScrum);
        }

        // POST: DailyScrums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("dailyScrumID,dailyScrumNumber,ScrumID,dailyScrumTime,description")] DailyScrum dailyScrum)
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
            ViewData["ScrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID", dailyScrum.ScrumID);
            return View(dailyScrum);
        }

        // GET: DailyScrums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.dailyScrumsTable == null)
            {
                return NotFound();
            }

            var dailyScrum = await _context.dailyScrumsTable
                .Include(d => d.scrum)
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
            if (_context.dailyScrumsTable == null)
            {
                return Problem("Entity set 'ApplicationDbContext.dailyScrumsTable'  is null.");
            }
            var dailyScrum = await _context.dailyScrumsTable.FindAsync(id);
            if (dailyScrum != null)
            {
                _context.dailyScrumsTable.Remove(dailyScrum);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyScrumExists(int id)
        {
          return (_context.dailyScrumsTable?.Any(e => e.dailyScrumID == id)).GetValueOrDefault();
        }
    }
}
