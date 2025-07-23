using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using YAGO.World.Application.InfrastructureInterfaces;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Users;

namespace YAGO.World.Infrastructure.Identity
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
        {
            services
                .Configure<IdentityOptions>(options =>
                    options.Password.RequireNonAlphanumeric = false
                );

            services
                .AddDefaultIdentity<User>()
                .AddRoles<IdentityRole<long>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services
                .AddScoped<IIdentityManager, IdentityManager>();
        }
    }
}
