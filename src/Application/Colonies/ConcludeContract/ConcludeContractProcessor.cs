using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Companies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Ships;

namespace YAGO.World.Application.Colonies.ConcludeContract
{
    public class ConcludeContractProcessor : IConcludeContractProcessor
    {
        private readonly IColonyRepository _colonyRepository;

        public ConcludeContractProcessor(
            IColonyRepository colonyRepository)
        {
            _colonyRepository = colonyRepository;
        }

        public async Task<ConcludeContractResult> Execute(ConcludeContractCommand command, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var allContracts = CompanyDataset.Get().ToList();
            var company = allContracts.Find(x => x.Id == command.СontractId)
                ?? throw new YagoNotFoundException(nameof(Company), command.СontractId);

            var ship = ShipDataset.GetShip(colony.ShipId);
            var companies = CompanyDataset.GetCompanies(colony.CompanyIds);

            company.СoncludeСontract(colony, ship, companies);
            await _colonyRepository.Update(colony, cancellationToken);

            var colonyWithDetails = new ColonyWithDetails(colony, ship, companies);
            return new ConcludeContractResult(colonyWithDetails);
        }
    }
}
