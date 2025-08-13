using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models.AuditLogs
{
    public class AuditLog: IAuditInfo
    {
        [Key]
        public int ID { get; set; }
        public int TypeID { get; set; }
        public int ActionID { get; set; }
        public string Name { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public string? Remark { get; set; }
    }
}
