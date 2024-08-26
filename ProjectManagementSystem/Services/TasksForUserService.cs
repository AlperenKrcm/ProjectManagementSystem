using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services
{
    public class TasksForUserService
    {
        private readonly ApplicationDbContext _context;
        public TasksForUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<TasksForUser>> GetTasksForUserAsync()
        {
            return await _context.tasksForUser.Include(t => t.project).ToListAsync();
        }
        public async Task<TasksForUser> GetDetailsTasksForUsertAsync(int? id)
        {
            return await _context.tasksForUser.Include(s => s.project).FirstOrDefaultAsync(m => m.ProjectID == id);
        }

        
    }
}
