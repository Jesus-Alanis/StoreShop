using Carting.Application;
using Carting.DataAccess.Repositories;
using Carting.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Carting.Infra.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICartingService, CartingService>();

            var connectionString = configuration.GetConnectionString("InMemory") ?? string.Empty;
            services.AddSingleton<ICartRepository>(serviceProvider => new CartRepository(connectionString));
        }
    }
}
