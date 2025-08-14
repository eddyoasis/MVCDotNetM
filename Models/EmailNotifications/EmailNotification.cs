using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models.EmailNotifications
{
    public class EmailNotification : ISetUserInfo
    {
        [Key]
        public int ID { get; set; }
        public string MarginType { get; set; }
        public int TypeID { get; set; }
        public string EmailTemplate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
