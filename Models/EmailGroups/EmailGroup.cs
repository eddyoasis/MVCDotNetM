using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models.EmailGroups
{
    public class EmailGroup : ISetUserInfo
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int TypeID { get; set; }
        public string? EmailTo { get; set; }
        public string? EmailCC { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
