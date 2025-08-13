using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.ViewModels
{
    public class AuditLogViewModel
    {
        public int ID { get; set; }

        public string Type { get; set; }
        public int TypeID { get; set; }
        public string Action { get; set; }
        public int ActionID { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? Remark { get; set; }
    }
}
