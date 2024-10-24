using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly GenericService<Project> _genericService;
        private readonly ProjectService _projectService;

        public ProjectsController(ApplicationDbContext context, GenericService<Project> genericService, ProjectService projectService )
        {
            _projectService = projectService;
            _genericService= genericService;
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            
                return View(await _genericService.GetAllAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.projects == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetDetailsProject(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "admin")]

        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("projectID,projectName,projectDescription,startTime,projectDeadline,client")] Project project)
        {
            project.TasksForUsers = project.TasksForUsers ?? new List<TasksForUser>();
            ModelState.Remove("TasksForUsers");
            project.status = 0;
            ModelState.Remove("status");
            if (ModelState.IsValid)
            {
               await _genericService.AddAsync(project);
                return RedirectToAction(nameof(Index));
            }
            return View();
               
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "admin,ScrumMaster")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.projects == null)
            {
                return NotFound();
            }

            var project = await _genericService.GetByIdAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin,ScrumMaster")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("projectID,projectName,projectDescription,startTime,projectDeadline,status,client")] Project project)
        {
            if (id != project.projectID)
            {
                return NotFound();
            }
            ModelState.Remove("status");
            ModelState.Remove("TasksForUsers");

            if (ModelState.IsValid)
            {
                try
                {
                    await _genericService.UpdateAsync(project);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.projectID))
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
            return View(project);
        }
        [Authorize(Roles = "admin,ScrumMaster")]

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.projects == null)
            {
                return NotFound();
            }

            var project = await _projectService.GetDetailsProject(id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin,ScrumMaster")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.projects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.projects'  is null.");
            }
            var project = await _genericService.GetByIdAsync(id);
            if (project != null)
            {
            await _genericService.DeleteAsync(project);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return (_context.projects?.Any(e => e.projectID == id)).GetValueOrDefault();
        }
    }
}
