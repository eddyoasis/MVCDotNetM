//using Microsoft.EntityFrameworkCore;
//using MVCWebApp.Data;
//using MVCWebApp.Models.MarginCalls;

//namespace MVCWebApp.Repositories
//{
//    public interface IMarginCallEODRepository : IGenericRepository<MarginCall>
//    {
//        Task<List<MarginCallDto>> GetMarginCallEOD();
//    }

//    public class MarginCallEODRepository
//        (ApplicationDbContext _context) : GenericRepository<MarginCall>(_context), IMarginCallEODRepository
//    {
//        public async Task<List<MarginCallDto>> GetMarginCallEOD()
//        {
//            var result = _context.MarginCall
//               .FromSqlInterpolated($"exec [dbo].[USP_MarginCall_MTM_Eddy] @Mode=2")
//               .AsEnumerable()
//               .Select(x => new MarginCallDto
//               {
//                  VMAmt = double.TryParse(x.VM, out var vmAmt) ? vmAmt : 0.0
//               })
//               .ToList();

//            //var result = await _context.MarginCall
//            //    .FromSqlInterpolated($"exec [dbo].[USP_MarginCall_MTM_Eddy] @Mode=2")
//            //    .AsNoTracking()
//            //    .ToListAsync();

//            //var result = _context
//            //    .Set<MarginCall2>()
//            //    .FromSqlInterpolated($"exec [dbo].[USP_MarginCall_EOD] @Mode=2")
//            //    .AsEnumerable()
//            //    .Select(x => new MarginCall2
//            //    {
//            //        VM = 1
//            //    })
//            //    .ToList();

//            //var result = _context
//            //    .MarginCall
//            //    .FromSqlInterpolated($"exec [dbo].[USP_MarginCall_EOD] @Mode=2")
//            //    .AsQueryable()
//            //    .AsNoTracking();

//            return result;
//        }
//    }
//}
