using Microsoft.Extensions.DependencyInjection;
using YAGO.World.Application.Colonies;
using YAGO.World.Application.Colonies.CreateColony;
using YAGO.World.Application.Colonies.DeactivateColony;
using YAGO.World.Application.Colonies.IssueDecree;
using YAGO.World.Application.Colonies.RunCycle;
using YAGO.World.Application.Cycles;
using YAGO.World.Application.Decrees;
using YAGO.World.Application.GetColonyWithDetails;
using YAGO.World.Application.GetPaginatedColonies;
using YAGO.World.Application.Users;

namespace YAGO.World.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services
                .AddScoped<IUserService, UserService>()
                .AddScoped<IColonyService, ColonyService>()
                .AddScoped<ICycleProvider, CycleProvider>()
                .AddScoped<IDecreeService, DecreeService>()
                .AddScoped<IColonyWithDetailsProvider, ColonyWithDetailsProvider>()
                .AddScoped<IPaginatedColoniesProvider, PaginatedColoniesProvider>()
                .AddColonyCommands();

            return services;
        }

        private static IServiceCollection AddColonyCommands(this IServiceCollection services)
        {
            services
                .AddScoped<IRunCycleProcessor, RunCycleProcessor>()
                .AddScoped<IIssueDecreeProcessor, IssueDecreeProcessor>()
                .AddScoped<ICreateColonyProcessor, CreateColonyProcessor>()
                .AddScoped<IDeactivateColonyProcessor, DeactivateColonyProcessor>();

            return services;
        }
    }
}
