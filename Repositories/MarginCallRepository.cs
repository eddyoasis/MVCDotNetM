using MVCWebApp.Data;
using MVCWebApp.Models.MarginCalls;
using MVCWebApp.Models.MarginFormulas;

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
