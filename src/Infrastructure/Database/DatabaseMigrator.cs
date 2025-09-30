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
            _logger.LogInformation("Начало применения миграций БД...");
            
            try
            {
                await _databaseContext.Database.MigrateAsync();
                _logger.LogInformation("Миграция завершена успешно.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при применении миграций БД.");
                throw;
            }
        }
    }
}
