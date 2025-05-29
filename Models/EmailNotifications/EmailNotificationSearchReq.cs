namespace MVCWebApp.Models.EmailNotifications
{
    public class EmailNotificationSearchReq: BaseSearchReq
    {
        public string MarginType { get; set; }
        public string EmailTemplate { get; set; }
    }
}
