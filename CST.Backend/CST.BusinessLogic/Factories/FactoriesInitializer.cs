using CST.BusinessLogic.Factories.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace CST.BusinessLogic.Factories;

public static class FactoriesInitializer
{
	public static IServiceCollection ConfigureFactories(this IServiceCollection serviceCollection)
	{
		serviceCollection.AddScoped<IEmailFactory, EmailFactory>();

		return serviceCollection;
	}
}