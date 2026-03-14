using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Companies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.CreateColony
{
    public class CreateColonyProcessor : ICreateColonyProcessor
    {
        private readonly IColonyRepository _colonyRepository;

        public CreateColonyProcessor(
            IColonyRepository colonyRepository)
        {
            _colonyRepository = colonyRepository;
        }

        public async Task<CreateColonyResult> Execute(CreateColonyCommand command, CancellationToken cancellationToken)
        {
            var userColony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (userColony != null)
                throw new YagoException(string.Format("Пользователь уже имеет колонию '{0}'.", userColony.Name));

            var isNameAvailable = await _colonyRepository.IsNameAvailable(command.ColonyName, cancellationToken);
            if (!isNameAvailable)
                throw new YagoException(string.Format("Название колонии '{0}' уже занято.", command.ColonyName));

            var colony = Colony.CreateNew(command.UserId, command.ColonyName, command.GavernorType);
            colony = await _colonyRepository.Add(colony, cancellationToken);

            var colonyCreated = await _colonyRepository.Find(colony.Id, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(Colony), colony.Id);

            var companies = CompanyDataset.GetCompanies(colony.CompanyIds);
            var colonyWithDetails = new ColonyWithDetails(colonyCreated, companies);
            return new CreateColonyResult(colonyWithDetails);
        }
    }
}
