using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services
{
    public class ProjectService
    {
        private readonly ApplicationDbContext _context;
        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

       
        public async Task<Project> GetDetailsProject(int? id)
        {
            return await _context.projects.FirstOrDefaultAsync(m => m.projectID == id);
        }
    }
}
