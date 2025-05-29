using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Identity;
using YAGO.World.Infrastructure.Promt;
using YAGO.World.Infrastructure.WorkSession;

namespace YAGO.World.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDatabase(configuration)
                .AddIdentityInfrastructure()
                .AddWorkSession()
                .AddScoped<PromtCreator>();
        }
    }
}
