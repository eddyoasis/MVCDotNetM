using MVCWebApp.Data;
using MVCWebApp.Models.MarginCalls;

namespace MVCWebApp.Repositories
{
    public interface IMarginCallRepository : IGenericRepository<MarginCall>
    {

    }

    public class MarginCallRepository
        (ApplicationDbContext _context) : GenericRepository<MarginCall>(_context), IMarginCallRepository
    {
    }
}
