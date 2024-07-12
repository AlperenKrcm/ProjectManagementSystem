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
    public class SupportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SupportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Supports
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.supports.Include(s => s.tasksForUser);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Supports/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.supports == null)
            {
                return NotFound();
            }

            var support = await _context.supports
                .Include(s => s.tasksForUser)
                .FirstOrDefaultAsync(m => m.supportID == id);
            if (support == null)
            {
                return NotFound();
            }

            return View(support);
        }

        // GET: Supports/Create
        public IActionResult Create()
        {
            ViewData["taskForUserID"] = new SelectList(_context.tasksForUser, "taskForUserID", "taskForUserID");
            return View();
        }

        // POST: Supports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("supportID,taskForUserID,description,helpDescription,helperID")] Support support)
        {
            ModelState.Remove("TasksForUser");
            if (ModelState.IsValid)
            {
                _context.Add(support);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["taskForUserID"] = new SelectList(_context.tasksForUser, "taskForUserID", "taskForUserID", support.taskForUserID);
            return View(support);
        }

        // GET: Supports/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.supports == null)
            {
                return NotFound();
            }

            var support = await _context.supports.FindAsync(id);
            if (support == null)
            {
                return NotFound();
            }
            ViewData["taskForUserID"] = new SelectList(_context.tasksForUser, "taskForUserID", "taskForUserID", support.taskForUserID);
            return View(support);
        }

        // POST: Supports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("supportID,taskForUserID,description,helpDescription,helperID")] Support support)
        {
            if (id != support.supportID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(support);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupportExists(support.supportID))
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
            ViewData["taskForUserID"] = new SelectList(_context.tasksForUser, "taskForUserID", "taskForUserID", support.taskForUserID);
            return View(support);
        }

        // GET: Supports/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.supports == null)
            {
                return NotFound();
            }

            var support = await _context.supports
                .Include(s => s.tasksForUser)
                .FirstOrDefaultAsync(m => m.supportID == id);
            if (support == null)
            {
                return NotFound();
            }

            return View(support);
        }

        // POST: Supports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.supports == null)
            {
                return Problem("Entity set 'ApplicationDbContext.supports'  is null.");
            }
            var support = await _context.supports.FindAsync(id);
            if (support != null)
            {
                _context.supports.Remove(support);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupportExists(int id)
        {
          return (_context.supports?.Any(e => e.supportID == id)).GetValueOrDefault();
        }
    }
}
