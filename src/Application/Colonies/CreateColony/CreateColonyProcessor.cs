using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.CreateColony
{
    public class CreateColonyProcessor : ICreateColonyProcessor
    {
        private readonly IColonyRepository _colonyRepository;
        private readonly IColonyWithShipAndContractsRepository _colonyWithShipAndContractsRepository;

        public CreateColonyProcessor(
            IColonyRepository colonyRepository,
            IColonyWithShipAndContractsRepository colonyWithShipAndContractsRepository)
        {
            _colonyRepository = colonyRepository;
            _colonyWithShipAndContractsRepository = colonyWithShipAndContractsRepository;
        }

        public async Task<CreateColonyResult> Execute(CreateColonyCommand command, CancellationToken cancellationToken)
        {
            var userColony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (userColony != null)
                throw new YagoException(string.Format("Пользователь уже имеет колонию '{0}'.", userColony.Name));

            var colonyWithName = await _colonyRepository.FindByName(command.ColonyName, cancellationToken);
            if (colonyWithName != null)
                throw new YagoException(string.Format("Название колонии '{0}' уже занято.", command.ColonyName));

            var colony = Colony.CreateNew(command.UserId, command.ColonyName, command.GavernorType);
            colony = await _colonyRepository.Add(colony, cancellationToken);

            var colonyCreated = await _colonyWithShipAndContractsRepository.Find(colony.Id, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(ColonyWithShipAndContracts), colony.Id);

            return new CreateColonyResult(colonyCreated);
        }
    }
}
