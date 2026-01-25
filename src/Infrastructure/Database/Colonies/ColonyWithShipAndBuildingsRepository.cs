using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Contracts;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Infrastructure.Database.Colonies
{
    internal class ColonyWithShipAndContractsRepository : IColonyWithShipAndContractsRepository
    {
        private readonly IColonyRepository _colonyRepository;

        public ColonyWithShipAndContractsRepository(
            IColonyRepository colonyRepository)
        {
            _colonyRepository = colonyRepository;
        }

        public async Task<ColonyWithShipAndContracts?> Find(long colonyId, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.Find(colonyId, cancellationToken);
            if (colony == null)
                return null;

            var ship = ShipDataset.GetShip(colony.ShipId);

            var contracts = ContractDataset.GetContracts(colony.Contracts);

            return new ColonyWithShipAndContracts(
                colony,
                ship,
                contracts);
        }
    }
}
