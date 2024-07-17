using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Enums
{
    public enum StatusEnum
    {
        [Display(Name ="NotStarted")]
        NotStarted=0,
        [Display(Name = "InProgress")]

        InProgress = 1,
        [Display(Name ="Completed")]
        Completed=2,
        [Display(Name ="OnHold")]

        OnHold = 3,
        [Display(Name ="Testing")]

        Testing = 4
    }
}
