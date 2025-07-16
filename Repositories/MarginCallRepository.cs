using Microsoft.EntityFrameworkCore;
using MVCWebApp.Data;
using MVCWebApp.Models.MarginCalls;

namespace MVCWebApp.Repositories
{
    public interface IMarginCallRepository : IGenericRepository<MarginCall>
    {
        Task<bool> ApproveWithSP(string portfolioID);
        Task<MarginCall> GetByPortfolioID(string portfolioID);
        Task<IEnumerable<string>> GetAllCollateralCcy();
        Task<IEnumerable<string>> GetAllIMCcy();
        Task<IEnumerable<string>> GetAllVMCcy();
    }

    public class MarginCallRepository
        (ApplicationDbContext _context) : GenericRepository<MarginCall>(_context), IMarginCallRepository
    {
        public async Task<bool> ApproveWithSP(string portfolioID)
        {
            var updateCount = await _context
                .Database
                .ExecuteSqlRawAsync($"EXECUTE dbo.USP_MarginCall_Update_MarginCallFlag @PortfolioID={portfolioID}");

            return updateCount == 1;
        }

        public async Task<MarginCall> GetByPortfolioID(string portfolioID) =>
            await _context.MarginCall.FirstOrDefaultAsync(x => x.PortfolioID == portfolioID);

        public async Task<IEnumerable<string>> GetAllCollateralCcy() =>
            await _context.MarginCall
                .Select(x => x.Collateral_Ccy)
                .Distinct()
                .Where(ccy => !string.IsNullOrEmpty(ccy))
                .ToListAsync();

        public async Task<IEnumerable<string>> GetAllIMCcy() =>
            await _context.MarginCall
                .Select(x => x.IM_Ccy)
                .Distinct()
                .Where(ccy => !string.IsNullOrEmpty(ccy))
                .ToListAsync();

        public async Task<IEnumerable<string>> GetAllVMCcy() =>
            await _context.MarginCall
                .Select(x => x.VM_Ccy)
                .Distinct()
                .Where(ccy => !string.IsNullOrEmpty(ccy))
                .ToListAsync();

    }
}
