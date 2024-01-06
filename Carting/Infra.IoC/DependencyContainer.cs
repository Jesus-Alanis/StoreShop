using Azure.Messaging.ServiceBus;
using Carting.Application;
using Carting.DataAccess.Repositories;
using Carting.Domain.ExternalServices;
using Carting.Domain.Repositories;
using Carting.Infra.ExternalServices;
using Carting.Infra.ExternalServices.MessageBroker;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Carting.Infra.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDatabase(configuration);
            services.RegisterMessageBroker(configuration);
            services.RegisterLogging();

            services.AddScoped<ICartingService, CartingService>();
        }

        private static void RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("InMemory") ?? string.Empty;
            services.AddSingleton<ICartRepository>(serviceProvider => {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                return new CartRepository(loggerFactory.CreateLogger<CartRepository>(), connectionString);
            });
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
            services.AddHostedService<ItemMessageConsumer>();
        }

        private static void RegisterLogging(this IServiceCollection services)
        {
            services.Configure<TelemetryConfiguration>(config => {
                config.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            });

            services.AddApplicationInsightsTelemetry(options => {
                options.EnableAdaptiveSampling = false;
                options.EnableQuickPulseMetricStream = false;
            });
        }
    }
}
