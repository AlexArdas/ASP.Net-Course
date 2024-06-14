using System.IO.Abstractions;
using CST.BusinessLogic.Factories;
using CST.BusinessLogic.Services;
using CST.Common.Providers;
using CST.Common.Repositories;
using CST.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CST.BusinessLogic.Configuration
{
    public static class ModuleInitializer
    {
        public static IServiceCollection ConfigureBll(this IServiceCollection services)
        {
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IMailingService, MailingService>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IReportGeneratorService, ReportGeneratorService>();
            services.AddTransient<INotificationChannelService, NotificationChannelService>();
            services.AddTransient<IRequestService, RequestService>();
            services.AddTransient<ISampleDataService, SampleDataService>();
            services.AddTransient<IUserService, UserService>();
            services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
            services.AddSingleton<IBlobService, BlobService>();
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IEmailReceiver, EmailReceiver>();
            services.AddScoped<IFileSystem, FileSystem>();

            services.ConfigureFactories();

            return services;
        }
    }
}
