using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Buildings;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal class ColonyWithShipAndBuildingsRepository : IColonyWithShipAndBuildingsRepository
    {
        private readonly IColonyRepository _colonyRepository;
        private readonly IBuildingRepository _buildingRepository;

        public ColonyWithShipAndBuildingsRepository(
            IColonyRepository colonyRepository,
            IBuildingRepository buildingRepository)
        {
            _colonyRepository = colonyRepository;
            _buildingRepository = buildingRepository;
        }

        public async Task<ColonyWithShipAndBuildings?> Find(long colonyId, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.Find(colonyId, cancellationToken);
            if (colony == null)
                return null;

            var ship = Ship.GetDefaultShip();

            var buildings = await _buildingRepository.GetBuildings(colony.BuildingIds, cancellationToken);

            return new ColonyWithShipAndBuildings(
                colony,
                ship,
                buildings);
        }
    }
}
