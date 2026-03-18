using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Application.Colonies.CreateColony;
using YAGO.World.Application.Colonies.DeactivateColony;
using YAGO.World.Application.Colonies.GetPaginatedColonies;
using YAGO.World.Application.Colonies.IssueDecree;
using YAGO.World.Application.Colonies.RunCycle;
using YAGO.World.Application.Common.Database;
using YAGO.World.Application.Cycles;
using YAGO.World.Application.Decrees;
using YAGO.World.Application.Users;
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
            ConfigureServices(builder, isDevelopment);

            var app = builder.Build();

            await InitializeDatabase(app.Services);
            Configure(app);

            app.Run();
        }

        private static void ConfigureServices(
            WebApplicationBuilder builder,
            bool isDevelopment)
        {
            builder.Services.AddInfrastructure(builder.Configuration);

            AddApplicationServices(builder.Services);

            AddAuthentication(builder);

            builder.Services.AddControllers();

            builder.Services.AddHealthChecks();

            builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = isDevelopment
                    ? "ClientApp/dist"
                    : "wwwroot/dist";
            });
        }

        private static void AddApplicationServices(IServiceCollection services)
        {
            services
                .AddScoped<IGetMyUserProcessor, GetMyUserProcessor>()
                .AddScoped<ILoginUserProcessor, LoginUserProcessor>()
                .AddScoped<IRegisterUserProcessor, RegisterUserProcessor>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<IColonyService, ColonyService>()
                .AddScoped<ICycleProvider, CycleProvider>()
                .AddScoped<IDecreeService, DecreeService>()
                .AddScoped<IPaginatedColoniesProvider, PaginatedColoniesProvider>()
                .AddColonyCommands();
        }

        private static IServiceCollection AddColonyCommands(this IServiceCollection services)
        {
            services
                .AddScoped<IRunCycleProcessor, RunCycleProcessor>()
                .AddScoped<IIssueDecreeProcessor, IssueDecreeProcessor>()
                .AddScoped<ICreateColonyProcessor, CreateColonyProcessor>()
                .AddScoped<IDeactivateColonyProcessor, DeactivateColonyProcessor>();

            return services;
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

        private static void Configure(WebApplication app)
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

        private static async Task InitializeDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseInitializer>();
            await databaseInitializer.Initialize(CancellationToken.None);
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
