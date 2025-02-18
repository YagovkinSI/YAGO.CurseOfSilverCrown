using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddDatabase(configuration);
        }
    }
}
