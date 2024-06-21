using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Models
{
    public class Sprint
    {
        public int sprintID { get; set; }

        public int sprintNumber {  get; set; }

        [ForeignKey("scrum")]
        public int ScrumID {  get; set; }

        public Scrum scrum { get; set; }


        public DateTime springTime { get; set; }

        public string description {  get; set; }

    }
}
