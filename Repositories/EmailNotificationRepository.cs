using Microsoft.EntityFrameworkCore;
using MVCWebApp.Data;
using MVCWebApp.Models.EmailNotifications;
using System.Linq;

namespace MVCWebApp.Repositories
{
    public interface IEmailNotificationRepository
    {
        Task<IEnumerable<EmailNotification>> GetAllAsync();
        IQueryable<EmailNotification> GetAllQueryable();
        Task<EmailNotification?> GetByIdAsync(int id);
        Task AddAsync(EmailNotification emailNotification);
        void Update(EmailNotification emailNotification);
        void Delete(EmailNotification emailNotification);
    }

    public class EmailNotificationRepository(ApplicationDbContext context) : IEmailNotificationRepository
    {
        public async Task<IEnumerable<EmailNotification>> GetAllAsync() =>
            await context.EmailNotifications.ToListAsync();

        public IQueryable<EmailNotification> GetAllQueryable()
        {
            return context.EmailNotifications.AsQueryable();
        }

        public async Task<EmailNotification?> GetByIdAsync(int id) =>
            await context.EmailNotifications.FindAsync(id);

        public async Task AddAsync(EmailNotification emailNotification)
        {
            await context.EmailNotifications.AddAsync(emailNotification);
            await context.SaveChangesAsync();
        }

        public void Update(EmailNotification emailNotification)
        {
            context.EmailNotifications.Update(emailNotification);
            context.SaveChanges();
        }

        public void Delete(EmailNotification emailNotification)
        {
            context.EmailNotifications.Remove(emailNotification);
            context.SaveChanges();
        }
    }
}
