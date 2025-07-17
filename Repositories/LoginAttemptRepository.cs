using MVCWebApp.Data;
using MVCWebApp.Models;

namespace MVCWebApp.Repositories
{
    public interface ILoginAttemptRepository : IGenericRepository<LoginAttempt>
    {
    }

    public class LoginAttemptRepository
        (ApplicationDbContext _context) : GenericRepository<LoginAttempt>(_context), ILoginAttemptRepository
    {
    }
}
