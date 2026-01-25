using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Database;
using YAGO.World.Domain.Colonies;
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

            if (_databaseContext.Colonies.Any(x => x.StatesJson == "[]"))
            {
                foreach (var colony in _databaseContext.Colonies)
                {
                    var buildingIds = JsonConvert.DeserializeObject<long[]>(colony.BuildingIdsJson);
                    var startGavernorType = (GavernorType)buildingIds[0];
                    var contracts = GetContracts(buildingIds);
                    var colonyParameters = new ColonyParameters(shipId: 1, startGavernorType, contracts);
                    colony.SetStatesJson(colonyParameters);
                    someChanges = true;
                }
            }

            if (_databaseContext.Colonies.Any(x => !x.StatesJson.Contains("ShipId")))
            {
                foreach (var colony in _databaseContext.Colonies)
                {
                    var colonyParameters = JsonConvert.DeserializeObject<ColonyParameters>(colony.StatesJson);
                    colonyParameters.SetShipDefault();
                    colony.SetStatesJson(colonyParameters);
                    someChanges = true;
                }
            }

            if (someChanges)
                await _databaseContext.SaveChangesAsync(cancellationToken);
        }

        private Dictionary<long, int> GetContracts(long[] buildingIds)
        {
            var result = new Dictionary<long, int>();
            for (var i = 1; i < 4; i++)
            {
                var count = buildingIds.Count(x => x == i);
                if (count == 0)
                    continue;

                result.Add(i, count);
            }
            return result;
        }
    }
}
