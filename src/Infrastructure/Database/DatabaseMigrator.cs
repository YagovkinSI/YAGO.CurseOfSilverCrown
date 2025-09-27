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
            var migrations = _databaseContext.Database.GetPendingMigrations();
            _logger.LogInformation($"Ожидаемые миграции: {string.Join(", ", migrations)}.");

            var appliedMigrations = _databaseContext.Database.GetAppliedMigrations();
            _logger.LogInformation($"Примененные миграции: {string.Join(", ", appliedMigrations)}.");

            await _databaseContext.Database.MigrateAsync();
        }
    }
}
