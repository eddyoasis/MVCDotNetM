namespace MVCWebApp.ViewModels
{
    public class MarginCallViewModel
    {
        public int ID { get; set; }
        public string ClientCode { get; set; }
        public string LedgerBal { get; set; }
        public string TNE { get; set; }
        public string IM { get; set; }
        public int Percentages { get; set; }
        public string CcyCode { get; set; }
        public string TypeOfMarginCall { get; set; }
        public string OrderDetails { get; set; }
        public DateTime TimeStemp { get; set; }
        public string Status { get; set; }
    }
}
