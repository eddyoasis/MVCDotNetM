namespace MVCWebApp.Models
{
    public class BaseSearchReq
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public bool IsSearchByCreatedDate { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo{ get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }
}
