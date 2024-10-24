using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        private readonly EmailApiService _emailApiService;
        public TasksForUsersController(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, TasksForUserService tasksForUserService,GenericService<TasksForUser> genericService,EmailApiService emailApiService)
        {
            _genericService= genericService;
            _tasksForUserService= tasksForUserService;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailApiService = emailApiService;
        }

        // GET: TasksForUsers
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            return View( await _tasksForUserService.GetTasksForUserAsync(userId, userRole));
        }

        // GET: TasksForUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
          
            return View(await _tasksForUserService.GetDetailsTasksForUsertAsync(id));
        }

        // GET: TasksForUsers/Create

        [Authorize(Roles = "admin,TeamLeader")]

        public IActionResult CreateSelectProject()
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            ViewData.Clear();
            if (userRole == "admin" || userRole == "TeamLeader")
            {
                ViewData["ProjectID"] = new SelectList(_context.projects, "projectID", "projectName");

                return View();
            }

            TempData["HataMesaji"] = "Görev Atama Yetkiniz Yoktur.";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "admin,TeamLeader")]

        public IActionResult Create(int ProjectID)
        {


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
        [Authorize(Roles = "admin,TeamLeader")]

        public async Task<IActionResult> Create([Bind("taskForUserID,ProjectID,userID,taskDeadline,status, taskDescription")] TasksForUser tasksForUser)
        {

            tasksForUser.project = await _context.projects.FindAsync(tasksForUser.ProjectID);
            _emailApiService.SendEmailAsync(tasksForUser.userID, "Görev atandı", tasksForUser.taskDescription);
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
        [Authorize(Roles = "admin,TeamLeader")]

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
        [Authorize(Roles = "admin,TeamLeader")]

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
        [Authorize(Roles = "admin,TeamLeader")]

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
        [Authorize(Roles = "admin,TeamLeader")]

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
