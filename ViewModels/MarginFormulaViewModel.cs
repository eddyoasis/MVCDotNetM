using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.ViewModels
{
    public class MarginFormulaViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Type")]
        public string Type { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Formula")]
        public string Formula { get; set; }

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
