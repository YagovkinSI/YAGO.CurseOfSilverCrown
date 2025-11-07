using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Buildings;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Buildings;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies
{
    public class ColonyService : IColonyService
    {
        private readonly IColonyRepository _colonyRepository;
        private readonly IBuildingRepository _buildingRepository;
        private readonly IColonyWithShipAndBuildingsRepository _colonyWithShipAndBuildingsRepository;

        public ColonyService(
            IColonyRepository colonyRepository,
            IBuildingRepository buildingRepository,
            IColonyWithShipAndBuildingsRepository colonyWithShipAndBuildingsRepository)
        {
            _colonyRepository = colonyRepository;
            _buildingRepository = buildingRepository;
            _colonyWithShipAndBuildingsRepository = colonyWithShipAndBuildingsRepository;
        }

        public async Task<Colony?> GetMyColony(long userId, CancellationToken cancellationToken)
        {
            return await _colonyRepository.FindByUserId(userId, cancellationToken);
        }

        public async Task<ColonyWithShipAndBuildings?> GetMyColonyWithShipAndBuildings(long userId, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(userId, cancellationToken);
            return colony == null ? null : await _colonyWithShipAndBuildingsRepository.Find(colony.Id, cancellationToken);
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

            return await _colonyWithShipAndBuildingsRepository.Find(colony.Id, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(ColonyWithShipAndBuildings), colony.Id);
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

            var colonyWithShipAndBuildingsDto = await _colonyWithShipAndBuildingsRepository.Find(colony.Id, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(ColonyWithShipAndBuildings), colony.Id);

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
            foreach (var colonyId in colonies.Data.Select(x => x.Id))
            {
                var result = await _colonyWithShipAndBuildingsRepository.Find(colonyId, cancellationToken)
                    ?? throw new YagoNotFoundException(nameof(ColonyWithShipAndBuildings), colonyId);
                coloniesWithShipAndBuildings.Add(result);
            }

            return new PaginatedData<ColonyWithShipAndBuildings>(
                coloniesWithShipAndBuildings.ToArray(),
                colonies.Total,
                colonies.Page,
                colonies.Limit);
        }
    }
}
