using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models;
using MVCWebApp.Models.AuditLogs;
using MVCWebApp.Models.EmailNotifications;
using MVCWebApp.Models.MarginFormulas;
using MVCWebApp.Repositories;
using MVCWebApp.ViewModels;

namespace MVCWebApp.Services
{
    public interface IAuditLogService
    {
        Task<IEnumerable<AuditLogViewModel>> GetAllAsync(AuditLogSearchReq req);
        Task<AuditLogViewModel> GetByIdAsync(int id);
        Task AddAsync(AuditLog req);
    }

    public class AuditLogService(
        IAuditLogRepository _auditLogRepository,
        IMapModel _mapper) : BaseService, IAuditLogService
    {

        public async Task<IEnumerable<AuditLogViewModel>> GetAllAsync(AuditLogSearchReq req)
        {
            var auditLogs = _auditLogRepository.GetAllQueryable();
            auditLogs = auditLogs.Where(x =>
                (req.TypeID == 0 || req.TypeID == x.TypeID) &&
                (req.ActionID == 0 || req.ActionID == x.ActionID) &&
                (req.Name.IsNullOrEmpty() || x.Name.ToLower().Contains(req.Name.ToLower()))
            )
            .OrderByDescending(x => x.CreatedAt);

            var searchReq = _mapper.MapDto<AuditLogSearchReq, BaseSearchReq>(req);

            var paginatedResult = await PaginatedList<AuditLogViewModel>.
                    GetByPagesAndBaseAuditAsync<AuditLog, AuditLogViewModel>(
                        auditLogs,
                        _mapper,
                        searchReq);

            return paginatedResult;
        }

        public async Task<AuditLogViewModel> GetByIdAsync(int id)
        {
            var auditLog = await _auditLogRepository.GetByIdAsync(id);
            return _mapper.MapDto<AuditLogViewModel>(auditLog);
        }

        public async Task AddAsync(AuditLog entity)
        {
            await _auditLogRepository.AddAsync(entity);
        }
    }
}
