using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCWebApp.Models.EmailNotifications
{
    public class AuditLogSearchReq : BaseSearchReq
    {
        public List<SelectListItem> TypeSelections { get; set; }
        public int TypeID { get; set; }

        public List<SelectListItem> ActionSelections { get; set; }
        public int ActionID { get; set; }

        public string Name { get; set; }
    }
}
