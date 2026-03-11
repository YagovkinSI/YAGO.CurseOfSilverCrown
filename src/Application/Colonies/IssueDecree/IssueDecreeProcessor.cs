using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Colonies.Companies;
using YAGO.World.Domain.Colonies.Ships;
using YAGO.World.Domain.Decrees;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.IssueDecree
{
    public class IssueDecreeProcessor : IIssueDecreeProcessor
    {
        private readonly IColonyRepository _colonyRepository;

        public IssueDecreeProcessor(
            IColonyRepository colonyRepository)
        {
            _colonyRepository = colonyRepository;
        }

        public async Task<IssueDecreeResult> Execute(IssueDecreeCommand command, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var allContracts = DecreeDataset.Get().ToList();
            var decree = allContracts.Find(x => x.Id == command.DecreeId)
                ?? throw new YagoNotFoundException(nameof(Decree), command.DecreeId);

            var policies = colony.Policies;
            var ship = ShipDataset.GetShip(policies.ShipId);
            var colonyStats = colony.Stats;
            var companies = CompanyDataset.GetCompanies(colonyStats.CompanyIds);

            decree.IssueDecree(colony, ship, companies);
            await _colonyRepository.Update(colony, cancellationToken);

            var colonyWithDetails = new ColonyWithDetails(colony, ship, companies);
            return new IssueDecreeResult(colonyWithDetails);
        }
    }
}
