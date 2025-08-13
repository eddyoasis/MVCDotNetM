using MVCWebApp.Data;
using MVCWebApp.Models;
using MVCWebApp.Models.AuditLogs;

namespace MVCWebApp.Repositories
{
    public interface IAuditLogRepository : IGenericRepository<AuditLog>
    {
    }

    public class AuditLogRepository
        (ApplicationDbContext _context) : GenericRepository<AuditLog>(_context), IAuditLogRepository
    {
    }
}
