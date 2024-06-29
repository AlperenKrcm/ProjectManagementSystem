using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models
{
    public class Project
    {

        [Key]
        public int projectID {  get; set; }

        [Required(ErrorMessage ="Please Type Project Name")]
        public string projectName { get; set; }

        public string? projectDescription { get; set; }

        public DateTime startTime { get; set; }
        public DateTime projectDeadline { get; set; }

        public string status {  get; set; }

        public string client {  get; set; }
        public virtual ICollection<TasksForUser> TasksForUsers { get; set; }



    }
}
