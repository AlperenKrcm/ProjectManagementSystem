using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services
{
    public class SupportService
    {
        private readonly ApplicationDbContext _context;
        public SupportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Support>> GetSupportAsync()
        {
            return await _context.supports.Include(t => t.tasksForUser).ToListAsync();
        }
        public async Task<Support> GetDetailsSupportAsync(int? id)
        {
            return await _context.supports.Include(s => s.tasksForUser).FirstOrDefaultAsync(m => m.supportID == id);
        }    
        
        


    }
}
