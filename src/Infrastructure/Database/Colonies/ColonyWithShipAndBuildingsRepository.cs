using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Ships;
using YAGO.World.Domain.Units;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal class ColonyWithShipAndBuildingsRepository : IColonyWithShipAndBuildingsRepository
    {
        private readonly IColonyRepository _colonyRepository;

        public ColonyWithShipAndBuildingsRepository(
            IColonyRepository colonyRepository)
        {
            _colonyRepository = colonyRepository;
        }

        public async Task<ColonyWithShipAndBuildings?> Find(long colonyId, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.Find(colonyId, cancellationToken);
            if (colony == null)
                return null;

            var ship = Ship.GetDefaultShip();

            var units = UnitsDataset.GetUnits(colony.UnitIds);

            return new ColonyWithShipAndBuildings(
                colony,
                ship,
                units);
        }
    }
}
