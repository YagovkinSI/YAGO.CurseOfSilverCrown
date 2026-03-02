using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.Colonies.Companies;
using YAGO.World.Domain.Colonies.Ships;
using YAGO.World.Domain.Decrees;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.GetColonyWithDetails
{
    public class ColonyWithDetailsProvider : IColonyWithDetailsProvider
    {
        private readonly IColonyRepository _colonyRepository;

        public ColonyWithDetailsProvider(IColonyRepository colonyRepository)
        {
            _colonyRepository = colonyRepository;
        }

        public async Task<ColonyWithDetails?> Get(GetColonyWithDetailsCommand command, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (colony == null)
                return null;

            var ship = ShipDataset.GetShip(colony.ShipId);
            var companies = CompanyDataset.GetCompanies(colony.CompanyIds);

            return new ColonyWithDetails(colony, ship, companies);
        }
    }
}
