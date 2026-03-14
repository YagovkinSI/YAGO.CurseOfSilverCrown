using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Database;
using YAGO.World.Infrastructure.Database.Colonies;

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

            if (someChanges)
                await _databaseContext.SaveChangesAsync(cancellationToken);
        }
    }
}
