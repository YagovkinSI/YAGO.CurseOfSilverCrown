using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YAGO.World.Host.Infrastructure.Database;
using YAGO.World.Host.Infrastructure.Identity;
using YAGO.World.Host.Infrastructure.WorkSession;

namespace YAGO.World.Host.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDatabase(configuration)
                .AddIdentityInfrastructure()
                .AddWorkSession();
        }
    }
}
