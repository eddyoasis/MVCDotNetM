namespace MVCWebApp.Models.MarginFormulas
{
    public class MarginFormulaSearchReq : BaseSearchReq
    {
        public string MarginType { get; set; }
        public string MarginFormula { get; set; }
    }
}
