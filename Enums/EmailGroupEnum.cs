using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Enums
{
    public enum EmailGroupTypeEnum
    {
        [Display(Name = "Auto Margin")]
        AutoMargin = 1,

        [Display(Name = "Stop Loss")]
        StopLoss = 2,

        [Display(Name = "MOC")]
        MOC = 3
    }

    public enum EmailGroupTypeSearchEnum
    {
        [Display(Name = "All")]
        All = 0,

        [Display(Name = "Auto Margin")]
        AutoMargin = 1,

        [Display(Name = "Stop Loss")]
        StopLoss = 2,

        [Display(Name = "MOC")]
        MOC = 3
    }
}
