using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using YAGO.World.Application.InfrastructureInterfaces.Database;

namespace YAGO.World.Infrastructure.Database
{
    internal class DatabaseMigrator : IDatabaseMigrator
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

        public async Task Migrate()
        {
            LogMigrateInfo();
            await _databaseContext.Database.MigrateAsync();
            _logger.LogInformation("Миграция завершена.");
        }

        private void LogMigrateInfo()
        {
            try
            {
                var pendingMigrations = _databaseContext.Database.GetPendingMigrations();
                _logger.LogInformation("Ожидаемые миграции: {pendingMigrations}.", string.Join(", ", pendingMigrations));

                var appliedMigrations = _databaseContext.Database.GetAppliedMigrations();
                _logger.LogInformation("Примененные миграции: {appliedMigrations}.", string.Join(", ", appliedMigrations));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось выполнить логирование информации по миграции.");
            }
        }
    }
}
