using Carting.Application;
using Carting.DataAccess.Repositories;
using Carting.Domain.Repositories;
using Carting.Infra.ExternalServices.MessageBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carting.Infra.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterDatabase(services, configuration);
            RegisterMessageBroker(services, configuration);

            services.AddScoped<ICartingService, CartingService>();
            services.AddHostedService<ItemMessageConsumer>();
        }

        private static void RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("InMemory") ?? string.Empty;
            services.AddSingleton<ICartRepository>(serviceProvider => new CartRepository(connectionString));
        }

        private static void RegisterMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            var serviceBusConfiguration = configuration.GetSection("AzureServiceBus");
            var connectionstring = serviceBusConfiguration["ConnectionString"] ?? string.Empty;
            services.AddSingleton<IMessageBroker>(serviceProvider => new AzureServiceBus(connectionstring));
        }
    }
}
