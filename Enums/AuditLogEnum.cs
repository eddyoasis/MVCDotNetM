using System.ComponentModel.DataAnnotations;

namespace MVCWebApp.Enums
{
    public enum AuditLogTypeEnum
    {
        All = 0,
        AutoMarginMTM = 1,
        AutoMarginEOD = 2,
        EmailNotifcation = 3,
        Formula = 4,
        EmailGroup = 5
    }

    public enum AuditLogActionEnum
    {
        All = 0,
        Create = 1,
        Edit = 2,
        Delete = 3,
        Approve = 4,
        ApproveStoploss = 5,
        ApproveMOC = 6
    }
}
