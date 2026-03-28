using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Database;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Infrastructure.Database.Colonies;

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

            if (_databaseContext.Colonies.Any(x => !x.StatesJson.Contains("Maintenance")))
            {
                foreach (var colony in _databaseContext.Colonies)
                {
                    var colonyParameters = JsonConvert.DeserializeObject<ColonyParameters>(colony.StatesJson);
                    if (colonyParameters!.Maintenance == 0)
                    {
                        colonyParameters.SetShipParameters();
                        colony.SetStatesJson(colonyParameters);
                        someChanges = true;
                    }
                }
            }

            if (_databaseContext.Colonies.Any(x => !x.StatesJson.Contains("MinningIndustry")))
            {
                foreach (var colony in _databaseContext.Colonies)
                {
                    var colonyParameters = JsonConvert.DeserializeObject<ColonyParameters>(colony.StatesJson);
                    if (colonyParameters!.MinningIndustry == default)
                    {
                        colonyParameters.SetIndustry(colonyParameters.Companies);
                        colony.SetStatesJson(colonyParameters);
                        someChanges = true;
                    }
                }
            }

            if (_databaseContext.Colonies.Any(x => !x.StatesJson.Contains("\"Minning\"")))
            {
                foreach (var colony in _databaseContext.Colonies)
                {
                    var colonyParameters = JsonConvert.DeserializeObject<ColonyParameters>(colony.StatesJson);
                    if (string.IsNullOrEmpty(colonyParameters!.MinningIndustry.Name))
                    {
                        colonyParameters.SetIndustryNames();
                        colony.SetStatesJson(colonyParameters);
                        someChanges = true;
                    }
                }
            }

            if (_databaseContext.Cycles.Any(x => x.State != CycleState.Unknown))
            {
                foreach (var cycle in _databaseContext.Cycles)
                {
                    if (cycle.State == CycleState.Unknown)
                        continue;

                    cycle.UpdateToIsCompleted();
                    someChanges = true;
                }
            }

            var wipeDate = DateTime.Parse("2026-03-24").ToUniversalTime();
            if (_databaseContext.Colonies
                .Include(x => x.User)
                .Any(x => (x.Deactivated && x.DeactivateAtUtc < wipeDate) || x.User == null || x.User.LastActivityAtUtc < wipeDate))
            {
                _databaseContext.Colonies.ExecuteDelete();
                someChanges = true;
            }

            if (someChanges)
                await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
