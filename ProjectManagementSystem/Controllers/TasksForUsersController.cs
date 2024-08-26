using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers
{
    public class TasksForUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TasksForUserService _tasksForUserService;
        private readonly GenericService<TasksForUser> _genericService;
        public TasksForUsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, TasksForUserService tasksForUserService,GenericService<TasksForUser> genericService)
        {
            _genericService= genericService;
            _tasksForUserService= tasksForUserService;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: TasksForUsers
        public async Task<IActionResult> Index()
        {
            return View( await _tasksForUserService.GetTasksForUserAsync());
        }

        // GET: TasksForUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
          
            return View(await _tasksForUserService.GetDetailsTasksForUsertAsync(id));
        }

        // GET: TasksForUsers/Create

        // [Authorize(Roles ="admin")]
        // [Authorize(Roles="TeamLeader")]
        public IActionResult CreateSelectProject()
        {
            ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName");

            return View();
        }
        // [Authorize(Roles ="admin")]
        // [Authorize(Roles="TeamLeader")]
        public IActionResult Create(int ProjectID)
        {
            ViewData.Clear();


            ViewData["ProjectID"] = new SelectList(_context.projects.Where(x=>x.projectID==ProjectID), "projectID", "projectName");

            var tasksForUser = new TasksForUser
            {
                ProjectID = ProjectID

            };
            ViewData["ProjectTeamUsers"] = new SelectList(
               from pt in _context.projectTeams
               join u in _context.Users on pt.UserID equals u.Id
               where pt.ProjectID == ProjectID
               select new { pt.projectTeamID, u.UserName },"UserName","UserName");
/*
            var users = _userManager.Users.ToList();
            var userSelectList = users.Select(user => new SelectListItem
            {
                Value = user.Id,
                Text = user.UserName
            }).ToList();

            ViewData["ProjectTeamUsers"] = new SelectList(_context.projectTeams.Where(x => x.ProjectID == ProjectID), "projectTeamID", "UserID");
            */ 
            return View(tasksForUser);
        }

        // POST: TasksForUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize(Roles ="admin")]
        // [Authorize(Roles="TeamLeader")]
        public async Task<IActionResult> Create([Bind("taskForUserID,ProjectID,userID,taskDeadline,status")] TasksForUser tasksForUser)
        {

            tasksForUser.project = await _context.projects.FindAsync(tasksForUser.ProjectID);
            tasksForUser.status = 0;
            ModelState.Remove("status");
            ModelState.Remove("project");
            if (ModelState.IsValid)
            {

                await _genericService.AddAsync(tasksForUser);  
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
                    await _genericService.UpdateAsync(tasksForUser);
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.tasksForUser == null)
            {
                return NotFound();
            }

            var tasksForUser = await _tasksForUserService.GetDetailsTasksForUsertAsync(id);
            if (tasksForUser == null)
            {
                return NotFound();
            }

            return View(tasksForUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.tasksForUser == null)
            {
                return Problem("Entity set 'ApplicationDbContext.tasksForUser'  is null.");
            }
//            var tasksForUser = await _context.tasksForUser.FindAsync(id);
          await _genericService.DeleteAsync(await _genericService.GetByIdAsync(id));
            return RedirectToAction(nameof(Index));
        }

        private bool TasksForUserExists(int id)
        {
            return (_context.tasksForUser?.Any(e => e.taskForUserID == id)).GetValueOrDefault();
        }
    }
}
