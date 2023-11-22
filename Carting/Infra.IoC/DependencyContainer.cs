using Azure.Messaging.ServiceBus;
using Carting.Application;
using Carting.DataAccess.Repositories;
using Carting.Domain.ExternalServices;
using Carting.Domain.Repositories;
using Carting.Infra.ExternalServices;
using Carting.Infra.ExternalServices.MessageBroker;
using Microsoft.Extensions.Azure;
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
            var config = configuration.GetSection("AzureServiceBus");

            services.AddAzureClients(builder =>
            {
                builder.AddServiceBusClient(config["ConnectionString"]);

                builder.AddClient<ServiceBusProcessor, ServiceBusProcessorOptions>((_, _, provider) =>
                provider.GetRequiredService<ServiceBusClient>()
                    .CreateProcessor(config["CartItemsTopic"], config["CartItemsSubscription"], new ServiceBusProcessorOptions { MaxConcurrentCalls = 1, AutoCompleteMessages = false }))
                    .WithName(config["CartItemsSubscription"]);
            });

            services.Configure<MessageBrokerConfiguration>(config => configuration.GetSection("AzureServiceBus").Bind(config));
            services.AddSingleton<IMessageBroker, ServiceBusMessageBroker>();
        }
    }
}
