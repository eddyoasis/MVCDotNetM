using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models.EmailNotifications
{
    public class EmailNotificationAddReq
    {
        [Required]
        [Display(Name = "Name")]
        public string MarginType { get; set; }

        [Required]
        [Display(Name = "Email Template")]
        public string EmailTemplate { get; set; }

        [Display(Name = "Type")]
        public List<SelectListItem> TypeSelections { get; set; } = new List<SelectListItem>();

        public int TypeID { get; set; }
    }
}
