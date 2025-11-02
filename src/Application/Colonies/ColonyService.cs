using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Buildings;
using YAGO.World.Application.Common.Pagination;
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

        public async Task<ColonyWithShipAndBuildings?> GetMyColonyWithShipAndBuildings(long userId, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(userId, cancellationToken);
            return colony == null ? null : await GetColonyWithShipAndBuildingsDtoInner(colony.Id, cancellationToken);
        }

        public async Task<ColonyWithShipAndBuildings> CreateColony(
            long userId,
            string name,
            ColonyPresetType presetType,
            CancellationToken cancellationToken)
        {
            var userColony = await _colonyRepository.FindByUserId(userId, cancellationToken);
            if (userColony != null)
                throw new YagoException(string.Format("Пользователь уже имеет колонию '{0}'.", userColony.Name));

            var colonyWithName = await _colonyRepository.FindByName(name, cancellationToken);
            if (colonyWithName != null)
                throw new YagoException(string.Format("Название колонии '{0}' уже занято.", name));

            var colony = Colony.CreateNew(userId, name, presetType);
            colony = await _colonyRepository.Add(colony, cancellationToken);

            return await GetColonyWithShipAndBuildingsDtoInner(colony.Id, cancellationToken);
        }

        public async Task<ColonyWithShipAndBuildings> BuyBuilding(
            long userId,
            long buildingId,
            CancellationToken cancellationToken)
        {
            var colony = await GetMyColony(userId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var building = await _buildingRepository.Find(buildingId, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(Building), buildingId);

            var colonyWithShipAndBuildingsDto = await GetColonyWithShipAndBuildingsDtoInner(colony.Id, cancellationToken);

            colonyWithShipAndBuildingsDto.ByuBuilding(building);
            await _colonyRepository.Update(colonyWithShipAndBuildingsDto.Colony, cancellationToken);

            return colonyWithShipAndBuildingsDto;
        }

        public async Task<PaginatedData<ColonyWithShipAndBuildings>> GetPaginatedColonies(
            int page,
            CancellationToken cancellationToken)
        {
            var colonies = await _colonyRepository.GetPaginatedColonies(page, cancellationToken);

            var coloniesWithShipAndBuildings = new List<ColonyWithShipAndBuildings>();
            foreach (var colony in colonies.Data)
            {
                var result = await GetColonyWithShipAndBuildingsDtoInner(colony.Id, cancellationToken);
                coloniesWithShipAndBuildings.Add(result);
            }

            return new PaginatedData<ColonyWithShipAndBuildings>(
                coloniesWithShipAndBuildings.ToArray(),
                colonies.Total,
                colonies.Page,
                colonies.Limit);
        }

        private async Task<ColonyWithShipAndBuildings> GetColonyWithShipAndBuildingsDtoInner(
            long colonyId,
            CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.Find(colonyId, cancellationToken);
            if (colony == null)
                throw new YagoNotFoundException(nameof(Colony), colonyId);

            var ship = Ship.GetDefaultShip();

            var buildings = await _buildingRepository.GetBuildings(colony.BuildingIds, cancellationToken);

            return new ColonyWithShipAndBuildings(
                colony,
                ship,
                buildings);
        }
    }
}
