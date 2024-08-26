using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;


namespace ProjectManagementSystem.Services
{
    public class DailyScrumService
    {
        private readonly ApplicationDbContext _context;
        public DailyScrumService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<DailyScrum>> GetDailyScrumsAsync(string userId)
        {
            var userProjectIds =await  _context.projectTeams.Where(pt => pt.UserID == userId).Select(pt => pt.ProjectID).ToListAsync();
            var scrums = await _context.scrums.Include(s => s.project).Where(s => userProjectIds.Contains(s.ProjectID)).ToListAsync();
            return await _context.dailyScrumsTable
                     .Include(s => s.scrum)
                     .Where(s => scrums.Select(scrum => scrum.scrumID).Contains(s.ScrumID))
                     .ToListAsync();
        }
        public async Task<DailyScrum> GetDetailsDailyScrumAsync(int? id)
        {
            return await _context.dailyScrumsTable.Include(d => d.scrum).FirstOrDefaultAsync(m => m.dailyScrumID == id);
        }
        public async Task AddScrumWithDailyScrumsAsync(Scrum scrum, int numberofSprint)
        {
            _context.Add(scrum);
            await _context.SaveChangesAsync();

            DailyScrum dummydaily = _context.dailyScrumsTable
                .Where(x => x.ScrumID == scrum.scrumID)
                .FirstOrDefault();

            if (dummydaily != null)
            {
                var a = dummydaily.dailyScrumTime;

                for (int i = dummydaily.dailyScrumNumber; i < numberofSprint; i++)
                {
                    DailyScrum dailyScrum = new DailyScrum
                    {
                        ScrumID = scrum.scrumID,
                        dailyScrumNumber = i + dummydaily.dailyScrumNumber + 1,
                        dailyScrumTime = a,
                        description = "Yoneticinin düzenlenmesi için boş bırakıldı"
                    };

                    if (a.DayOfWeek == DayOfWeek.Saturday)
                        a = a.AddDays(2);
                    else if (a.DayOfWeek == DayOfWeek.Sunday)
                        a = a.AddDays(1);

                    _context.Add(dailyScrum);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                DateTime a = DateTime.Now;
                for (int i = 0; i < numberofSprint; i++)
                {
                    DailyScrum dailyScrum = new DailyScrum
                    {
                        ScrumID = scrum.scrumID,
                        dailyScrumNumber = i + 1,
                        dailyScrumTime = a,
                        description = "Yoneticinin düzenlenmesi için boş bırakıldı"
                    };

                    if (a.DayOfWeek == DayOfWeek.Saturday)
                        a = a.AddDays(2);
                    else if (a.DayOfWeek == DayOfWeek.Sunday)
                        a = a.AddDays(1);

                    _context.Add(dailyScrum);
                    await _context.SaveChangesAsync();
                    a = a.AddDays(1);
                }
            }
        }
    }
}


