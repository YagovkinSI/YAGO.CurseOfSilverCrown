using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cycles;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Ships;
using YAGO.World.Infrastructure.Database.Colonies;

namespace YAGO.World.Infrastructure.Database.Cycles
{
    internal class CycleRepository : ICycleRepository
    {
        private readonly ApplicationDbContext _databaseContext;

        public CycleRepository(
            ApplicationDbContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Cycle?> Find(long cycleId, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.Cycles
                .FindAsync([cycleId], cancellationToken);
            return entity?.ToDomain();
        }

        public async Task<Cycle?> GetLast(long colonyId, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.Cycles
                .Where(x => x.ColonyId == colonyId)
                .OrderByDescending(x => x.CompletedUtc ?? DateTime.MaxValue)
                .FirstOrDefaultAsync(cancellationToken);
            return entity?.ToDomain();
        }

        public async Task<Cycle> CreateNew(long colonyId, CancellationToken cancellationToken)
        {
            var entity = await _databaseContext.Cycles
                .FirstOrDefaultAsync(x => x.ColonyId == colonyId && x.CompletedUtc == null, cancellationToken);
            if (entity != null)
                throw new YagoException(string.Format("У колонии {0} уже есть невыполненый цикл.", colonyId));

            var newEntity = CycleEntity.CreateNew(colonyId);
            _databaseContext.Add(newEntity);
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return newEntity.ToDomain();
        }

        public async Task<Cycle> ApplyCycle(long cycleId, CancellationToken cancellationToken)
        {
            var cycleEtity = await _databaseContext.Cycles
                .FindAsync([cycleId], cancellationToken);
            if (cycleEtity == null)
                throw new YagoNotFoundException(nameof(Cycle), cycleId);
            if (cycleEtity.CompletedUtc != null)
                throw new YagoException(string.Format("Цикл {0} уже является завершенным.", cycleId));

            var colonyEntity = await _databaseContext.Colonies
                .FindAsync([cycleEtity.ColonyId], cancellationToken);
            if (colonyEntity == null)
                throw new YagoNotFoundException(nameof(Colony), cycleEtity.ColonyId);

            var income = await CalculateSolarIncome(colonyEntity, cancellationToken);
            colonyEntity.AddSolarsByIncome(income);
            cycleEtity.SetCompleted();
            await _databaseContext.SaveChangesAsync(cancellationToken);

            return cycleEtity.ToDomain();
        }

        private async Task<decimal> CalculateSolarIncome(ColonyEntity colonyEntity, CancellationToken cancellationToken)
        {
            var buildingIds = JsonConvert.DeserializeObject<long[]>(colonyEntity.BuildingIdsJson)!;
            var distinctIds = buildingIds.Distinct().ToArray();
            var buildingsDict = await _databaseContext.Buildings
                .Where(b => distinctIds.Contains(b.Id))
                .ToDictionaryAsync(b => b.Id, cancellationToken);
            var buildings = buildingIds
                .Select(id => buildingsDict[id])
                .ToArray();

            var ship = Ship.GetDefaultShip();

            var income = buildings.Sum(x => x.SolarsIncome) - ship.SolarsConsumption;
            return income;
        }
    }
}