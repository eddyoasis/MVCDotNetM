using MVCWebApp.Repositories;

namespace MVCWebApp.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IEmailNotificationRepository EmailNotifications { get; }
        Task<int> CompleteAsync();
    }

    public class UnitOfWork(ApplicationDbContext context, IEmailNotificationRepository emailNotifications) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;
        public IEmailNotificationRepository EmailNotifications { get; } = emailNotifications;

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
