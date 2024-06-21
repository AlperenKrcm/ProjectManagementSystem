using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Models
{
    public class TasksForUser
    {
        [Key]
        public int taskForUserID {  get; set; }
        [ForeignKey("project")]
        public int ProjectID {  get; set; }
        public Project project { get; set; }
            
        public string userID {  get; set; }

        public DateTime taskDeadline { get; set; } 

        public string status {  get; set; } // controller => status=Appointed 


    }

}
