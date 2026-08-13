using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Identity;
using YAGO.World.Infrastructure.Database;
using YAGO.World.Infrastructure.Database.Users;

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
                options.Password.RequiredLength = IdentityManager.PasswordRequiredLength;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireDigit = true;
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
                    // Настройки из Infrastructure
                    options.Cookie.Name = CookieName;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.ExpireTimeSpan = TimeSpan.FromDays(CookieExpirationDays);
                    options.SlidingExpiration = true;

                    // Настройки из Program.cs (перенесены сюда)
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    };
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = 403;
                        return Task.CompletedTask;
                    };
                });

            return services;
        }
    }
}
