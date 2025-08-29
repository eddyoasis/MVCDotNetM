using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MVCWebApp.Enums
{
    public enum CurrencySearchEnum
    {
        [Display(Name = "All")]
        All = 0,

        [Display(Name = "Singapore Dollar")]
        SGD = 1,

        [Display(Name = "US Dollar")]
        USD = 2
    }

    public enum MarginCallSearchStatusEnum
    {
        [Display(Name = "All")]
        All = 0,

        [Display(Name = "Pending")]
        Pending = 1,

        [Display(Name = "In Progress")]
        InProgress = 2,

        [Display(Name = "Rejected")]
        Rejected = 3,

        [Display(Name = "Approved")]
        Approved = 4
    }

    public enum YesNoEnum
    {
        [Display(Name = "All")]
        All = 0,
        YES = 1,
        NO = 2
    }

    public enum MarginCallMode
    {
        All = 2, //for DB Value
        ResetFlag = 3, //for DB Value
        MarginAvailable = 11,
        StoplossAvailable = 12,
        MOCAvailable = 13,
        TriggeredTodayMargin = 41,
        TriggeredTodayStoploss = 42,
        TriggeredTodayMOC = 43
    }

    public enum MarginCallEODDay
    {
        All = 0,
        Day1 = 1,
        Day2 = 2,
        Day3 = 3
    }

    public enum MarginCallOrderByColumn
    {
        PortfolioID = 1,
        Percentages = 2,
        MarginCallAmount = 3,
        Collateral = 4,
        VM = 5,
        IM = 6,
        InsertedDatetime = 7,
        ModifiedDatetime = 8
    }

    public enum MarginApprovalType
    {
        Margin = 1,
        Stoploss = 2,
        MOC = 3
    }

    public enum OrderByType
    {
        Asc = 1,
        Desc = 2
    }
}
