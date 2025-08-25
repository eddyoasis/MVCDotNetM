using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Enums
{
    public enum EmailNotificationTypeEnum
    {
        [Display(Name = "Day 1")]
        Day1 = 1,

        [Display(Name = "Day 2")]
        Day2 = 2,

        [Display(Name = "Day 3")]
        Day3 = 3,

        [Display(Name = "MTM")]
        MTM = 11,

        [Display(Name = "MTM Stoploss")]
        MTMStoploss = 12,

        [Display(Name = "Day 2 Stoploss")]
        Day2Stoploss = 121,

        [Display(Name = "Day 3 Stoploss")]
        Day3Stoploss = 122,

        [Display(Name = "MOC")]
        MOC = 13,
    }

    public enum EmailNotificationTypeSearchEnum
    {
        [Display(Name = "All")]
        All = 0,

        [Display(Name = "Day 1")]
        Day1 = 1,

        [Display(Name = "Day 2")]
        Day2 = 2,

        [Display(Name = "Day 3")]
        Day3 = 3,

        [Display(Name = "MTM")]
        MTM = 11,

        [Display(Name = "MTM Stoploss")]
        MTMStoploss = 12,

        [Display(Name = "Day 2 Stoploss")]
        Day2Stoploss = 121,

        [Display(Name = "Day 3 Stoploss")]
        Day3Stoploss = 122,

        [Display(Name = "MOC")]
        MOC = 13,
    }
}
