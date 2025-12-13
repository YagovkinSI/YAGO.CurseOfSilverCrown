using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Common.Pagination;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies
{
    public class ColonyService : IColonyService
    {
        private readonly IColonyRepository _colonyRepository;
        private readonly IColonyWithShipAndBuildingsRepository _colonyWithShipAndBuildingsRepository;

        public ColonyService(
            IColonyRepository colonyRepository,
            IColonyWithShipAndBuildingsRepository colonyWithShipAndBuildingsRepository)
        {
            _colonyRepository = colonyRepository;
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
