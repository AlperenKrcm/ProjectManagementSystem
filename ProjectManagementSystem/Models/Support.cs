using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Models
{
    public class Support
    {
        public int supportID { get; set; }

        [ForeignKey("task")]
        public int taskID {  get; set; }
        public TasksForUser tasksForUser { get; set; }

        public string description {  get; set; }

        public string? helpDescription { get; set; }
        public string? helperID { get; set; }
    }
}
