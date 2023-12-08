using Azure.Messaging.ServiceBus;
using Catalog.Application;
using Catalog.DataAccess;
using Catalog.DataAccess.Repositories;
using Catalog.Domain.ExternalServices;
using Catalog.Domain.Repositories;
using Catalog.Infra.ExternalServices;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Infra.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterDatabase(configuration);
            services.RegisterMessageBroker(configuration);
            services.RegisterLogging();

            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();            
        }

        private static void RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionstring = configuration.GetConnectionString("InMemory") ?? string.Empty;
            services.AddDbContext<CatalogDbContext>(options => options.UseInMemoryDatabase(connectionstring), contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Singleton);
        }

        private static void RegisterMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetSection("AzureServiceBus");

            services.AddAzureClients(builder =>
            {
                builder.AddServiceBusClient(config["ConnectionString"]);

                builder.AddClient<ServiceBusSender, ServiceBusClientOptions>((_, _, provider) =>
                provider.GetRequiredService<ServiceBusClient>().CreateSender(config["CartItemsTopic"]))
                                                        .WithName(config["CartItemsTopic"]);
            });

            services.Configure<MessageBrokerConfiguration>(config => configuration.GetSection("AzureServiceBus").Bind(config));
            services.AddScoped<IMessageBroker, ServiceBusMessageBroker>();
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