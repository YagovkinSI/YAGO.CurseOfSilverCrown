using Microsoft.Extensions.DependencyInjection;
using System;

namespace YAGO.World.Infrastructure.WorkSession
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddWorkSession(this IServiceCollection services)
        {
            return services
                .AddDistributedMemoryCache()
                .AddSession(options =>
                {
                    options.Cookie.Name = ".YSI.CurseOfSilverCrown.Session";
                    options.IdleTimeout = TimeSpan.FromDays(3);
                    options.Cookie.IsEssential = true;
                });
        }
    }
}
