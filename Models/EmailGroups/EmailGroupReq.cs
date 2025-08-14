using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models.EmailGroups
{
    public class EmailGroupAddReq
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "EmailTo")]
        public string? EmailTo { get; set; }

        [Display(Name = "EmailCC")]
        public string? EmailCC { get; set; }

        [Display(Name = "Type")]
        public List<SelectListItem> TypeSelections { get; set; } = new List<SelectListItem>();

        public int TypeID { get; set; }
    }

    public class EmailGroupEditReq : EmailGroupAddReq
    {
        public int ID { get; set; }
    }

    public class EmailGroupSearchReq : BaseSearchReq
    {
        public string Name { get; set; }
        public string EmailTo { get; set; }
        public string EmailCC { get; set; }

        public List<SelectListItem> TypeSelections { get; set; }
        public int TypeID { get; set; }
    }
}
