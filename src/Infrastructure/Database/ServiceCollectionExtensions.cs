using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using YAGO.World.Application.InfrastructureInterfaces;
using YAGO.World.Application.Users.Interfaces;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Infrastructure.Database.Repositories;

namespace YAGO.World.Infrastructure.Database
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext(configuration)
                .AddDatabaseDeveloperPageExceptionFilter()
                .AddScoped<IDatabaseMigrator, DatabaseMigrator>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IUpdateDatabaseRepository, UpdateDatabaseRepository>();
        }

        private static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
                throw new YagoException("Строка подключения DefaultConnection не найдена.");

            services
                .AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(connectionString,
                        b => b.MigrationsAssembly("YAGO.World.Infrastructure")
                    ));

            return services;
        }
    }
}
