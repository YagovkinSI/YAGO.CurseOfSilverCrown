using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using YAGO.World.Application.ApplicationInitializing;
using YAGO.World.Application.CurrentUsers;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.Story;
using YAGO.World.Application.Story.Interfaces;
using YAGO.World.Host.Middlewares;
using YAGO.World.Infrastructure;

namespace YAGO.World.Host
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInfrastructure(Configuration);

            AddApplicationServices(services);

            services.AddControllers();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        private static void AddApplicationServices(IServiceCollection services)
        {
            services.AddHostedService<ApplicationInitializeService>();

            services
                .AddScoped<ICurrentUserService, CurrentUserService>()
                .AddScoped<IStoryService, StoryService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ExceptionMiddleware>();

            UseApiEndpoints(app);

            UseSpa(app);
        }

        private void UseApiEndpoints(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void UseSpa(IApplicationBuilder app)
        {
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
            });
        }
    }
}
