using CST.Common.Models.Domain;
using CST.Dal.Repositories;
using CST.Dal.SqlContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using CST.Common.Repositories;

[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
namespace CST.Dal.Configuration
{
    [ExcludeFromCodeCoverage]
    public static class ModuleInitializer
    {
        public static IServiceCollection ConfigureDal(this IServiceCollection services, IConfiguration configuration)
        {
            ConfigureDbContext(services, configuration);
            AddDependenciesToContainer(services);

            return services;
        }

        private static void ConfigureDbContext(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("CstDatabase");

            services.AddDbContext<CstContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            var dbFactory = new CstContextFactory(connectionString);
            services.AddSingleton(typeof(ICstContextFactory), dbFactory);
        }

        private static void AddDependenciesToContainer(IServiceCollection services)
        {
            services.AddTransient<IReportRepository, ReportRepository>();
            services.AddTransient<IRepository<ReportDomainEntity>, BaseRepository<ReportDomainEntity>>();
            services.AddTransient<IMailingRepository, MailingRepository>();
            services.AddTransient<IRepository<MailingDomainEntity>, BaseRepository<MailingDomainEntity>>();
            services.AddTransient<INotificationChannelRepository, NotificationChannelRepository>();
            services.AddTransient<IRepository<NotificationChannelDomainEntity>, BaseRepository<NotificationChannelDomainEntity>>();
            services.AddTransient<IRepository<LocationDomainEntity>, BaseRepository<LocationDomainEntity>>();
            services.AddTransient<IRepository<UserDomainEntity>, BaseRepository<UserDomainEntity>>();
            services.AddTransient<IRepository<RoleDomainEntity>, BaseRepository<RoleDomainEntity>>();
            services.AddTransient<IRepository<UserRoleDomainEntity>, BaseRepository<UserRoleDomainEntity>>();
            services.AddTransient<IRepository<RequestDomainEntity>, BaseRepository<RequestDomainEntity>>();
            services.AddTransient<IRepository<RequestFormDomainEntity>, BaseRepository<RequestFormDomainEntity>>();
            services.AddTransient<IHasIdRepository<UserDomainEntity>, ItemHasIdRepository<UserDomainEntity>>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IShownRequestToUserRepository, ShownRequestToUserRepository>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<IMailingsApproversRepository, MailingsApproversRepository>();
        }
    }
}
