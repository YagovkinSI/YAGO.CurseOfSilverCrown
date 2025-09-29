using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using YAGO.World.Application.InfrastructureInterfaces;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Models.Users;

namespace YAGO.World.Infrastructure.Identity
{
    public static class ServiceCollectionExtensions
    {
        public const string CookieName = "YAGO.Auth";
        public const int CookieExpirationDays = 14;

        public static IServiceCollection AddIdentityInfrastructure(this IServiceCollection services)
        {
            return services
                .AddIdentity()
                .ConfigureApplicationCookie()
                .AddScoped<IIdentityManager, IdentityManager>();
        }

        private static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
                {
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.AllowedUserNameCharacters
                        = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-[]().";
                });

            services
                .AddDefaultIdentity<UserEntity>()
                .AddRoles<IdentityRole<long>>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }

        private static IServiceCollection ConfigureApplicationCookie(this IServiceCollection services)
        {
            services
                .ConfigureApplicationCookie(options =>
                {
                    options.Cookie.Name = CookieName;
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(CookieExpirationDays);
                    options.SlidingExpiration = true;
                });

            return services;
        }
    }
}
