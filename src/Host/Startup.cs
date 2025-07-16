using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using YAGO.World.Application.ApplicationInitializing;
using YAGO.World.Application.CurrentUsers;
using YAGO.World.Application.CurrentUsers.Interfaces;
using YAGO.World.Application.Story;
using YAGO.World.Application.Story.Interfaces;
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

            services.AddControllersWithViews();

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
            UseExceptionHandler(app, env);

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            UseSpa(app);
            UseEndpoints(app);
        }

        private void UseExceptionHandler(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
        }

        private void UseSpa(IApplicationBuilder app)
        {
            app.Map("/app", spaApp =>
            {
                spaApp.UseSpa(spa =>
                {
                    spa.Options.SourcePath = "ClientApp";
                });
            });
        }

        private void UseEndpoints(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", context =>
                {
                    context.Response.Redirect("/app");
                    return Task.CompletedTask;
                });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
