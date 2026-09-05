using Microsoft.Extensions.DependencyInjection;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Infrastructure.Datasets.GameEvents;
using YAGO.World.Infrastructure.Datasets.Persons;
using YAGO.World.Infrastructure.Datasets.Reforms;
using YAGO.World.Infrastructure.Datasets.Wiki;

namespace YAGO.World.Infrastructure.Datasets
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatasets(this IServiceCollection services)
        {
            return services
                .AddScoped<IGameEventRepository, GameEventRepository>()
                .AddScoped<IReformRepository, ReformRepository>()
                .AddScoped<IPersonRepository, PersonRepository>()
                .AddScoped<IWikiRepository, WikiRepository>();
        }
    }
}
