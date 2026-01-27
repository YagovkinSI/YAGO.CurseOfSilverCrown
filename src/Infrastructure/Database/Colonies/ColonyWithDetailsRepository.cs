using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Contracts;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal class ColonyWithDetailsRepository : IColonyWithDetailsRepository
    {
        private readonly IColonyRepository _colonyRepository;

        public ColonyWithDetailsRepository(
            IColonyRepository colonyRepository)
        {
            _colonyRepository = colonyRepository;
        }

        public async Task<ColonyWithDetails?> Find(long colonyId, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.Find(colonyId, cancellationToken);
            if (colony == null)
                return null;

            var ship = ShipDataset.GetShip(colony.ShipId);

            var contracts = ContractDataset.GetContracts(colony.Contracts);

            return new ColonyWithDetails(
                colony,
                ship,
                contracts);
        }
    }
}
