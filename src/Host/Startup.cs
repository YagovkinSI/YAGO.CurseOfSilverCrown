using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
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
            services.ConfigureApplicationCookie(options => {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.HttpOnly = true;
            }); 
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("/keys"))
                .SetApplicationName("YourAppName");

            services.AddCors(options => {
                options.AddPolicy("ClientApp", builder => {
                    builder.
                        SetIsOriginAllowed(origin =>
                            origin == "http://89.111.153.37")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

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

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();

            app.UseCors("ClientApp");
            app.UseAuthentication();
            app.UseAuthorization();

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
