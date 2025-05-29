using MVCWebApp.Data;
using MVCWebApp.Models.MarginFormulas;

namespace MVCWebApp.Repositories
{
    public interface IMarginFormulaRepository: IGenericRepository<MarginFormula>
    {
        IQueryable<MarginFormula> GetAllQueryable();
    }

    public class MarginFormulaRepository
        (ApplicationDbContext _context) : GenericRepository<MarginFormula>(_context), IMarginFormulaRepository
    {
        public IQueryable<MarginFormula> GetAllQueryable()
        {
            return _context.MarginFormulas.AsQueryable();
        }
    }
}
