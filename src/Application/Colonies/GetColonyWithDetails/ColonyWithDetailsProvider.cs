using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Companies;

namespace YAGO.World.Application.Colonies.GetColonyWithDetails
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

            var companies = CompanyDataset.GetCompanies(colony.CompanyIds);

            return new ColonyWithDetails(colony, companies);
        }
    }
}
