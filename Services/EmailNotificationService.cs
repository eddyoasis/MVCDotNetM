using MVCWebApp.Enums;
using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models;
using MVCWebApp.Models.AuditLogs;
using MVCWebApp.Models.EmailNotifications;
using MVCWebApp.Repositories;
using MVCWebApp.ViewModels;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;

namespace MVCWebApp.Services
{
    public interface IEmailNotificationService
    {
        Task<IEnumerable<EmailNotification>> GetAllAsync();
        Task<IEnumerable<EmailNotificationViewModel>> GetAllAsync(EmailNotificationSearchReq req);
        Task<EmailNotificationViewModel?> GetByIdAsync(int id);
        Task<EmailNotification?> GetByEntityIdAsync(int id);
        Task AddAsync(EmailNotificationAddReq req);
        Task Update(EmailNotificationEditReq req, EmailNotification emailNotification);
        Task Delete(EmailNotification emailNotification);
    }

    public class EmailNotificationService(
        IEmailNotificationRepository emailNotificationRepository,
        IAuditLogService _auditLogService,
        IMapModel _mapper
        ) : BaseService, IEmailNotificationService
    {
        public async Task<IEnumerable<EmailNotification>> GetAllAsync() =>
            await emailNotificationRepository.GetAllAsync();

        public async Task<IEnumerable<EmailNotificationViewModel>> GetAllAsync(EmailNotificationSearchReq req)
        {
            var emailNotifications = emailNotificationRepository.GetAllQueryable();

            emailNotifications = emailNotifications.Where(x =>
                (req.MarginType.IsNullOrEmpty() || x.MarginType.ToLower().Contains(req.MarginType.ToLower())) &&
                (req.EmailTemplate.IsNullOrEmpty() || x.EmailTemplate.ToLower().Contains(req.EmailTemplate.ToLower()))
            )
            .OrderByDescending(x => (x.ModifiedAt ?? x.CreatedAt));

            var searchReq = _mapper.MapDto<EmailNotificationSearchReq, BaseSearchReq>(req);

            var paginatedResult = await PaginatedList<EmailNotificationViewModel>.
                    GetByPagesAndBaseAsync<EmailNotification, EmailNotificationViewModel>(
                        emailNotifications,
                        _mapper,
                        searchReq);

            return paginatedResult;
        }

        public async Task<EmailNotification?> GetByEntityIdAsync(int id)
        {
            return await emailNotificationRepository.GetByIdAsync(id);
        }

        public async Task<EmailNotificationViewModel?> GetByIdAsync(int id)
        {
            var emailNotificationEntity = await emailNotificationRepository.GetByIdAsync(id);
            return _mapper.MapDto<EmailNotificationViewModel>(emailNotificationEntity);
        }

        public async Task AddAsync(EmailNotificationAddReq req)
        {
            var entity = _mapper.MapDtoCreateSetUsername<EmailNotificationAddReq, EmailNotification>(req, Username);
            await emailNotificationRepository.AddAsync(entity);

            var auditReq = new AuditLog
            {
                TypeID = (int)AuditLogTypeEnum.EmailNotifcation,
                ActionID = (int)AuditLogActionEnum.Create,
                Name = req.MarginType,
                CreatedBy = Username,
                CreatedAt = DateTime.Now,
                NewValue = JsonConvert.SerializeObject(entity)
            };

            await _auditLogService.AddAsync(auditReq);
        }

        public async Task Update(EmailNotificationEditReq req, EmailNotification entity)
        {
            _mapper.Map(req, entity, Username);

            await emailNotificationRepository.Update(entity);

            var auditReq = new AuditLog
            {
                TypeID = (int)AuditLogTypeEnum.EmailNotifcation,
                ActionID = (int)AuditLogActionEnum.Edit,
                Name = req.MarginType,
                CreatedBy = Username,
                CreatedAt = DateTime.Now,
                NewValue = JsonConvert.SerializeObject(entity)
            };

            await _auditLogService.AddAsync(auditReq);
        }

        public async Task Delete(EmailNotification emailNotification)
        {
            var auditReq = new AuditLog
            {
                TypeID = (int)AuditLogTypeEnum.EmailNotifcation,
                ActionID = (int)AuditLogActionEnum.Delete,
                Name = emailNotification.MarginType,
                CreatedBy = Username,
                CreatedAt = DateTime.Now,
                NewValue = JsonConvert.SerializeObject(emailNotification)
            };

            await _auditLogService.AddAsync(auditReq);

            await emailNotificationRepository.Delete(emailNotification);
        }
    }
}
