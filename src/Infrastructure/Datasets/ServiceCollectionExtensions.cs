using Microsoft.Extensions.DependencyInjection;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Infrastructure.Datasets.GameEvents;
using YAGO.World.Infrastructure.Datasets.Reforms;

namespace YAGO.World.Infrastructure.Datasets
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatasets(this IServiceCollection services)
        {
            return services
                .AddScoped<IGameEventRepository, GameEventRepository>()
                .AddScoped<IReformRepository, ReformRepository>();
        }
    }
}
