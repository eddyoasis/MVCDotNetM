using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models.EmailNotifications
{
    public class EmailNotificationAddReq
    {
        [Required]
        [Display(Name = "Margin Type")]
        public string MarginType { get; set; }

        [Required]
        [Display(Name = "Email Template")]
        public string EmailTemplate { get; set; }
    }
}
