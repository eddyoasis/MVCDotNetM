using Microsoft.EntityFrameworkCore;
using MVCWebApp.Data;
using MVCWebApp.Enums;
using MVCWebApp.Models.MarginCalls;

namespace MVCWebApp.Repositories
{
    public interface IMarginCallRepository : IGenericRepository<MarginCall>
    {
        IEnumerable<IMProductMTMDBResult> GetMTMIMProduct(string portfolioID);
        Task<bool> ResetFlagMTM(string portfolioID, string user);
        Task<bool> ApproveMarginCallMTM(string portfolioID, int approvalType);
        IEnumerable<MarginCallDto> GetMarginCallMTM(MarginCallMode mode, string portfolioID = null, string user = null);
        MarginCallDto GetMarginCallMTM(string portfolioID);

        IEnumerable<IMProductEODDBResult> GetEODIMProduct(string portfolioID);
        Task<bool> ResetFlagEOD(string portfolioID, string user);
        Task<bool> ApproveMarginCallEOD(string portfolioID, int approvalType);
        IEnumerable<MarginCallDto> GetMarginCallEOD(MarginCallMode mode, string portfolioID = null, string user = null);
        MarginCallDto GetMarginCallEOD(string portfolioID);

        StoplossOrderDetailDBResult GetStoplossOrderDetail(string portfolioID, bool isMTM);
        ClientEmailDBResult GetClientEmail(string portfolioID);

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
        public IEnumerable<IMProductMTMDBResult> GetMTMIMProduct(string portfolioID)
        {
            var result = _context.IMProductMTMDBResult
               .FromSqlInterpolated($"exec [dbo].[USP_IMProduct] @Mode=1")
               .AsEnumerable()
               .Where(x => x.PortfolioId == portfolioID);

            return result;
        }

        public async Task<bool> ResetFlagMTM(string portfolioID, string user)
        {
            try
            {
                var updateCount = await _context
               .Database
               .ExecuteSqlRawAsync($"exec [dbo].[USP_MarginCall_MTM] @Mode={(int)MarginCallMode.ResetFlag}, @ClientCode='{portfolioID}', @User='{user}'");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ApproveMarginCallMTM(string portfolioID, int approvalType)
        {
            var updateCount = await _context
                .Database
                .ExecuteSqlRawAsync($"exec [dbo].[USP_MarginCall_MTM_Update] @PortfolioID='{portfolioID}', @ApprovalType={approvalType}");

            return updateCount == 1;
        }

        public IEnumerable<MarginCallDto> GetMarginCallMTM(MarginCallMode mode, string portfolioID = null, string user = null)
        {
            var result = _context.MarginCallMTM
               .FromSqlInterpolated($"exec [dbo].[USP_MarginCall_MTM] @Mode={(int)mode}, @ClientCode={portfolioID}, @User={user}")
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
        public IEnumerable<IMProductEODDBResult> GetEODIMProduct(string portfolioID)
        {
            var result = _context.IMProductEODDBResult
               .FromSqlInterpolated($"exec [dbo].[USP_IMProduct] @Mode=2")
               .AsEnumerable()
               .Where(x => x.PortfolioId == portfolioID);

            return result;
        }

        public async Task<bool> ResetFlagEOD(string portfolioID, string user)
        {
            try
            {
                var updateCount = await _context
               .Database
               .ExecuteSqlRawAsync($"exec [dbo].[USP_MarginCall_EOD] @Mode={(int)MarginCallMode.ResetFlag}, @ClientCode='{portfolioID}', @User='{user}'");

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ApproveMarginCallEOD(string portfolioID, int approvalType)
        {
            var updateCount = await _context
                .Database
                .ExecuteSqlRawAsync($"exec [dbo].[USP_MarginCall_EOD_Update] @PortfolioID='{portfolioID}', @ApprovalType={approvalType}");

            return updateCount == 1;
        }

        public IEnumerable<MarginCallDto> GetMarginCallEOD(MarginCallMode mode, string portfolioID = null, string user = null)
        {
            var result = _context.MarginCallEOD
               .FromSqlInterpolated($"exec [dbo].[USP_MarginCall_EOD] @Mode={(int)mode}, @ClientCode={portfolioID}, @User={user}")
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
        

        public StoplossOrderDetailDBResult GetStoplossOrderDetail(string portfolioID, bool isMTM)
        {
            var result = _context.StoplossOrderDetailDBResult
               .FromSqlInterpolated($"exec [dbo].[USP_StopLoss] @Mode={(isMTM ? 3 : 4)}")
               .AsEnumerable()
               .Where(x => x.PortfolioID == portfolioID);

            var action = result.Any() ?
                string.Join(Environment.NewLine, result.Select(x => x.Action)) : "-";

            return new StoplossOrderDetailDBResult { Action = action };
        }

        public ClientEmailDBResult GetClientEmail(string portfolioID)
        {
            var result = _context.ClientEmailDBResult
               .FromSqlInterpolated($"exec [dbo].[USP_Email_Recipients] @PortfolioID={portfolioID}")
               .AsEnumerable()
               .Select(x => new ClientEmailDBResult
               {
                   Portfolio = x.Portfolio,
                   Email = x.Email.Replace(";", "\r\n")
               })
               .FirstOrDefault();

            return result;
        }

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
                MarginCallTriggerFlag = entity.MTMTriggerFlag == "Y",
                StoplossTriggerFlag = entity.StoplossFlag == "Y",
                IsAvailableReset = entity.MTMTriggerFlag == "Y" || entity.StoplossFlag == "Y",
                MarginCallTriggerDatetime = entity.MTMTriggerDatetime,
                StoplossTriggerDatetime = entity.StopLossDatetime,
                EmailTo = entity.EmailTo,
                IMProduct = "IM Product"
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
                MarginCallTriggerFlag = entity.EODTriggerFlag == "Y",
                StoplossTriggerFlag = entity.StoplossFlag == "Y",
                MOCTriggerFlag = entity.MOCFlag == "Y",
                IsAvailableReset = entity.EODTriggerFlag == "Y" || entity.StoplossFlag == "Y" || entity.MOCFlag == "Y",
                MarginCallTriggerDatetime = entity.EODTriggerDatetime,
                StoplossTriggerDatetime = entity.StopLossDatetime,
                MOCTriggerDatetime = entity.MOCDatetime,
                Day = entity.Day,
                IMProduct = "IM Product"
            };
        }
    }
}
