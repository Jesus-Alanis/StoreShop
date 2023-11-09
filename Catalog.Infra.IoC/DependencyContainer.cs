using Catalog.Application;
using Catalog.DataAccess;
using Catalog.DataAccess.Repositories;
using Catalog.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace StoreShop.Infra
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<ICatalogService, CatalogService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();            
        }

        public static void RegisterDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogDbContext>(options => options.UseInMemoryDatabase(configuration.GetConnectionString("InMemory")), contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Singleton);
        }
    }
}