using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Database;

namespace YAGO.World.Infrastructure.Database
{
    internal class DatabaseMigrator : IDatabaseInitializer
    {
        private readonly ApplicationDbContext _databaseContext;
        private readonly ILogger<DatabaseMigrator> _logger;

        public DatabaseMigrator(
            ApplicationDbContext databaseContext,
            ILogger<DatabaseMigrator> logger)
        {
            _databaseContext = databaseContext;
            _logger = logger;
        }

        public async Task Initialize(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Инициализация базы данных...");

            try
            {
                await _databaseContext.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Этап миграции пройден успешно.");

                await InitilaizeData(cancellationToken);
                _logger.LogInformation("Этап обновления данных БД пройден успешно.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Ошибка при инициализации базы данных.");
                throw;
            }
        }

        public async Task InitilaizeData(CancellationToken cancellationToken)
        {
            var someChanges = false;

            if (_databaseContext.Cycles.Any(x => x.State == Domain.Cycles.CycleState.Unknown))
            {
                foreach (var cycle in _databaseContext.Cycles)
                {
                    if (cycle.State == Domain.Cycles.CycleState.Unknown)
                    {
                        cycle.Migrate();
                        someChanges = true;
                    }
                }
            }

            if (someChanges)
                await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
