using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models.Req
{
    public class MarginFormulaAddReq
    {
        [Required]
        [Display(Name = "Margin Type")]
        public string MarginType { get; set; }

        [Required]
        [Display(Name = "Margin Formula")]
        public string MarginFormula { get; set; }
    }
}
