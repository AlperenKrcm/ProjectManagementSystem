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

        public async Task<List<TasksForUser>> GetTasksForUserAsync(string userId, string role)
        {
            if (role == "admin")
            {
                // Admin tüm görevleri görebilir
                return await _context.tasksForUser
                    .Include(t => t.project)
                    .ToListAsync();
            }
            else if (role == "TeamLeader" || role == "TeamPerson")
            {
                // Hem TeamLeader hem de TeamPerson rolleri için aynı sorguyu kullanabiliriz
                return await _context.tasksForUser
                    .Include(t => t.project)
                    .Where(t => _context.projectTeams
                        .Any(pt => pt.UserID == userId && pt.ProjectRole == role && pt.ProjectID == t.ProjectID))
                    .ToListAsync();
            }

            // Eğer rol tanımlı değilse boş liste dönüyoruz
            return new List<TasksForUser>();
        }

        public async Task<TasksForUser> GetDetailsTasksForUsertAsync(int? id)
        {
            return await _context.tasksForUser.Include(s => s.project).FirstOrDefaultAsync(m => m.taskForUserID == id);
        }

        
    }
}
