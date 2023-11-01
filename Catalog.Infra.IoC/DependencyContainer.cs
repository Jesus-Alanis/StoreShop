using Catalog.Application;
using Catalog.DataAccess;
using Catalog.DataAccess.Repositories;
using Catalog.Domain.ExternalServices;
using Catalog.Domain.Repositories;
using Catalog.Infra.ExternalServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StoreShop.Infra
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            RegisterDatabase(services, configuration);
            RegisterMessageBroker(services, configuration);

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
            var serviceBusConfiguration = configuration.GetSection("AzureServiceBus");
            var connectionstring = serviceBusConfiguration["ConnectionString"] ?? string.Empty;
            var topicName = serviceBusConfiguration["TopicName"] ?? string.Empty;
            services.AddSingleton<IMessageBroker>(serviceProvider => new AzureServiceBus(connectionstring, topicName));
        }
    }
}