using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Controllers
{
    public class ProjectTeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ProjectTeamsController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: ProjectTeams
        public async Task<IActionResult> Index()
        {

            var applicationDbContext = _context.projectTeams.Include(p => p.Project);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ProjectTeams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.projectTeams == null)
            {
                return NotFound();
            }

            var projectTeam = await _context.projectTeams
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.projectTeamID == id);
            if (projectTeam == null)
            {
                return NotFound();
            }

            return View(projectTeam);
        }

        // GET: ProjectTeams/Create
        public async Task<IActionResult> Create()
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

        // POST: ProjectTeams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                _context.Add(projectTeam);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            ViewBag.UsersID = new SelectList(userSelectList, "Value", "Text", projectTeam.UserID);
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName", projectTeam.ProjectID);

            return View(projectTeam);
        }

        // GET: ProjectTeams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.projectTeams == null)
            {
                return NotFound();
            }

            var projectTeam = await _context.projectTeams.FindAsync(id);
            if (projectTeam == null)
            {
                return NotFound();
            }
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName", projectTeam.ProjectID);
            return View(projectTeam);
        }

        // POST: ProjectTeams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("projectTeamID,ProjectID,UserID,ProjectRole")] ProjectTeam projectTeam)
        {
            if (id != projectTeam.projectTeamID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projectTeam);
                    await _context.SaveChangesAsync();
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.projectTeams == null)
            {
                return NotFound();
            }

            var projectTeam = await _context.projectTeams
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.projectTeamID == id);
            if (projectTeam == null)
            {
                return NotFound();
            }

            return View(projectTeam);
        }

        // POST: ProjectTeams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.projectTeams == null)
            {
                return Problem("Entity set 'ApplicationDbContext.projectTeams'  is null.");
            }
            var projectTeam = await _context.projectTeams.FindAsync(id);
            if (projectTeam != null)
            {
                _context.projectTeams.Remove(projectTeam);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectTeamExists(int id)
        {
            return (_context.projectTeams?.Any(e => e.projectTeamID == id)).GetValueOrDefault();
        }
    }
}
