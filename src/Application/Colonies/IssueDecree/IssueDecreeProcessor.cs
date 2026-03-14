using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Companies;
using YAGO.World.Domain.Entities.Decrees;
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

            var companies = CompanyDataset.GetCompanies(colony.CompanyIds);

            decree.IssueDecree(colony, companies);
            await _colonyRepository.Update(colony, cancellationToken);

            var colonyWithDetails = new ColonyWithDetails(colony, companies);
            return new IssueDecreeResult(colonyWithDetails);
        }
    }
}
