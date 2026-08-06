using Certes;
using FluffySpoon.AspNet.EncryptWeMust;
using FluffySpoon.AspNet.EncryptWeMust.Certes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Database;
using YAGO.World.Domain.Services;
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
            builder.Services.AddDataProtection()
                .SetApplicationName("YagoWorld");

            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("YAGO.World.Application")));
            AddApplicationServices(builder.Services);

            if (!isDevelopment)
            {
                AddLetsEncrypt(builder);
            }

            builder.Services.AddControllers();

            builder.Services.AddHealthChecks();

            builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = isDevelopment
                    ? "ClientApp/dist"
                    : "wwwroot/dist";
            });

            AddCors(builder);
        }

        // Настройка Let's Encrypt для автоматического получения и обновления SSL-сертификата
        private static void AddLetsEncrypt(WebApplicationBuilder builder)
        {
            // 1. Настраиваем Let's Encrypt
            var letsEncryptConfig = builder.Configuration.GetSection("LetsEncrypt");
            var csrConfig = letsEncryptConfig.GetSection("CertificateSigningRequest");
            var options = new LetsEncryptOptions
            {
                Email = letsEncryptConfig["Email"],
                Domains = new[] { letsEncryptConfig["Domain"] },
                UseStaging = bool.Parse(letsEncryptConfig["UseStaging"] ?? "false"),
                TimeUntilExpiryBeforeRenewal = TimeSpan.FromDays(30),
                CertificateSigningRequest = new CsrInfo
                {
                    CountryName = csrConfig["CountryName"] ?? "RU",
                    State = csrConfig["State"] ?? "Moscow",
                    Locality = csrConfig["Locality"] ?? "Moscow",
                    Organization = csrConfig["Organization"] ?? "YAGO",
                    OrganizationUnit = csrConfig["OrganizationUnit"] ?? "IT"
                }
            };
            builder.Services.AddFluffySpoonLetsEncrypt(options);
            builder.Services.AddFluffySpoonLetsEncryptFileCertificatePersistence();
            builder.Services.AddFluffySpoonLetsEncryptMemoryChallengePersistence();

            // 2. Добавляем персистентность сертификата в файл
            builder.Services.AddFluffySpoonLetsEncryptFileCertificatePersistence();
        }

        private static void AddApplicationServices(IServiceCollection services)
        {
            services
                .AddScoped<IGameEventGenerator, GameEventGenerator>();
        }

        private static void AddCors(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:5001", // для локальной разработки
                        "http://localhost", // для проверки через Docker
                        "https://yagoworld.ru" // для продакшена
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // если используешь куки или авторизацию
                });
            });
        }

        private static void Configure(WebApplication app)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            // Включение Let's Encrypt middleware для обработки ACME-вызовов и HTTPS
            if (!app.Environment.IsDevelopment())
            {
                // Один middleware для всего
                app.UseFluffySpoonLetsEncrypt();
                app.UseHttpsRedirection();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            app.UseCors("AllowFrontend");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<UserActivityMiddleware>();

            UseApiEndpoints(app);

            UseSpa(app);
        }

        private static async Task InitializeDatabase(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var databaseInitializer = scope.ServiceProvider.GetRequiredService<IDatabaseMigrator>();
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
