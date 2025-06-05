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
}
