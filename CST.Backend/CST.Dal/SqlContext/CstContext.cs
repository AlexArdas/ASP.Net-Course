using CST.Common.Models.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CST.DAL.Tests")]
namespace CST.Dal
{
    public class CstContext : DbContext
    {
        public CstContext()
        {
        }

        public CstContext(DbContextOptions<CstContext> options)
            : base(options)
        {
        }

        public DbSet<ReportDomainEntity> ReportDomainEntities { get; set; }
        public DbSet<KeyNumberSetDomainEntity> KeyNumberSetDomainEntities { get; set; }
        public DbSet<ReportColumnSetDomainEntity> ReportColumnSetDomainEntities { get; set; }
        public virtual DbSet<MailingDomainEntity> MailingDomainEntities { get; set; }
        public DbSet<NotificationChannelDomainEntity> NotificationChannelDomainEntities { get; set; }
        public virtual DbSet<LocationDomainEntity> LocationDomainEntities { get; set; }
        public virtual DbSet<UserDomainEntity> UserDomainEntities { get; set; }
        public DbSet<RequestFormDomainEntity> RequestFormDomainEntities { get; set; }
        public DbSet<RequestDomainEntity> RequestDomainEntities { get; set; }
        public DbSet<RequestMessageDomainEntity> RequestMessageDomainEntities { get; set; }
        public DbSet<ShownRequestToUserDomainEntity> ShownRequestToUserDomainEntities { get; set; }
        public DbSet<MailingsApproversDomainEntity> MailingsApproversDomainEntities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("IHubDatabase");
                optionsBuilder.UseNpgsql(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CstContext).Assembly);
            SetDateTimeKindToUTC(modelBuilder);
        }

        private void SetDateTimeKindToUTC(ModelBuilder modelBuilder)
        {
            var dateTimeConverter = new ValueConverter<DateTime, DateTime>(
                v => v,
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

            var nullableDateTimeConverter = new ValueConverter<DateTime?, DateTime?>(
                v => v,
                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (entityType.IsKeyless)
                {
                    continue;
                }

                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(DateTime))
                    {
                        property.SetValueConverter(dateTimeConverter);
                    }
                    else if (property.ClrType == typeof(DateTime?))
                    {
                        property.SetValueConverter(nullableDateTimeConverter);
                    }
                }
            }
        }
    }
}
