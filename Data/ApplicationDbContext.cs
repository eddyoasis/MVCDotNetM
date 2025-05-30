using Microsoft.EntityFrameworkCore;
using MVCWebApp.Models.EmailNotifications;
using MVCWebApp.Models.MarginCalls;
using MVCWebApp.Models.MarginFormulas;

namespace MVCWebApp.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<EmailNotification> EmailNotifications { get; set; }
        public DbSet<MarginFormula> MarginFormulas { get; set; }
        public DbSet<MarginCall> MarginCall { get; set; }
    }
}
