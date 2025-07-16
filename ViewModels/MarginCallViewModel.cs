using Microsoft.AspNetCore.Mvc.Rendering;
using MVCWebApp.Models.MarginCalls;

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
        public List<string> EmailTemplateTitle { get; set; }
        public string EmailTemplateValue { get; set; }

    }
}
