using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;

public class DailyEmailService
{
    private readonly ApplicationDbContext _context;
    private readonly EmailApiService _emailApiService;

    public DailyEmailService(ApplicationDbContext context, EmailApiService emailApiService)
    {
        _context = context;
        _emailApiService = emailApiService;
    }

    public async Task SendDailyScrumEmails()
    {
        var oneDayAgo = DateTime.Now.AddDays(-1);

        var recentDailyScrums = await _context.dailyScrumsTable
            .Where(d => d.dailyScrumTime >= oneDayAgo).Select(d => d.ScrumID).Distinct().ToListAsync();

        if (!recentDailyScrums.Any())
        {
            return;
        }

        var projectIds = await _context.scrums
            .Where(s => recentDailyScrums.Contains(s.scrumID))
            .Select(s => s.ProjectID)
            .Distinct()
            .ToListAsync();

        if (!projectIds.Any())
        {
            return;
        }

        var teamEmails = await _context.projectTeams
            .Where(pt => projectIds.Contains(pt.ProjectID))
            .Select(pt => pt.UserID) 
            .Distinct()
            .ToListAsync();

        var emailList = await _context.Users
            .Where(u => teamEmails.Contains(u.Id))
            .Select(u => u.Email)
            .Distinct()
            .ToListAsync();

        if (!emailList.Any())
        {
            return;
        }

        await _emailApiService.SendEmailMultiAsync(emailList, "Daily Scrum Raporu", "Son 24 saatteki scrum raporu.");

    }
}
