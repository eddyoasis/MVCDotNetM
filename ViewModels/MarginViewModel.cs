using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.ViewModels
{
    public class MarginViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Percentage must be greater than 0.")]
        [Display(Name = "Percentage")]
        public int Percentage { get; set; }

        [Display(Name = "Status")]
        public int Status { get; set; }
    }
}
