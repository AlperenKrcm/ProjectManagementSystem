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

namespace ProjectManagementSystem.Controllers
{
    public class ScrumsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ScrumsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Scrums
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var userProjectIds = _context.projectTeams
                               .Where(pt => pt.UserID == userId)
                               .Select(pt => pt.ProjectID)
                               .ToList();
            var scrums = _context.scrums.Include(s => s.project)
                                     .Where(s => userProjectIds.Contains(s.ProjectID))
                                     .ToList();

            return View(scrums);
        }

        // GET: Scrums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.scrums == null)
            {
                return NotFound();
            }

            var scrum = await _context.scrums
                .Include(s => s.project)
                .FirstOrDefaultAsync(m => m.scrumID == id);
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
            if (ModelState.IsValid)
            {
                _context.Add(scrum);
                await _context.SaveChangesAsync();

                DailyScrum dummydaily = new DailyScrum();
               
                dummydaily = _context.dailyScrumsTable.Where(x => x.ScrumID == scrum.scrumID).FirstOrDefault();


                if (dummydaily != null)
                {

                    var a = dummydaily.dailyScrumTime;

                    for (int i = dummydaily.dailyScrumNumber; i < numberofSprint; i++)
                    {
                        DailyScrum dailyScrum = new DailyScrum();
                        dailyScrum.ScrumID = Convert.ToInt32(scrum.scrumID);
                        dailyScrum.dailyScrumNumber = i + dummydaily.dailyScrumNumber + 1;

                        if ( a.DayOfWeek == DayOfWeek.Saturday)
                            a = a.AddDays(2);
                        else if (a.DayOfWeek == DayOfWeek.Sunday)
                            a = a.AddDays(1);
                        
                        dailyScrum.dailyScrumTime = a;
                        dailyScrum.description = "Yoneticinin düzenlenmesi için boş bırakıldı";

                        _context.Add(dailyScrum);
                        await _context.SaveChangesAsync();

                    }

                }
                else
                {
                    DateTime a= DateTime.Now;
                    for (int i = 0; i < numberofSprint; i++)
                    {
                        
                        DailyScrum dailyScrum = new DailyScrum();
                        dailyScrum.ScrumID = Convert.ToInt32(scrum.scrumID);
                        dailyScrum.dailyScrumNumber = i + 1;
                        
                        if (a.DayOfWeek == DayOfWeek.Saturday)
                            a = a.AddDays(2);
                        else if (a.DayOfWeek == DayOfWeek.Sunday)
                            a = a.AddDays(1);
                      

                        dailyScrum.dailyScrumTime = a;
                        dailyScrum.description = "Yoneticinin düzenlenmesi için boş bırakıldı";
                        _context.Add(dailyScrum);
                        await _context.SaveChangesAsync();
                        a = a.AddDays(1);
                    }
                }
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

            var scrum = await _context.scrums.FindAsync(id);
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
                    _context.Update(scrum);
                    await _context.SaveChangesAsync();
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

            var scrum = await _context.scrums
                .Include(s => s.project)
                .FirstOrDefaultAsync(m => m.scrumID == id);
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
            var scrum = await _context.scrums.FindAsync(id);
            if (scrum != null)
            {
                _context.scrums.Remove(scrum);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScrumExists(int id)
        {
            return (_context.scrums?.Any(e => e.scrumID == id)).GetValueOrDefault();
        }
    }
}
