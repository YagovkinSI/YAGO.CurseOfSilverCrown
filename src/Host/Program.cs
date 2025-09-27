using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
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
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            Configure(app);

            app.Run();
        }

        private static void ConfigureServices(
            IServiceCollection services, 
            Microsoft.Extensions.Configuration.ConfigurationManager configuration)
        {
            services.AddInfrastructure(configuration);

            AddApplicationServices(services);

            services.AddControllers();

            services.AddHealthChecks();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        private static void AddApplicationServices(IServiceCollection services)
        {
            services.AddHostedService<ApplicationInitializeService>();

            services
                .AddScoped<ICurrentUserService, CurrentUserService>();
        }

        private static void Configure(WebApplication app)
        {
            MigrateDatabse(app.Services);

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

        private static void MigrateDatabse(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var migrator = scope.ServiceProvider.GetRequiredService<IDatabaseMigrator>();
            migrator.Migrate().GetAwaiter().GetResult();
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
