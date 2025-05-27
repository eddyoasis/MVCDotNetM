using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models
{
    public class MarginFormulaViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Margin Type")]
        public string MarginType { get; set; }

        [Display(Name = "Margin Formula")]
        public string MarginFormula { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified At")]
        public DateTime? ModifiedAt { get; set; }
    }
}
