using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services
{
    public class ProjectTeamService
    {
        private readonly ApplicationDbContext _context;
        public ProjectTeamService(ApplicationDbContext context)
        {
          _context = context;
        }
    
        public async Task<List<ProjectTeam>> GetProjectTeam()
        {
            var applicationDbContext = _context.projectTeams.Include(p => p.Project);
            return await applicationDbContext.ToListAsync();
        }

        public async Task<ProjectTeam> GetDetailsProjectTeam(int? id)
        {
            return await _context.projectTeams.Include(p => p.Project).FirstOrDefaultAsync(m => m.projectTeamID == id);
        }    
    }
}
