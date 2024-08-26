using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services
{
    public class SprintService
    {
        private readonly ApplicationDbContext _context;
        public SprintService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Sprint>> GetSprintAsync(string userId)
        {
            
             
            return await _context.sprints
                          .Include(s => s.Scrum)
                          .Where(s => _context.projectTeams
                                              .Where(pt => pt.UserID == userId)
                                              .Select(pt => pt.ProjectID)
                                              .Contains(s.Scrum.ProjectID)).ToListAsync();
        }
        public async Task<Sprint> GetSprintDetailsAsync(int? id)
        {
            return await _context.sprints.Include(s => s.Scrum).FirstOrDefaultAsync(m => m.sprintID == id);
        }

       
    }
}
