using MVCWebApp.Data;
using MVCWebApp.Models.EmailGroups;

namespace MVCWebApp.Repositories
{
    public interface IEmailGroupRepository : IGenericRepository<EmailGroup>
    {
    }

    public class EmailGroupRepository
        (ApplicationDbContext _context) : GenericRepository<EmailGroup>(_context), IEmailGroupRepository
    {
    }
}
