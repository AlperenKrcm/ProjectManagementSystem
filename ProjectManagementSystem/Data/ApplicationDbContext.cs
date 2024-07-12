using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<DailyScrum> dailyScrumsTable { get; set; }
         public DbSet<Sprint> sprints { get; set; }  

        public DbSet<Project> projects { get; set; }    
        public DbSet<ProjectTeam> projectTeams { get; set; }
        public DbSet<Scrum> scrums { get; set; }    
        public DbSet<Support> supports {  get; set; }
        public DbSet<TasksForUser> tasksForUser { get; set; }
        
    }
}
