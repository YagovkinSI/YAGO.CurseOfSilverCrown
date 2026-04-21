using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Database;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Infrastructure.Database.Colonies;
using YAGO.World.Infrastructure.Database.Cycles;

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

        private async Task InitilaizeData(CancellationToken cancellationToken)
        {
            var someChanges = false;

            var wipeDate = DateTime.Parse("2026-04-22").ToUniversalTime();
            if (DateTime.Now < wipeDate)
            {
                _databaseContext.Colonies.ExecuteDelete();
                someChanges = true;
            }

            if (_databaseContext.Users
                .Include(x => x.Colonies)
                .Any(x => !x.Colonies!.Any(x => !x.Deactivated)))
            {
                var usersWithoutColonies = _databaseContext.Users
                    .Include(x => x.Colonies)
                    .Where(x => !x.Colonies!.Any(x => !x.Deactivated));
                foreach (var user in usersWithoutColonies)
                {
                    var entities = Colony.CreateNew(user.Id);
                    foreach (var entity in entities)
                    {
                        switch (entity)
                        {
                            case Colony colony:
                                _databaseContext.Add(colony.ToEntity());
                                break;
                            case Cycle cycle:
                                _databaseContext.Add(cycle.ToEntity());
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }
                    someChanges = true;
                }
            }

            if (_databaseContext.Colonies
                .Include(x => x.Cycles)
                .Any(x => !x.Cycles!.Any(x => !x.IsComplited)))
            {
                var coloniesWithoutCycles = _databaseContext.Colonies
                    .Include(x => x.Cycles)
                    .Where(x => !x.Cycles!.Any(x => !x.IsComplited));
                foreach (var colony in coloniesWithoutCycles)
                {
                    var cycle = Cycle.CreateNew(colony.Id, prevCycle: null);
                    _databaseContext.Add(cycle.ToEntity());
                    someChanges = true;
                }
            }

            if (someChanges)
                await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
