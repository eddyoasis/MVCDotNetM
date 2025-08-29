using Microsoft.EntityFrameworkCore;
using MVCWebApp.Models;
using MVCWebApp.Models.AuditLogs;
using MVCWebApp.Models.EmailGroups;
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
        public DbSet<MarginCallMTM> MarginCallMTM { get; set; }
        public DbSet<MarginCallEOD> MarginCallEOD { get; set; }
        public DbSet<LoginAttempt> LoginAttempt { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<EmailGroup> EmailGroup { get; set; }
        public DbSet<ClientEmailDBResult> ClientEmailDBResult { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ClientEmailDBResult>().HasNoKey().ToView(null);

            // Declare that this entity has no primary key
            modelBuilder.Entity<MarginCall>()
                .HasNoKey()
                .Ignore(u => u.EODTriggerFlag)
                .Ignore(u => u.MTMTriggerFlag)
                .Ignore(u => u.MarginCallFlag)
                .Ignore(u => u.ModifiedDatetime)
                .Ignore(u => u.Remarks)
                .Ignore(u => u.Type);

            modelBuilder.Entity<MarginCallMTM>()
                .HasNoKey()
                .Ignore(u => u.MarginCallFlag)
                .Ignore(u => u.InsertedDatetime)
                .Ignore(u => u.ModifiedDatetime)
                .Ignore(u => u.EmailTo)
                .Ignore(u => u.Remarks)
                .Ignore(u => u.Type);
                //.Property(e => e.MTMTriggerDatetime)
                //    .HasColumnName("MTMTriggerDatetime");

        modelBuilder.Entity<MarginCallEOD>()
                .HasNoKey()
                .Ignore(u => u.MarginCallFlag)
                .Ignore(u => u.InsertedDatetime)
                .Ignore(u => u.ModifiedDatetime)
                .Ignore(u => u.EmailTo)
                .Ignore(u => u.Remarks)
                .Ignore(u => u.Type);
        }
    }
}
