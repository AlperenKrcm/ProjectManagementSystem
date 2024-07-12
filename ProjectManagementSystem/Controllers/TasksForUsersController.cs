﻿using System;
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
    public class TasksForUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TasksForUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TasksForUsers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.tasksForUser.Include(t => t.project);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TasksForUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.tasksForUser == null)
            {
                return NotFound();
            }

            var tasksForUser = await _context.tasksForUser
                .Include(t => t.project)
                .FirstOrDefaultAsync(m => m.taskForUserID == id);
            if (tasksForUser == null)
            {
                return NotFound();
            }

            return View(tasksForUser);
        }

        // GET: TasksForUsers/Create
        public IActionResult Create()
        {
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName");
            return View();
        }

        // POST: TasksForUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("taskForUserID,ProjectID,userID,taskDeadline,status")] TasksForUser tasksForUser)
        {

            tasksForUser.project = await _context.projects.FindAsync(tasksForUser.ProjectID);
            ModelState.Remove("project");

            if (ModelState.IsValid)
            {

                _context.Add(tasksForUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName", tasksForUser.ProjectID);
            return View(tasksForUser);
        }

        // GET: TasksForUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.tasksForUser == null)
            {
                return NotFound();
            }

            var tasksForUser = await _context.tasksForUser.FindAsync(id);
            if (tasksForUser == null)
            {
                return NotFound();
            }
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName", tasksForUser.ProjectID);
            return View(tasksForUser);
        }

        // POST: TasksForUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("taskForUserID,ProjectID,userID,taskDeadline,status")] TasksForUser tasksForUser)
        {
            if (id != tasksForUser.taskForUserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tasksForUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TasksForUserExists(tasksForUser.taskForUserID))
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
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName", tasksForUser.ProjectID);
            return View(tasksForUser);
        }

        // GET: TasksForUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tasksForUser == null)
            {
                return NotFound();
            }

            var tasksForUser = await _context.tasksForUser
                .Include(t => t.project)
                .FirstOrDefaultAsync(m => m.taskForUserID == id);
            if (tasksForUser == null)
            {
                return NotFound();
            }

            return View(tasksForUser);
        }

        // POST: TasksForUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tasksForUser == null)
            {
                return Problem("Entity set 'ApplicationDbContext.tasksForUser'  is null.");
            }
            var tasksForUser = await _context.tasksForUser.FindAsync(id);
            if (tasksForUser != null)
            {
                _context.tasksForUser.Remove(tasksForUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TasksForUserExists(int id)
        {
          return (_context.tasksForUser?.Any(e => e.taskForUserID == id)).GetValueOrDefault();
        }
    }
}
