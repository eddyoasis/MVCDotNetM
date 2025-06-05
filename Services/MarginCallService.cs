using MVCWebApp.Enums;
using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models;
using MVCWebApp.Models.MarginCalls;
using MVCWebApp.Repositories;
using MVCWebApp.ViewModels;

namespace MVCWebApp.Services
{
    public interface IMarginCallService
    {
        Task<IEnumerable<MarginCallViewModel>> GetAllAsync(MarginCallSearchReq req);
        Task<MarginCall?> GetByEntityIdAsync(int id);
        Task<MarginCallViewModel?> GetByIdAsync(int id);
        Task<bool> ApproveAsync(MarginCall entity);
        Task<bool> RejectAsync(MarginCall entity);
    }

    public class MarginCallService(
        IMarginCallRepository _marginCallRepository,
        IMapModel _mapper
        ) : BaseService, IMarginCallService
    {
        public async Task<IEnumerable<MarginCallViewModel>> GetAllAsync(MarginCallSearchReq req)
        {
            var marginCalls = _marginCallRepository.GetAllQueryable();

            marginCalls = marginCalls.Where(x =>
                ((req.DateFrom == DateTime.MinValue || req.DateTo == DateTime.MinValue) ||
                    (req.IsSearchByCreatedDate ?
                        x.TimeStemp >= req.DateFrom && x.TimeStemp < req.DateTo.AddDays(1) :
                        x.ModifiedAt >= req.DateFrom && x.ModifiedAt < req.DateTo.AddDays(1))) &&
                (req.ModifiedBy.IsNullOrEmpty() || x.ModifiedBy.ToLower().Contains(req.ModifiedBy.ToLower())) &&
                (req.ClientCode.IsNullOrEmpty() || x.ClientCode.ToLower().Contains(req.ClientCode.ToLower())) &&
                (req.LedgerBal.IsNullOrEmpty() || x.LedgerBal.ToLower().Contains(req.LedgerBal.ToLower())) &&
                (req.TNE.IsNullOrEmpty() || x.TNE.ToLower().Contains(req.TNE.ToLower())) &&
                (req.IM.IsNullOrEmpty() || x.IM.ToLower().Contains(req.IM.ToLower())) &&
                (req.Percentages <= 0 || x.Percentages == req.Percentages) &&
                (req.SelectedCcyCode==0 || req.SelectedCcyCode == x.CcyCode) &&
                (req.TypeOfMarginCall.IsNullOrEmpty() || x.TypeOfMarginCall.ToLower().Contains(req.TypeOfMarginCall.ToLower())) &&
                (req.OrderDetails.IsNullOrEmpty() || x.OrderDetails.ToLower().Contains(req.OrderDetails.ToLower())) &&
                (req.SelectedStatus == 0 || req.SelectedStatus == x.Status)
            );

            var searchReq = _mapper.MapDto<MarginCallSearchReq, BaseSearchReq>(req);

            var paginatedResult = await PaginatedList<MarginCallViewModel>.
                    GetByPagesAsync<MarginCall, MarginCallViewModel>(
                        marginCalls,
                        _mapper,
                        searchReq);

            return paginatedResult;
        }

        public async Task<MarginCall?> GetByEntityIdAsync(int id)
        {
            return await _marginCallRepository.GetByIdAsync(id);
        }

        public async Task<MarginCallViewModel?> GetByIdAsync(int id)
        {
            var entity = await _marginCallRepository.GetByIdAsync(id);

            return _mapper.MapDto<MarginCallViewModel>(entity);
        }

        public async Task<bool> ApproveAsync(MarginCall entity)
        {
            try
            {
                entity.Status = (int)MarginCallSearchStatusEnum.Approved;

                _mapper.Map(entity, Username);

                await _marginCallRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        public async Task<bool> RejectAsync(MarginCall entity)
        {
            try
            {
                entity.Status = (int)MarginCallSearchStatusEnum.Rejected;

                _mapper.Map(entity, Username);

                await _marginCallRepository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }
    }
}
