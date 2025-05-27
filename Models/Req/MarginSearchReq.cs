namespace MVCWebApp.Models.Req
{
    public class MarginSearchReq
    {
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string Username { get; set; }

        public int? Percentage { get; set; }
    }
}
