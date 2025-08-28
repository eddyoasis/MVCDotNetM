using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCWebApp.Models.MarginCalls
{
    [Table("TBL_MarginCall_MTM")]
    public class MarginCallMTM
    {
        //[Key]
        //public int ID { get; set; }
        public string PortfolioID { get; set; }
        public string Percentages { get; set; }
        public string Collateral { get; set; }
        public string Collateral_Ccy { get; set; }
        public string VM { get; set; }
        public string VM_Ccy { get; set; }
        public string IM { get; set; }
        public string IM_Ccy { get; set; }
        public double MarginCallAmount { get; set; }
        public string? MarginCallFlag { get; set; }
        public bool? EODTriggerFlag { get; set; }
        public bool? MTMTriggerFlag { get; set; }
        public bool? MarginCallTriggerFlag { get; set; }
        public bool? StoplossTriggerFlag { get; set; }
        public bool? MOCTriggerFlag { get; set; }
        public string? Type { get; set; }
        public string? Remarks { get; set; }
        public string? EmailTo { get; set; }
        public DateTime InsertedDatetime { get; set; }
        public DateTime? ModifiedDatetime { get; set; }
        public DateTime? MarginCallTriggerDatetime { get; set; }
        public DateTime? StoplossTriggerDatetime { get; set; }
        public DateTime? MOCTriggerDatetime { get; set; }
    }

    public class MarginCallMTMDto : MarginCallDto
    {
    }
}
