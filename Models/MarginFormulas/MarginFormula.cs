using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models.MarginFormulas
{
    public class MarginFormula : ISetUserInfo
    {
        [Key]
        public int ID { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Formula { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
