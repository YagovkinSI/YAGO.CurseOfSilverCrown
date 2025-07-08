using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;
using YAGO.World.Infrastructure.Database.Repositories;

namespace YAGO.World.Infrastructure.Database
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("YAGO.World.Infrastructure")
                    ))
                .AddDatabaseDeveloperPageExceptionFilter()
                .AddScoped<IRepositoryForUpdateData, RepositoryForUpdateData>()
                .AddScoped<IRepositoryProvince, RepositoryProvince>()
                .AddScoped<IRepositoryOrganizations, RepositoryOrganizations>()
                .AddScoped<IRepositoryCommads, RepositoryCommads>()
                .AddScoped<IRepositoryUnits, RepositoryUnits>()
                .AddScoped<IRepositoryTurns, RepositoryTurns>();
        }
    }
}
