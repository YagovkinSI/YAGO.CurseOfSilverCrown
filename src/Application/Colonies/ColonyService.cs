using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Buildings;
using YAGO.World.Domain.Buildings;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Application.Colonies
{
    public class ColonyService : IColonyService
    {
        private readonly IColonyRepository _colonyRepository;
        private readonly IBuildingRepository _buildingRepository;

        public ColonyService(
            IColonyRepository colonyRepository,
            IBuildingRepository buildingRepository)
        {
            _colonyRepository = colonyRepository;
            _buildingRepository = buildingRepository;
        }

        public async Task<Colony?> GetMyColony(long userId, CancellationToken cancellationToken)
        {
            return await _colonyRepository.FindByUserId(userId, cancellationToken);
        }

        public async Task<ColonyWithShipAndBuildingsDto?> GetMyColonyWithShipAndBuildings(long userId, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(userId, cancellationToken);
            return colony == null ? null : await GetColonyWithShipAndBuildingsDtoInner(colony.Id, cancellationToken);
        }

        public async Task<ColonyWithShipAndBuildingsDto> CreateColony(long userId, string name, ColonyPresetType presetType, CancellationToken cancellationToken)
        {
            var userColony = await _colonyRepository.FindByUserId(userId, cancellationToken);
            if (userColony != null)
                throw new YagoException(string.Format("Пользователь уже имеет колонию '{0}'.", userColony.Name));

            var colonyWithName = await _colonyRepository.FindByName(name, cancellationToken);
            if (colonyWithName != null)
                throw new YagoException(string.Format("Название колонии '{0}' уже занято.", name));

            var createColonyDto = new CreateColonyDto(
                userId,
                name,
                presetType);
            var colony = await _colonyRepository.CreateColomy(createColonyDto, cancellationToken);

            return await GetColonyWithShipAndBuildingsDtoInner(colony.Id, cancellationToken);
        }

        public async Task<ColonyWithShipAndBuildingsDto> BuyBuilding(
            long userId, 
            long buildingId, 
            CancellationToken cancellationToken)
        {
            var colony = await GetMyColony(userId, cancellationToken);
            if (colony == null)
                throw new YagoException("Пользователь не имеет колонии.");

            var building = await _buildingRepository.Find(buildingId, cancellationToken);
            if (building == null)
                throw new YagoNotFoundException(nameof(Building), buildingId);

            var colonyWithShipAndBuildingsDto = await GetColonyWithShipAndBuildingsDtoInner(colony.Id, cancellationToken);

            if (colonyWithShipAndBuildingsDto.Colony.Solars < building.Cost)
                throw new YagoException("Недостаточно средств.");

            if (colonyWithShipAndBuildingsDto.Ship.Zones - colonyWithShipAndBuildingsDto.ZonesOccupied < building.ZonesOccupied)
                throw new YagoException("Недостаточно секторов.");

            await _colonyRepository.ByuBuilding(colony.Id, building, cancellationToken);

            return await GetColonyWithShipAndBuildingsDtoInner(colony.Id, cancellationToken);
        }

        private async Task<ColonyWithShipAndBuildingsDto> GetColonyWithShipAndBuildingsDtoInner(long colonyId, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.Find(colonyId, cancellationToken);
            if (colony == null)
                throw new YagoNotFoundException(nameof(Colony), colonyId);

            var ship = Ship.GetDefaultShip();

            var buildings = await _buildingRepository.GetBuildings(colony.BuildingIds, cancellationToken);

            return new ColonyWithShipAndBuildingsDto(
                colony,
                ship,
                buildings,
                colony.CalculateSolarIncome(buildings, ship),
                colony.CalculateReputation(buildings),
                colony.CalculatePopulation(buildings),
                colony.CalculateZonesOccupied(buildings));
        }
    }
}
