using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCWebApp.Models.MarginCalls
{
    [Table("TBL_MarginCall")]
    public class MarginCall
    {
        //[Key]
        //public int ID { get; set; }
        public string PortfolioID { get; set; }
        public decimal Percentages { get; set; }
        public double Collateral { get; set; }
        public string Collateral_Ccy { get; set; }
        public double VM { get; set; }
        public string VM_Ccy { get; set; }
        public double IM { get; set; }
        public string IM_Ccy { get; set; }
        public string? MarginCallFlag { get; set; }
        public bool? EODTriggerFlag { get; set; }
        public bool? MTMTriggerFlag { get; set; }
        public string? Type { get; set; }
        public string? Remarks { get; set; }
        public DateTime InsertedDatetime { get; set; }
        public DateTime? ModifiedDatetime { get; set; }
    }
}
