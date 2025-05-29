using Microsoft.EntityFrameworkCore;
using MVCWebApp.Models.EmailNotifications;

namespace MVCWebApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<EmailNotification> EmailNotifications { get; set; }
    }
}
