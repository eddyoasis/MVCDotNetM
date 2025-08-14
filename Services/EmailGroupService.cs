using MVCWebApp.Enums;
using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models;
using MVCWebApp.Models.AuditLogs;
using MVCWebApp.Models.EmailGroups;
using MVCWebApp.Models.EmailNotifications;
using MVCWebApp.Repositories;
using MVCWebApp.ViewModels;
using Newtonsoft.Json;

namespace MVCWebApp.Services
{
    public interface IEmailGroupService
    {
        Task<IEnumerable<EmailGroup>> GetAllAsync();
        Task<IEnumerable<EmailGroupViewModel>> GetAllAsync(EmailGroupSearchReq req);
        Task<EmailGroup?> GetByEntityIdAsync(int id);
        Task<EmailGroupViewModel?> GetByIdAsync(int id);
        Task AddAsync(EmailGroupAddReq req);
        Task UpdateAsync(EmailGroupEditReq req, EmailGroup entity);
        Task DeleteAsync(EmailGroup entity);
    }

    public class EmailGroupService(
        IEmailGroupRepository _emailGroupRepository,
        IAuditLogService _auditLogService,
        IMapModel _mapper
        ) : BaseService, IEmailGroupService
    {
        public async Task<IEnumerable<EmailGroup>> GetAllAsync() =>
            await _emailGroupRepository.GetAllAsync();

        public async Task<IEnumerable<EmailGroupViewModel>> GetAllAsync(EmailGroupSearchReq req)
        {
            var EmailGroups = _emailGroupRepository.GetAllQueryable();

            EmailGroups = EmailGroups.Where(x =>
                (req.TypeID == 0 || req.TypeID == x.TypeID) &&
                (req.Name.IsNullOrEmpty() || x.Name.ToLower().Contains(req.Name.ToLower())) &&
                (req.EmailTo.IsNullOrEmpty() || x.EmailTo.ToLower().Contains(req.EmailTo.ToLower())) &&
                (req.EmailCC.IsNullOrEmpty() || x.EmailCC.ToLower().Contains(req.EmailCC.ToLower()))
            )
            .OrderByDescending(x => (x.ModifiedAt ?? x.CreatedAt));

            var searchReq = _mapper.MapDto<EmailGroupSearchReq, BaseSearchReq>(req);

            var paginatedResult = await PaginatedList<EmailGroupViewModel>.
                    GetByPagesAndBaseAsync<EmailGroup, EmailGroupViewModel>(
                        EmailGroups,
                        _mapper,
                        searchReq);

            return paginatedResult;
        }

        public async Task<EmailGroup?> GetByEntityIdAsync(int id)
        {
            return await _emailGroupRepository.GetByIdAsync(id);
        }

        public async Task<EmailGroupViewModel?> GetByIdAsync(int id)
        {
            var entity = await _emailGroupRepository.GetByIdAsync(id);
            return _mapper.MapDto<EmailGroupViewModel>(entity);
        }

        public async Task AddAsync(EmailGroupAddReq req)
        {
            var entity = _mapper.MapDtoCreateSetUsername<EmailGroupAddReq, EmailGroup>(req, Username);
            await _emailGroupRepository.AddAsync(entity);

            var auditReq = new AuditLog
            {
                TypeID = (int)AuditLogTypeEnum.EmailGroup,
                ActionID = (int)AuditLogActionEnum.Create,
                Name = entity.Name,
                CreatedBy = Username,
                CreatedAt = DateTime.Now,
                NewValue = JsonConvert.SerializeObject(entity)
            };

            await _auditLogService.AddAsync(auditReq);
        }

        public async Task UpdateAsync(EmailGroupEditReq req, EmailGroup entity)
        {
            _mapper.Map(req, entity, Username);

            await _emailGroupRepository.UpdateAsync(entity);

            var auditReq = new AuditLog
            {
                TypeID = (int)AuditLogTypeEnum.EmailGroup,
                ActionID = (int)AuditLogActionEnum.Edit,
                Name = entity.Name,
                CreatedBy = Username,
                CreatedAt = DateTime.Now,
                NewValue = JsonConvert.SerializeObject(entity)
            };

            await _auditLogService.AddAsync(auditReq);
        }

        public async Task DeleteAsync(EmailGroup entity)
        {
            await _emailGroupRepository.DeleteAsync(entity);

            var auditReq = new AuditLog
            {
                TypeID = (int)AuditLogTypeEnum.EmailGroup,
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
