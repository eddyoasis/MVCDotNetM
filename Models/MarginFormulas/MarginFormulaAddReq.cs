using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models.MarginFormulas
{
    public class MarginFormulaAddReq
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Formula")]
        public string Formula { get; set; }

        [Display(Name = "Type")]
        public List<SelectListItem> TypeSelections { get; set; } = new List<SelectListItem>();

        public int Type { get; set; }
    }
}
