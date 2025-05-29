using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models.MarginFormulas
{
    public class MarginFormulaAddReq
    {
        [Required]
        [Display(Name = "Margin Type")]
        public string MarginType { get; set; }

        [Required]
        [Display(Name = "Margin Formula")]
        public string Formula { get; set; }
    }
}
