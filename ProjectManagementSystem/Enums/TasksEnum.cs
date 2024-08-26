using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Enums
{
    public enum TasksEnum
    {
        [Display(Name = "Appointed")]
        Appointed = 0,
        [Display(Name = "Is Being Done")]

        BeingDone = 1,
        [Display(Name = "Completed")]
        Completed = 2,
        [Display(Name = "OnHold")]
        OnHold = 3,
        [Display(Name = "Help")]

        Help = 4
    }
}
