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

        [Display(Name = "Stock loss")]
        StockLoss = 12,
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

        [Display(Name = "Stock loss")]
        StockLoss = 12,
    }
}
