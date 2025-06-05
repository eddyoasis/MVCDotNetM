using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models.MarginCalls
{
    public class MarginCall: ISetUpdateInfo
    {
        [Key]
        public int ID { get; set; }
        public string ClientCode { get; set; }
        public string LedgerBal { get; set; }
        public string TNE { get; set; }
        public string IM { get; set; }
        public int Percentages { get; set; }
        public int CcyCode { get; set; }
        public string TypeOfMarginCall { get; set; }
        public string OrderDetails { get; set; }
        public DateTime TimeStemp { get; set; }
        public int Status { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
