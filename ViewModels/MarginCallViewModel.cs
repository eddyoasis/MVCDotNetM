using Microsoft.AspNetCore.Mvc.Rendering;
using MVCWebApp.Models.MarginCalls;
using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.ViewModels
{
    public interface IMarginCallViewModel
    {
        public string Status { get; set; }
        public string CcyCode { get; set; }
    }

    public class MarginCallViewModel : MarginCall
    {
        public List<SelectListItem> EmailTemplateList { get; set; }

        [Display(Name = "EmailTemplate Selection")]
        public List<string> EmailTemplateTitle { get; set; }

        [Display(Name = "EmailTemplate Body")]
        public string EmailTemplateValue { get; set; }

    }
}
