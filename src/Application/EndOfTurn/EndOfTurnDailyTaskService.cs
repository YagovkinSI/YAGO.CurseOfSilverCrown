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
        private const int END_OF_TURN_UTC_HOUR = 3;

        private readonly ILogger<EndOfTurnDailyTaskService> _logger;
        private readonly IEndOfTurnProcess _endOfTurnProcess;

        public EndOfTurnDailyTaskService(
            ILogger<EndOfTurnDailyTaskService> logger,
            IEndOfTurnProcess endOfTurnProcess)
        {
            _logger = logger;
            _endOfTurnProcess = endOfTurnProcess;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Старт сервиса контроля перехода хода");
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.UtcNow;
                var nextRun = DateTime.UtcNow.Date;
                if (now.Hour >= END_OF_TURN_UTC_HOUR)
                    nextRun.AddDays(1);
                nextRun.AddHours(END_OF_TURN_UTC_HOUR);

                var delay = nextRun - now;
                _logger.LogInformation("Следущий перреход хода в {NextRun} (через {Delay})", nextRun, delay);

                await Task.Delay(delay, stoppingToken);

                if (!stoppingToken.IsCancellationRequested)
                {
                    await ExecuteDailyTaskAsync();
                }
            }
        }

        private async Task ExecuteDailyTaskAsync()
        {
            try
            {
                _logger.LogInformation("Запус перехода хода в {Time}", DateTime.UtcNow);
                await _endOfTurnProcess.Execute();
                _logger.LogInformation("Переход хода завершён в {Time}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при выполнении перехода хода");
            }
        }
    }
}
