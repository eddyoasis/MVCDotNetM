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
        public string Percentages { get; set; }
        public string Collateral { get; set; }
        public string Collateral_Ccy { get; set; }
        public double TNE { get; set; }
        public string TNE_Ccy { get; set; }
        public string VM { get; set; }
        public string VM_Ccy { get; set; }
        public double IM { get; set; }
        public string IMProduct { get; set; }
        public string IM_Ccy { get; set; }
        public double MarginCallAmount { get; set; }
        public bool MarginCallFlag { get; set; }
        public bool StoplossFlag { get; set; }
        public bool MOCFlag { get; set; }
        public bool? EODTriggerFlag { get; set; }
        public bool? MTMTriggerFlag { get; set; }
        public bool MarginCallTriggerFlag { get; set; }
        public bool StoplossTriggerFlag { get; set; }
        public bool MOCTriggerFlag { get; set; }
        public bool IsAvailableReset { get; set; }
        public string? Type { get; set; }
        public string? Remarks { get; set; }
        public DateTime InsertedDatetime { get; set; }
        public DateTime? ModifiedDatetime { get; set; }
        public DateTime? MarginCallTriggerDatetime { get; set; }
        public DateTime? StoplossTriggerDatetime { get; set; }
        public DateTime? MOCTriggerDatetime { get; set; }
        public string Day { get; set; }
    }

    public class MarginCallDto
    {

        public string PortfolioID { get; set; }
        public double Percentages { get; set; }
        public double Collateral { get; set; }
        public string Collateral_Ccy { get; set; }
        public double TNE { get; set; }
        public string TNE_Ccy { get; set; }
        public double VM { get; set; }
        public string VM_Ccy { get; set; }
        public double IM { get; set; }
        public string IMProduct { get; set; }
        public string IM_Ccy { get; set; }
        public double MarginCallAmount { get; set; }
        public string? Type { get; set; }
        public string? Remarks { get; set; }
        public bool MarginCallFlag { get; set; }
        public bool StoplossFlag { get; set; }
        public bool MOCFlag { get; set; }
        public bool MarginCallTriggerFlag { get; set; }
        public bool StoplossTriggerFlag { get; set; }
        public bool MOCTriggerFlag { get; set; }
        public bool IsAvailableReset { get; set; }
        public DateTime InsertedDatetime { get; set; }
        public DateTime? ModifiedDatetime { get; set; }
        public DateTime? MarginCallTriggerDatetime { get; set; }
        public DateTime? StoplossTriggerDatetime { get; set; }
        public DateTime? MOCTriggerDatetime { get; set; }
        public string Day { get; set; }
        public string? EmailTo { get; set; }
    }

    public class ClientEmailDBResult
    {
        public string Portfolio { get; set; }
        public string Email { get; set; }
    }

    public class StoplossOrderDetailDBResult
    {
        public string? PortfolioID { get; set; }
        public string? Action { get; set; }
    }

    public class MOCOrderDetailDBResult
    {
        public string? PortfolioID { get; set; }
        public string? Remarks { get; set; }
    }

    public class IMProductMTMDBResult
    {
        public string PortfolioId { get; set; }
        public string? ExchangeCode { get; set; }
        public string? ContractCode { get; set; }
        public string? CCY { get; set; }
        public decimal? IMProduct { get; set; }
    }

    public class IMProductEODDBResult
    {
        public string PortfolioId { get; set; }
        public string? ExchangeCode { get; set; }
        public string? ContractCode { get; set; }
        public string? CCY { get; set; }
        public decimal? IMProduct { get; set; }
    }
}
