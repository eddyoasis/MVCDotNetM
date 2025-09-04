using Microsoft.AspNetCore.Mvc.Rendering;
using MVCWebApp.Models.EmailGroups;
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
        public string StoplossOrderDetail { get; set; }

        public List<SelectListItem> EmailTemplateList { get; set; }

        [Display(Name = "EmailTemplate Selection")]
        public List<string> EmailTemplateTitle { get; set; }

        [Display(Name = "EmailTemplate Subject")]
        public string EmailTemplateSubject { get; set; }

        [Display(Name = "EmailTemplate Body")]
        public string EmailTemplateValue { get; set; }


        public List<EmailGroup> EmailGroupSelections { get; set; }

        [Display(Name = "Email To")]
        public string EmailTo { get; set; }

        [Display(Name = "Email CC")]
        public string EmailCC { get; set; }
    }
}
