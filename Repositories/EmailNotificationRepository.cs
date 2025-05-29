using Microsoft.EntityFrameworkCore;
using MVCWebApp.Data;
using MVCWebApp.Models.EmailNotifications;

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

    public class EmailNotificationRepository(ApplicationDbContext _context) : IEmailNotificationRepository
    {
        public async Task<IEnumerable<EmailNotification>> GetAllAsync() =>
            await _context.EmailNotifications.ToListAsync();

        public IQueryable<EmailNotification> GetAllQueryable()
        {
            return _context.EmailNotifications.AsQueryable();
        }

        public async Task<EmailNotification?> GetByIdAsync(int id) =>
            await _context.EmailNotifications.FindAsync(id);

        public async Task AddAsync(EmailNotification entity)
        {
            await _context.EmailNotifications.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public void Update(EmailNotification entity)
        {
            _context.EmailNotifications.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(EmailNotification entity)
        {
            _context.EmailNotifications.Remove(entity);
            _context.SaveChanges();
        }
    }
}
