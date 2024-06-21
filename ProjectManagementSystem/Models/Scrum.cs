using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManagementSystem.Models
{
    public class Scrum
    {
        [Key]
        public int scrumID { get; set; }
        public string scrumMaster {  get; set; }

        [ForeignKey("project")]
        public int ProjectID {  get; set; }
        public Project project { get; set; }    



        // denetleyicide takım işlerini yap.

    }

}
