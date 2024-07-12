using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Models
{
    public class DailyScrum
    {
        public int dailyScrumID { get; set; }

        public int dailyScrumNumber {  get; set; }

        [ForeignKey("scrum")]
        public int ScrumID {  get; set; }

        public Scrum scrum { get; set; }

        public DateTime dailyScrumTime { get; set; }

        public string description {  get; set; }

    }
}
