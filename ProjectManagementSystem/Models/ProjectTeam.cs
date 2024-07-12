using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Models
{
    public class ProjectTeam
    {

        [Key]
        public int projectTeamID { get; set; }

        [ForeignKey("project")]
        public int ProjectID {  get; set; }
        public Project Project { get; set; }    

        public string UserID {  get; set; }
        public string UserName {  get; set; }

        public string ProjectRole {  get; set; }



    }
}
