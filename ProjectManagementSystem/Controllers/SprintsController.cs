﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public class SprintsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SprintsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sprints
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.sprints.Include(s => s.scrum);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sprints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.sprints == null)
            {
                return NotFound();
            }

            var sprint = await _context.sprints
                .Include(s => s.scrum)
                .FirstOrDefaultAsync(m => m.sprintID == id);
            if (sprint == null)
            {
                return NotFound();
            }

            return View(sprint);
        }

        // GET: Sprints/Create
        public IActionResult Create()
        {
            ViewData["ScrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID");
            return View();
        }

        // POST: Sprints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("sprintID,sprintNumber,ScrumID,springTime,description")] Sprint sprint)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sprint);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ScrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID", sprint.ScrumID);
            return View(sprint);
        }

        // GET: Sprints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.sprints == null)
            {
                return NotFound();
            }

            var sprint = await _context.sprints.FindAsync(id);
            if (sprint == null)
            {
                return NotFound();
            }
            ViewData["ScrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID", sprint.ScrumID);
            return View(sprint);
        }

        // POST: Sprints/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("sprintID,sprintNumber,ScrumID,springTime,description")] Sprint sprint)
        {
            if (id != sprint.sprintID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sprint);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SprintExists(sprint.sprintID))
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
            ViewData["ScrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID", sprint.ScrumID);
            return View(sprint);
        }

        // GET: Sprints/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.sprints == null)
            {
                return NotFound();
            }

            var sprint = await _context.sprints
                .Include(s => s.scrum)
                .FirstOrDefaultAsync(m => m.sprintID == id);
            if (sprint == null)
            {
                return NotFound();
            }

            return View(sprint);
        }

        // POST: Sprints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.sprints == null)
            {
                return Problem("Entity set 'ApplicationDbContext.sprints'  is null.");
            }
            var sprint = await _context.sprints.FindAsync(id);
            if (sprint != null)
            {
                _context.sprints.Remove(sprint);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SprintExists(int id)
        {
          return (_context.sprints?.Any(e => e.sprintID == id)).GetValueOrDefault();
        }
    }
}
