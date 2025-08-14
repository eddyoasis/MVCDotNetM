using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.ViewModels
{
    public class EmailGroupViewModel
    {
        public int ID { get; set; }


        [Required]
        [Display(Name = "Type")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "EmailTo")]
        public string EmailTo { get; set; }

        [Required]
        [Display(Name = "EmailCC")]
        public string EmailCC { get; set; }

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
