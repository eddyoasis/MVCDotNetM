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
        public string EODTriggerFlag { get; set; }
        public string StoplossFlag { get; set; }
        public string MOCFlag { get; set; }
        public string? Type { get; set; }
        public string? Remarks { get; set; }
        public DateTime InsertedDatetime { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        [Column("EODTriggerDatetime")]
        public DateTime? EODTriggerDatetime { get; set; }

        [Column("StopLossDatetime")]
        public DateTime? StopLossDatetime { get; set; }

        [Column("MOCDatetime")]
        public DateTime? MOCDatetime { get; set; }

        public string Day { get; set; }
        public string? EmailTo { get; set; }
    }

    public class MarginCallEODDto : MarginCallDto
    {
    }
}
