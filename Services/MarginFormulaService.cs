using MVCWebApp.Enums;
using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models;
using MVCWebApp.Models.AuditLogs;
using MVCWebApp.Models.MarginFormulas;
using MVCWebApp.Repositories;
using MVCWebApp.ViewModels;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;

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
        IAuditLogService _auditLogService,
        IMapModel _mapper
        ) : BaseService, IMarginFormulaService
    {
        public async Task<IEnumerable<MarginFormulaViewModel>> GetAllAsync(MarginFormulaSearchReq req)
        {
            var marginFormulas = _marginFormulaRepository.GetAllQueryable();

            marginFormulas = marginFormulas.Where(x =>
                (req.Type == 0 || req.Type == x.Type) &&
                (req.Name.IsNullOrEmpty() || x.Name.ToLower().Contains(req.Name.ToLower())) &&
                (req.Formula.IsNullOrEmpty() || x.Formula.ToLower().Contains(req.Formula.ToLower()))
            )
            .OrderByDescending(x => (x.ModifiedAt ?? x.CreatedAt));

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

            var auditReq = new AuditLog
            {
                TypeID = (int)AuditLogTypeEnum.Formula,
                ActionID = (int)AuditLogActionEnum.Create,
                Name = entity.Name,
                CreatedBy = Username,
                CreatedAt = DateTime.Now,
                NewValue = JsonConvert.SerializeObject(entity)
            };

            await _auditLogService.AddAsync(auditReq);
        }

        public async Task UpdateAsync(MarginFormulaEditReq req, MarginFormula entity)
        {
            _mapper.Map(req, entity, Username);

            await _marginFormulaRepository.UpdateAsync(entity);

            var auditReq = new AuditLog
            {
                TypeID = (int)AuditLogTypeEnum.Formula,
                ActionID = (int)AuditLogActionEnum.Edit,
                Name = entity.Name,
                CreatedBy = Username,
                CreatedAt = DateTime.Now,
                NewValue = JsonConvert.SerializeObject(entity)
            };

            await _auditLogService.AddAsync(auditReq);
        }

        public async Task DeleteAsync(MarginFormula entity)
        {
            await _marginFormulaRepository.DeleteAsync(entity);

            var auditReq = new AuditLog
            {
                TypeID = (int)AuditLogTypeEnum.Formula,
                ActionID = (int)AuditLogActionEnum.Delete,
                Name = entity.Name,
                CreatedBy = Username,
                CreatedAt = DateTime.Now,
                NewValue = JsonConvert.SerializeObject(entity)
            };

            await _auditLogService.AddAsync(auditReq);
        }
    }
}
