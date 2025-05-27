namespace MVCWebApp.Models.Req
{
    public class BaseSearchReq
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    //public class SelectListItem
    //{
    //    public string Text { get; set; }
    //    public int Value { get; set; }
    //}
}
