using System.ComponentModel.DataAnnotations.Schema;

namespace MVCWebApp.Models.MarginCalls
{
    [Table("TBL_MarginCall_EOD")]
    public class MarginCallEOD
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
        public string? Type { get; set; }
        public string? Remarks { get; set; }
        public DateTime InsertedDatetime { get; set; }
        public DateTime? ModifiedDatetime { get; set; }
        public string Day { get; set; }
    }

    public class MarginCallEODDto : MarginCallDto
    {
    }
}
