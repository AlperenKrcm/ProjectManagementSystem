using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Models
{
    public class Sprint
    {
        public int sprintID { get; set; }

        [ForeignKey("scrum")]
        public int scrumID {  get; set; }
        public Scrum Scrum { get; set; }

        public string? description { get; set; } 

    }
}
