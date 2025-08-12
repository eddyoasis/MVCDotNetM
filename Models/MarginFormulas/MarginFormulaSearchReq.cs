using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCWebApp.Models.MarginFormulas
{
    public class MarginFormulaSearchReq : BaseSearchReq
    {
        public string Name { get; set; }
        public string Formula { get; set; }

        public List<SelectListItem> TypeSelections { get; set; }
        public int Type { get; set; }
    }
}
