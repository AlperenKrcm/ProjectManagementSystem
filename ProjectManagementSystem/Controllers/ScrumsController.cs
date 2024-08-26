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
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers
{
    public class ScrumsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly GenericService<Scrum> _genericService;
        private readonly ScrumService _ScrumService;

        public ScrumsController(ApplicationDbContext context, GenericService<Scrum> genericService, ScrumService scrumService)
        {
            _genericService = genericService;
            _ScrumService = scrumService;
            _context = context;
        }

        // GET: Scrums
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(await _ScrumService.GetScrumAsync(userId));
        }

        // GET: Scrums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.scrums == null)
            {
                return NotFound();
            }

            var scrum = await _ScrumService.GetDetailsScrumsAsync(id);
            if (scrum == null)
            {
                return NotFound();
            }

            return View(scrum);
        }

        // GET: Scrums/Create
        public IActionResult Create()
        {
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName");
            return View();
        }

        // POST: Scrums/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("scrumID,scrumMaster,ProjectID")] Scrum scrum, int numberofSprint)
        {
            ModelState.Remove("Project");
            if(ModelState.IsValid)
            {
                await _ScrumService.AddScrumWithDailyScrumsAsync(scrum, numberofSprint);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName", scrum.ProjectID);



            return View(scrum);
        }

        // GET: Scrums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.scrums == null)
            {
                return NotFound();
            }

            var scrum = await _genericService.GetByIdAsync(id);
            if (scrum == null)
            {
                return NotFound();
            }
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName", scrum.ProjectID);
            return View(scrum);
        }

        // POST: Scrums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("scrumID,scrumMaster,ProjectID")] Scrum scrum)
        {
            if (id != scrum.scrumID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _genericService.UpdateAsync(scrum);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScrumExists(scrum.scrumID))
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
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName", scrum.ProjectID);
            return View(scrum);
        }

        // GET: Scrums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.scrums == null)
            {
                return NotFound();
            }

            var scrum = await _ScrumService.GetDetailsScrumsAsync(id);
            if (scrum == null)
            {
                return NotFound();
            }

            return View(scrum);
        }

        // POST: Scrums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.scrums == null)
            {
                return Problem("Entity set 'ApplicationDbContext.scrums'  is null.");
            }
            var scrum = await _genericService.GetByIdAsync(id);
            await _genericService.DeleteAsync(scrum);
            return RedirectToAction(nameof(Index));
        }

        private bool ScrumExists(int id)
        {
            return (_context.scrums?.Any(e => e.scrumID == id)).GetValueOrDefault();
        }
    }
}
