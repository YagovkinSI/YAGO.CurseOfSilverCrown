using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies;
using YAGO.World.Domain.Companies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Application.GetColonyWithDetails
{
    public class ColonyWithDetailsProvider : IColonyWithDetailsProvider
    {
        private readonly IColonyRepository _colonyRepository;

        public ColonyWithDetailsProvider(IColonyRepository colonyRepository)
        {
            _colonyRepository = colonyRepository;
        }

        public async Task<ColonyWithDetails?> Execute(GetColonyWithDetailsCommand command, CancellationToken cancellationToken)
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
