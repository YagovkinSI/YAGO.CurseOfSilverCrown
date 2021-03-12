using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Repositories;

namespace YAGO.World.Application.ApplicationInitializing
{
    public class ApplicationInitializeService : IHostedService
    {
        private readonly ILogger<ApplicationInitializeService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ApplicationInitializeService(
            ILogger<ApplicationInitializeService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _logger.LogWarning("Старт инициализации приложения.");
                var repositoryForUpdateData = scope.ServiceProvider
                    .GetRequiredService<IUpdateDatabaseRepository>();

                await repositoryForUpdateData.Update(cancellationToken);
                _logger.LogWarning("Инициализация приложения завершена.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
