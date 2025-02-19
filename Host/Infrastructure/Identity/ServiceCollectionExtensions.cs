using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using YAGO.World.Host.Database.Users;
using YAGO.World.Host.Infrastructure.Database;

namespace YAGO.World.Host.Infrastructure.Identity
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
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            return services;
        }
    }
}
