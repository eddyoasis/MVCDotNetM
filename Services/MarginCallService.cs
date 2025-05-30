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
    }

    public class MarginCallService(
        IMarginCallRepository _marginCallRepository,
        IMapModel _mapper
        ) : BaseService, IMarginCallService
    {
        public async Task<IEnumerable<MarginCallViewModel>> GetAllAsync(MarginCallSearchReq req)
        {
            var marginCalls = _marginCallRepository.GetAllQueryable();

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
    }
}
