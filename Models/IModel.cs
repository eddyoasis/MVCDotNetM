namespace MVCWebApp.Models
{
    public interface ISetUserInfo
    {
        string CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
        string? ModifiedBy { get; set; }
        DateTime? ModifiedAt { get; set; }
    }

    public interface IAuditInfo
    {
        string CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }
    }

    public interface ISetUpdateInfo
    {
        string? ModifiedBy { get; set; }
        DateTime? ModifiedAt { get; set; }
    }
}
