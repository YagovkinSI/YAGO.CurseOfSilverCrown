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
        private readonly IColonyWithShipAndContractsRepository _colonyWithShipAndContractsRepository;

        public ColonyService(
            IColonyRepository colonyRepository,
            IColonyWithShipAndContractsRepository colonyWithShipAndContractsRepository)
        {
            _colonyRepository = colonyRepository;
            _colonyWithShipAndContractsRepository = colonyWithShipAndContractsRepository;
        }

        public async Task<Colony?> GetMyColony(long userId, CancellationToken cancellationToken)
        {
            return await _colonyRepository.FindByUserId(userId, cancellationToken);
        }

        public async Task<ColonyWithShipAndContracts?> GetMyColonyWithShipAndContracts(long userId, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(userId, cancellationToken);
            return colony == null ? null : await _colonyWithShipAndContractsRepository.Find(colony.Id, cancellationToken);
        }

        public async Task<PaginatedData<ColonyWithShipAndContracts>> GetPaginatedColonies(
            int page,
            CancellationToken cancellationToken)
        {
            var colonies = await _colonyRepository.GetPaginatedColonies(page, cancellationToken);

            var coloniesWithShipAndContracts = new List<ColonyWithShipAndContracts>();
            foreach (var colonyId in colonies.Data.Select(x => x.Id))
            {
                var result = await _colonyWithShipAndContractsRepository.Find(colonyId, cancellationToken)
                    ?? throw new YagoNotFoundException(nameof(ColonyWithShipAndContracts), colonyId);
                coloniesWithShipAndContracts.Add(result);
            }

            return new PaginatedData<ColonyWithShipAndContracts>(
                coloniesWithShipAndContracts.ToArray(),
                colonies.Total,
                colonies.Page,
                colonies.Limit);
        }
    }
}
