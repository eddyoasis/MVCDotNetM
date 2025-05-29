using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.ViewModels
{
    public class EmailNotificationViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Margin Type")]
        public string MarginType { get; set; }

        [Required]
        [Display(Name = "Email Template")]
        public string EmailTemplate { get; set; }

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
