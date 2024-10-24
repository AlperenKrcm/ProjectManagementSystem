using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectTeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ProjectTeamService _projectTeamService;
        private readonly GenericService<ProjectTeam> _genericService;
        public ProjectTeamsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ProjectTeamService projectTeamService, GenericService<ProjectTeam> genericService)
        {
            _genericService = genericService;
            _projectTeamService = projectTeamService;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _projectTeamService.GetProjectTeam());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.projectTeams == null)
            {
                return NotFound();
            }
            var projectTeam = await _projectTeamService.GetDetailsProjectTeam(id);
            if (projectTeam == null)
            {
                return NotFound();
            }

            return View(projectTeam);
        }

        // GET: ProjectTeams/Create
        [Authorize(Roles = "admin,TeamLeader")]

        public IActionResult Create()
        {
            ViewData.Clear();
            var users = _userManager.Users.ToList();
            var userSelectList = users.Select(user => new SelectListItem
            {
                Value = user.Id,
                Text = user.UserName
            }).ToList();
            ViewBag.UsersID = new SelectList(users, "Id", "UserName");

            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName");

            return View();
        }
        [Authorize(Roles = "admin,TeamLeader")]

        // POST: ProjectTeams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("projectTeamID,ProjectID,UserID,ProjectRole,UserName")] ProjectTeam projectTeam)
        {
            var users = _userManager.Users.ToList();
            var userSelectList = users.Select(user => new SelectListItem
            {
                Value = user.Id,
                Text = user.UserName
            }).ToList();
            var dummy = (users.FirstOrDefault(x => x.Id == projectTeam.UserID));
            projectTeam.UserName = Convert.ToString(dummy.UserName);
            ModelState.Remove("UserName");
            ModelState.Remove("Project");
            if (ModelState.IsValid)
            {
                await _genericService.AddAsync(projectTeam);
                return RedirectToAction(nameof(Index));
            }
           
            ViewBag.UsersID = new SelectList(userSelectList, "Value", "Text", projectTeam.UserID);
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName", projectTeam.ProjectID);

            return View(projectTeam);
        }

        // GET: ProjectTeams/Edit/5
        [Authorize(Roles = "admin,TeamLeader")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.projectTeams == null)
            {
                return NotFound();
            }

            var projectTeam = await _genericService.GetByIdAsync(id);
            if (projectTeam == null)
            {
                return NotFound();
            }
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName", projectTeam.ProjectID);
            return View(projectTeam);
        }

        // POST: ProjectTeams/Edit/5
        [Authorize(Roles = "admin,TeamLeader")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("projectTeamID,ProjectID,UserID,ProjectRole")] ProjectTeam projectTeam)
        {
            if (id != projectTeam.projectTeamID)
            {
                return NotFound();
            }
            ModelState.Remove("UserName");
            ModelState.Remove("Project");
            if (ModelState.IsValid)
            {
                try
                {
                    await _genericService.UpdateAsync(projectTeam);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectTeamExists(projectTeam.projectTeamID))
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
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName", projectTeam.ProjectID);
            return View(projectTeam);
        }

        // GET: ProjectTeams/Delete/5
        [Authorize(Roles = "admin,TeamLeader")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.projectTeams == null)
            {
                return NotFound();
            }

            var projectTeam = await _projectTeamService.GetDetailsProjectTeam(id);
            if (projectTeam == null)
            {
                return NotFound();
            }

            return View(projectTeam);
        }

        // POST: ProjectTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin,TeamLeader")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.projectTeams == null)
            {
                return Problem("Entity set 'ApplicationDbContext.projectTeams'  is null.");
            }
            var projectTeam = await _genericService.GetByIdAsync(id);
            if (projectTeam != null)
            {
            await _genericService.DeleteAsync(projectTeam);
            }

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "admin,TeamLeader")]

        private bool ProjectTeamExists(int id)
        {
            return (_context.projectTeams?.Any(e => e.projectTeamID == id)).GetValueOrDefault();
        }
    }
}
