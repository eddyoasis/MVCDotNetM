using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVCWebApp.Models.EmailNotifications
{
    public class EmailNotificationSearchReq: BaseSearchReq
    {
        public string MarginType { get; set; }
        public string EmailTemplate { get; set; }

        public List<SelectListItem> TypeSelections { get; set; }
        public int TypeID { get; set; }
    }
}
