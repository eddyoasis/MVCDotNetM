using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Enums
{
    public enum FormulaTypeEnum
    {
        [Display(Name = "Auto Margin")]
        AutoMargin = 1,

        [Display(Name = "Stock Loss")]
        StockLoss = 2
    }

    public enum FormulaTypeSearchEnum
    {
        [Display(Name = "All")]
        All = 0,

        [Display(Name = "Auto Margin")]
        AutoMargin = 1,

        [Display(Name = "Stock Loss")]
        StockLoss = 2
    }
}
