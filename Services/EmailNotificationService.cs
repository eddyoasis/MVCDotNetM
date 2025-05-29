using AutoMapper;
using MVCWebApp.Helper;
using MVCWebApp.Helper.Mapper;
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
        IMapModel mapper
        ) : BaseService, IEmailNotificationService
    {
        public async Task<IEnumerable<EmailNotificationViewModel>> GetAllAsync(EmailNotificationSearchReq req)
        {
            var emailNotifications = emailNotificationRepository.GetAllQueryable();

            int pageNumber = req.PageNumber ?? 1;
            int pageSize = req.PageSize ?? 10;

            var paginatedResult = await PaginatedList<EmailNotificationViewModel>.
                    CreateAsync<EmailNotification, EmailNotificationViewModel>(
                        emailNotifications,
                        pageNumber,
                        pageSize,
                        mapper);

            return paginatedResult;
        }

        public async Task<EmailNotification?> GetByEntityIdAsync(int id)
        {
            return await emailNotificationRepository.GetByIdAsync(id);
        }

        public async Task<EmailNotificationViewModel?> GetByIdAsync(int id)
        {
            var emailNotificationEntity = await emailNotificationRepository.GetByIdAsync(id);
            return mapper.MapDto<EmailNotificationViewModel>(emailNotificationEntity);
        }

        public async Task AddAsync(EmailNotificationAddReq req)
        {
            var entity = mapper.MapDtoCreateSetUsername<EmailNotificationAddReq, EmailNotification>(req, Username);
            await emailNotificationRepository.AddAsync(entity);
        }

        public void Update(EmailNotificationEditReq req, EmailNotification emailNotification)
        {
            mapper.Map(req, emailNotification, Username);

            emailNotificationRepository.Update(emailNotification);
        }

        public void Delete(EmailNotification emailNotification) =>
            emailNotificationRepository.Delete(emailNotification);
    }
}
