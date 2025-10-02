using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using YAGO.World.Application.ApplicationInitializing;
using YAGO.World.Application.CurrentUsers;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.InfrastructureInterfaces.Database;
using YAGO.World.Host.Middlewares;
using YAGO.World.Infrastructure;

namespace YAGO.World.Host
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var isDevelopment = builder.Environment.IsDevelopment();
            ConfigureServices(builder.Services, builder.Configuration, isDevelopment);

            var app = builder.Build();

            await MigrateDatabase(app.Services);
            Configure(app);

            app.Run();
        }

        private static void ConfigureServices(
            IServiceCollection services,
            Microsoft.Extensions.Configuration.ConfigurationManager configuration,
            bool isDevelopment)
        {
            services.AddInfrastructure(configuration);

            AddApplicationServices(services);

            services.AddControllers();

            services.AddHealthChecks();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = isDevelopment
                    ? "ClientApp/dist"
                    : "wwwroot/dist";
            });
        }

        private static void AddApplicationServices(IServiceCollection services)
        {
            services.AddHostedService<ApplicationInitializeService>();

            services
                .AddScoped<IMyUserService, CurrentUserService>();
        }

        private static void Configure(WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            UseApiEndpoints(app);

            UseSpa(app);
        }

        private static async Task MigrateDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var migrator = scope.ServiceProvider.GetRequiredService<IDatabaseMigrator>();
            await migrator.Migrate();
        }

        private static void UseApiEndpoints(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }

        private static void UseSpa(IApplicationBuilder app)
        {
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
            });
        }
    }
}
