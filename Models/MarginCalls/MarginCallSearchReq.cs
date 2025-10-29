using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCWebApp.Models.MarginCalls
{
    public class MarginCallSearchReq : BaseSearchReq
    {
        public string PortfolioID { get; set; }
        public double PercentagesFrom { get; set; }
        public double PercentagesTo { get; set; }
        public double CollateralFrom { get; set; }
        public double CollateralTo { get; set; }
        public List<SelectListItem> Collateral_Ccy { get; set; }
        public string Selected_Collateral_Ccy { get; set; }
        public double VMFrom { get; set; }
        public double VMTo { get; set; }
        public List<SelectListItem> VM_Ccy { get; set; }
        public string Selected_VM_Ccy { get; set; }
        public double IMFrom { get; set; }
        public double IMTo { get; set; }
        public List<SelectListItem> IM_Ccy { get; set; }
        public string Selected_IM_Ccy { get; set; }
        public double TNEFrom { get; set; }
        public double TNETo { get; set; }
        public List<SelectListItem> TNE_Ccy { get; set; }
        public string Selected_TNE_Ccy { get; set; }
        public List<SelectListItem> MarginCallFlag { get; set; }
        public int SelectedMarginCallFlag { get; set; }
        public List<SelectListItem> EODTriggerFlag { get; set; }
        public int SelectedEODTriggerFlag { get; set; }
        public List<SelectListItem> MTMTriggerFlag { get; set; }
        public int SelectedMTMTriggerFlag { get; set; }
        public int SearchByStatusType { get; set; }
        public List<SelectListItem> Day { get; set; }
        public int SelectedDay { get; set; }
        public string? Type { get; set; }
        public string? Remarks { get; set; }
        public DateTime InsertedDatetime { get; set; }
        public DateTime? ModifiedDatetime { get; set; }


        public List<SelectListItem> MarginCallOrderByColumns { get; set; }
        public int Selected_MarginCallOrderByColumn { get; set; }
        public int Selected_MarginMode { get; set; }
    }
}
