using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application;
using YAGO.World.Application.Common.Database;
using YAGO.World.Host.Middlewares;
using YAGO.World.Infrastructure;

namespace YAGO.World.Host
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder);
            var app = builder.Build();
            await InitializeDatabase(app.Services);
            ConfigureHttpPipeline(app);
            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplication();

            AddAuthentication(builder);
            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();

            var isDevelopment = builder.Environment.IsDevelopment();
            builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = isDevelopment
                    ? "ClientApp/dist"
                    : "wwwroot/dist";
            });
        }

        private static void AddAuthentication(WebApplicationBuilder builder)
        {
            builder.Services.ConfigureApplicationCookie(options =>
            {
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
        }

        private static async Task InitializeDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
            await databaseInitializer.Initialize(CancellationToken.None);
        }

        private static void ConfigureHttpPipeline(WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<UserActivityMiddleware>();

            UseApiEndpoints(app);

            UseSpa(app);
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
