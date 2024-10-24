﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers
{
    public class SprintsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GenericService<Sprint> _genericService;
        private readonly SprintService _SprintService;
        private readonly EmailApiService _emailApiService;

        public SprintsController(ApplicationDbContext context, GenericService<Sprint> genericService, SprintService sprintService, EmailApiService emailApiService)
        {
            _genericService = genericService;
            _SprintService = sprintService;
            _context = context;
            _emailApiService = emailApiService;
        }

        // GET: Sprints
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _SprintService.GetSprintAsync(userId));

        }

        // GET: Sprints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.sprints == null)
            {
                return NotFound();
            }

            var sprint = await _SprintService.GetSprintDetailsAsync(id);
            if (sprint == null)
            {
                return NotFound();
            }

            return View(sprint);
        }

        // GET: Sprints/Create
        [Authorize(Roles = "admin,TeamLeader,ScrumMaster")]

        public IActionResult Create()
        {
            ViewData["scrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID");
            return View();
        }

        // POST: Sprints/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,TeamLeader,ScrumMaster")]

        public async Task<IActionResult> Create([Bind("sprintID,scrumID,description")] Sprint sprint)
        {
            ModelState.Remove("Scrum");
            if (ModelState.IsValid)
            {
                await _genericService.AddAsync(sprint);

                ViewData["scrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID", sprint.scrumID);
                var scrum = await _context.scrums.FindAsync(sprint.scrumID);
               await _emailApiService.SendEmailMultiAsync(await _genericService.TeamListAsync(scrum.ProjectID), "Sprint", "Yeni bir sprint atandı. Detayları inceleyebilirsiniz");
                return RedirectToAction(nameof(Index));
            }
            return View(sprint);

        }

        // GET: Sprints/Edit/5
        [Authorize(Roles = "admin,TeamLeader,ScrumMaster")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.sprints == null)
            {
                return NotFound();
            }
            var sprint = await _genericService.GetByIdAsync(id);
            if (sprint == null)
            {
                return NotFound();
            }
            ViewData["scrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID", sprint.scrumID);
            return View(sprint);
        }

        // POST: Sprints/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,TeamLeader,ScrumMaster")]

        public async Task<IActionResult> Edit(int id, [Bind("sprintID,scrumID,description")] Sprint sprint)
        {
            if (id != sprint.sprintID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _genericService.UpdateAsync(sprint);
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
            ViewData["scrumID"] = new SelectList(_context.scrums, "scrumID", "scrumID", sprint.scrumID);
            return View(sprint);
        }

        [Authorize(Roles = "admin,TeamLeader,ScrumMaster")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.sprints == null)
            {
                return NotFound();
            }

            var sprint = await _SprintService.GetSprintDetailsAsync(id);
            if (sprint == null)
            {
                return NotFound();
            }

            return View(sprint);
        }
        [Authorize(Roles = "admin,TeamLeader,ScrumMaster")]
        // POST: Sprints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.sprints == null)
            {
                return Problem("Entity set 'ApplicationDbContext.sprints'  is null.");
            }
            var sprint = await _genericService.GetByIdAsync(id);
            await _genericService.DeleteAsync(sprint);
            return RedirectToAction(nameof(Index));
        }

        private bool SprintExists(int id)
        {
            return (_context.sprints?.Any(e => e.sprintID == id)).GetValueOrDefault();
        }
    }
}
