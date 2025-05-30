using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models;
using MVCWebApp.Models.MarginFormulas;
using MVCWebApp.Repositories;
using MVCWebApp.ViewModels;

namespace MVCWebApp.Services
{
    public interface IMarginFormulaService
    {
        Task<IEnumerable<MarginFormulaViewModel>> GetAllAsync(MarginFormulaSearchReq req);
        Task<MarginFormula?> GetByEntityIdAsync(int id);
        Task<MarginFormulaViewModel?> GetByIdAsync(int id);
        Task AddAsync(MarginFormulaAddReq req);
        Task UpdateAsync(MarginFormulaEditReq req, MarginFormula entity);
        Task DeleteAsync(MarginFormula entity);
    }

    public class MarginFormulaService(
        IMarginFormulaRepository _marginFormulaRepository,
        IMapModel _mapper
        ) : BaseService, IMarginFormulaService
    {
        public async Task<IEnumerable<MarginFormulaViewModel>> GetAllAsync(MarginFormulaSearchReq req)
        {
            var marginFormulas = _marginFormulaRepository.GetAllQueryable();

            marginFormulas = marginFormulas.Where(x =>
                (req.MarginType.IsNullOrEmpty() || x.MarginType.ToLower().Contains(req.MarginType.ToLower())) &&
                (req.MarginFormula.IsNullOrEmpty() || x.Formula.ToLower().Contains(req.MarginFormula.ToLower()))
            );

            var searchReq = _mapper.MapDto<MarginFormulaSearchReq, BaseSearchReq>(req);

            var paginatedResult = await PaginatedList<MarginFormulaViewModel>.
                    GetByPagesAndBaseAsync<MarginFormula, MarginFormulaViewModel>(
                        marginFormulas,
                        _mapper,
                        searchReq);

            return paginatedResult;
        }

        public async Task<MarginFormula?> GetByEntityIdAsync(int id)
        {
            return await _marginFormulaRepository.GetByIdAsync(id);
        }

        public async Task<MarginFormulaViewModel?> GetByIdAsync(int id)
        {
            var entity = await _marginFormulaRepository.GetByIdAsync(id);
            return _mapper.MapDto<MarginFormulaViewModel>(entity);
        }

        public async Task AddAsync(MarginFormulaAddReq req)
        {
            var entity = _mapper.MapDtoCreateSetUsername<MarginFormulaAddReq, MarginFormula>(req, Username);
            await _marginFormulaRepository.AddAsync(entity);
        }

        public async Task UpdateAsync(MarginFormulaEditReq req, MarginFormula entity)
        {
            _mapper.Map(req, entity, Username);

            await _marginFormulaRepository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(MarginFormula entity) =>
            await _marginFormulaRepository.DeleteAsync(entity);
    }
}
