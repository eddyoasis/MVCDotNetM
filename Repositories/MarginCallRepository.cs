using Microsoft.EntityFrameworkCore;
using MVCWebApp.Data;
using MVCWebApp.Enums;
using MVCWebApp.Models.MarginCalls;

namespace MVCWebApp.Repositories
{
    public interface IMarginCallRepository : IGenericRepository<MarginCall>
    {
        Task<bool> ApproveMarginCallMTM(string portfolioID);
        IEnumerable<MarginCallDto> GetMarginCallMTM(MarginCallMode mode);
        MarginCallDto GetMarginCallMTM(string portfolioID);

        Task<bool> ApproveMarginCallEOD(string portfolioID);
        IEnumerable<MarginCallDto> GetMarginCallEOD(MarginCallMode mode);
        MarginCallDto GetMarginCallEOD(string portfolioID);

        //Task<bool> ApproveWithSP(string portfolioID);
        //Task<MarginCall> GetByPortfolioID(string portfolioID);

        Task<IEnumerable<string>> GetAllCollateralCcyMTM();
        Task<IEnumerable<string>> GetAllIMCcyMTM();
        Task<IEnumerable<string>> GetAllVMCcyMTM();

        Task<IEnumerable<string>> GetAllCollateralCcy();
        Task<IEnumerable<string>> GetAllIMCcy();
        Task<IEnumerable<string>> GetAllVMCcy();
    }

    public class MarginCallRepository
        (ApplicationDbContext _context) : GenericRepository<MarginCall>(_context), IMarginCallRepository
    {
        /*-------------------------------------------------    MTM     ----------*/
        public async Task<bool> ApproveMarginCallMTM(string portfolioID)
        {
            var updateCount = await _context
                .Database
                .ExecuteSqlRawAsync($"exec [dbo].[USP_MarginCall_MTM_Update] @PortfolioID='{portfolioID}'");

            return updateCount == 1;
        }

        public IEnumerable<MarginCallDto> GetMarginCallMTM(MarginCallMode mode)
        {
            var result = _context.MarginCallMTM
               .FromSqlInterpolated($"exec [dbo].[USP_MarginCall_MTM] @Mode={(int)mode}")
               .AsEnumerable()
               .Select(ToMarginCallDTO);

            return result;
        }

        public MarginCallDto GetMarginCallMTM(string portfolioID)
        {
            var result = _context.MarginCallMTM
               .FromSqlInterpolated($"exec [dbo].[USP_MarginCall_MTM_ID] @PortfolioID={portfolioID}")
               .AsEnumerable()
               .Select(ToMarginCallDTO)
               .FirstOrDefault();

            return result;
        }

        /*-------------------------------------------------    EOD     ----------*/
        public async Task<bool> ApproveMarginCallEOD(string portfolioID)
        {
            var updateCount = await _context
                .Database
                .ExecuteSqlRawAsync($"exec [dbo].[USP_MarginCall_EOD_Update] @PortfolioID='{portfolioID}'");

            return updateCount == 1;
        }

        public IEnumerable<MarginCallDto> GetMarginCallEOD(MarginCallMode mode)
        {
            var result = _context.MarginCallEOD
               .FromSqlInterpolated($"exec [dbo].[USP_MarginCall_EOD] @Mode={(int)mode}")
               .AsEnumerable()
               .Select(ToMarginCallDTO);

            return result;
        }

        public MarginCallDto GetMarginCallEOD(string portfolioID)
        {
            var result = _context.MarginCallEOD
               .FromSqlInterpolated($"exec [dbo].[USP_MarginCall_EOD_ID] @PortfolioID={portfolioID}")
               .AsEnumerable()
               .Select(ToMarginCallDTO)
               .FirstOrDefault();

            return result;
        }

        /*-------------------------------------------------    General     ----------*/
        public async Task<bool> ApproveWithSP(string portfolioID)
        {
            var updateCount = await _context
                .Database
                .ExecuteSqlRawAsync($"EXECUTE dbo.USP_MarginCall_Update_MarginCallFlag @PortfolioID='{portfolioID}'");

            return updateCount == 1;
        }

        public async Task<MarginCall> GetByPortfolioID(string portfolioID) =>
            await _context.MarginCall.FirstOrDefaultAsync(x => x.PortfolioID == portfolioID);

        public async Task<IEnumerable<string>> GetAllCollateralCcyMTM() =>
            await _context.MarginCallMTM
                .Select(x => x.Collateral_Ccy)
                .Distinct()
                .Where(ccy => !string.IsNullOrEmpty(ccy))
                .ToListAsync();

        public async Task<IEnumerable<string>> GetAllIMCcyMTM() =>
            await _context.MarginCallMTM
                .Select(x => x.IM_Ccy)
                .Distinct()
                .Where(ccy => !string.IsNullOrEmpty(ccy))
                .ToListAsync();

        public async Task<IEnumerable<string>> GetAllVMCcyMTM() =>
            await _context.MarginCallMTM
                .Select(x => x.VM_Ccy)
                .Distinct()
                .Where(ccy => !string.IsNullOrEmpty(ccy))
                .ToListAsync();

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

        /*-------------------------------------------------    Private     ----------*/
        public MarginCallDto ToMarginCallDTO(MarginCallMTM entity) //MTM
        {
            return new MarginCallDto
            {
                VM = double.TryParse(entity.VM, out var vm) ? vm : 0.0,
                IM = double.TryParse(entity.IM, out var im) ? im : 0.0,
                Percentages = double.TryParse(entity.Percentages.Replace("%", ""), out var percentages) ? percentages : 0.0,
                Collateral = double.TryParse(entity.Collateral, out var collateral) ? collateral : 0.0,
                IM_Ccy = entity.IM_Ccy,
                Collateral_Ccy = entity.Collateral_Ccy,
                InsertedDatetime = entity.InsertedDatetime,
                ModifiedDatetime = entity.ModifiedDatetime,
                PortfolioID = entity.PortfolioID,
                Remarks = entity.Remarks,
                Type = entity.Type,
                VM_Ccy = entity.VM_Ccy,
                MarginCallAmount = entity.MarginCallAmount,
                MarginCallTriggerFlag = entity.MarginCallTriggerFlag == true,
                StoplossTriggerFlag = entity.StoplossTriggerFlag == true,
                MOCTriggerFlag = entity.MOCTriggerFlag == true,
                MarginCallTriggerDatetime = DateTime.Now.AddDays(-1),
                StoplossTriggerDatetime = entity.StoplossTriggerDatetime,
                MOCTriggerDatetime = entity.MOCTriggerDatetime,
                EmailTo = entity.EmailTo
            };
        }

        public MarginCallDto ToMarginCallDTO(MarginCallEOD entity) //EOD
        {
            return new MarginCallDto
            {
                VM = double.TryParse(entity.VM, out var vm) ? vm : 0.0,
                IM = double.TryParse(entity.IM, out var im) ? im : 0.0,
                Percentages = double.TryParse(entity.Percentages.Replace("%", ""), out var percentages) ? percentages : 0.0,
                Collateral = double.TryParse(entity.Collateral, out var collateral) ? collateral : 0.0,
                IM_Ccy = entity.IM_Ccy,
                Collateral_Ccy = entity.Collateral_Ccy,
                InsertedDatetime = entity.InsertedDatetime,
                ModifiedDatetime = entity.ModifiedDatetime,
                PortfolioID = entity.PortfolioID,
                Remarks = entity.Remarks,
                Type = entity.Type,
                VM_Ccy = entity.VM_Ccy,
                MarginCallAmount = entity.MarginCallAmount,
                MarginCallTriggerFlag = entity.MarginCallTriggerFlag != true,
                StoplossTriggerFlag = entity.StoplossTriggerFlag == true,
                MOCTriggerFlag = entity.MOCTriggerFlag == true,
                MarginCallTriggerDatetime = DateTime.Now.AddDays(-1),
                StoplossTriggerDatetime = entity.StoplossTriggerDatetime,
                MOCTriggerDatetime = entity.MOCTriggerDatetime,
                Day = entity.Day
            };
        }
    }
}
