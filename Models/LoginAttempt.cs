using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Models
{
    public class LoginAttempt
    {
        [Key]
        public int ID { get; set; }
        public string Username { get; set; }
        public DateTime TimeStemp { get; set; }
        public bool IsSuccess { get; set; }
        public string Remark { get; set; }

    }
}
