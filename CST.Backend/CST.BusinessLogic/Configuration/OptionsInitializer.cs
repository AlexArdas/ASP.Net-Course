using CST.Common.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CST.BusinessLogic.Configuration;

public static class OptionsInitializer
{
	public static IServiceCollection ConfigureApiOptions(
		this IServiceCollection serviceCollection,
		IConfiguration configuration)
	{
		serviceCollection.Configure<LinksOptions>(configuration.GetSection(nameof(LinksOptions)));

		serviceCollection.Configure<EmailTemplatesOptions>(configuration.GetSection(nameof(EmailTemplatesOptions)));
		serviceCollection.AddTransient<IConfigureOptions<EmailTemplatesOptions>, EmailTemplatesOptionsConfigurator>();

		return serviceCollection;
	}
}