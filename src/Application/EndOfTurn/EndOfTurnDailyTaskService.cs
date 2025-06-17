using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.EndOfTurn.Interfaces;

namespace YAGO.World.Application.EndOfTurn
{
    public class EndOfTurnDailyTaskService : BackgroundService
    {
        private const int END_OF_TURN_UTC_HOUR = 2;

        private readonly ILogger<EndOfTurnDailyTaskService> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public EndOfTurnDailyTaskService(
            ILogger<EndOfTurnDailyTaskService> logger,
            IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Старт сервиса контроля перехода хода");
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var nextRun = DateTime.UtcNow.Date;
                if (now.Hour >= END_OF_TURN_UTC_HOUR)
                    nextRun = nextRun.AddDays(1);
                nextRun = nextRun.AddHours(END_OF_TURN_UTC_HOUR);

                var delay = nextRun - now;
                _logger.LogInformation("Следущий перреход хода в {NextRun} (через {Delay})", nextRun, delay);

                await Task.Delay(delay, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    await ExecuteDailyTaskAsync();
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task ExecuteDailyTaskAsync()
        {
            try
            {
                _logger.LogInformation("Запус перехода хода в {Time}", DateTime.UtcNow);
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var endOfTurnProcess = scope.ServiceProvider
                        .GetRequiredService<IEndOfTurnProcess>();

                    await endOfTurnProcess.Execute();
                    _logger.LogInformation("Переход хода завершён в {Time}", DateTime.UtcNow);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при выполнении перехода хода");
            }
        }
    }
}
