using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
using MVCWebApp.Models;
using MVCWebApp.Models.EmailNotifications;
using MVCWebApp.Repositories;
using MVCWebApp.ViewModels;

namespace MVCWebApp.Services
{
    public interface IEmailNotificationService
    {
        Task<IEnumerable<EmailNotificationViewModel>> GetAllAsync(EmailNotificationSearchReq req);
        Task<EmailNotificationViewModel?> GetByIdAsync(int id);
        Task<EmailNotification?> GetByEntityIdAsync(int id);
        Task AddAsync(EmailNotificationAddReq req);
        void Update(EmailNotificationEditReq req, EmailNotification emailNotification);
        void Delete(EmailNotification emailNotification);
    }

    public class EmailNotificationService(
        IEmailNotificationRepository emailNotificationRepository,
        IMapModel _mapper
        ) : BaseService, IEmailNotificationService
    {
        public async Task<IEnumerable<EmailNotificationViewModel>> GetAllAsync(EmailNotificationSearchReq req)
        {
            var emailNotifications = emailNotificationRepository.GetAllQueryable();

            emailNotifications = emailNotifications.Where(x =>
                (req.MarginType.IsNullOrEmpty() || x.MarginType.ToLower().Contains(req.MarginType.ToLower())) &&
                (req.EmailTemplate.IsNullOrEmpty() || x.EmailTemplate.ToLower().Contains(req.EmailTemplate.ToLower()))
            );

            var searchReq = _mapper.MapDto<EmailNotificationSearchReq, BaseSearchReq>(req);

            var paginatedResult = await PaginatedList<EmailNotificationViewModel>.
                    CreateAsync<EmailNotification, EmailNotificationViewModel>(
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
        }

        public void Update(EmailNotificationEditReq req, EmailNotification entity)
        {
            _mapper.Map(req, entity, Username);

            emailNotificationRepository.Update(entity);
        }

        public void Delete(EmailNotification emailNotification) =>
            emailNotificationRepository.Delete(emailNotification);
    }
}
