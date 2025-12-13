using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.СreateColony
{
    public class CreateColonyProcessor : ICreateColonyProcessor
    {
        private readonly IColonyRepository _colonyRepository;
        private readonly IColonyWithShipAndBuildingsRepository _colonyWithShipAndBuildingsRepository;

        public CreateColonyProcessor(
            IColonyRepository colonyRepository,
            IColonyWithShipAndBuildingsRepository colonyWithShipAndBuildingsRepository)
        {
            _colonyRepository = colonyRepository;
            _colonyWithShipAndBuildingsRepository = colonyWithShipAndBuildingsRepository;
        }

        public async Task<CreateColonyResult> Execute(CreateColonyCommand command, CancellationToken cancellationToken)
        {
            var userColony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken);
            if (userColony != null)
                throw new YagoException(string.Format("Пользователь уже имеет колонию '{0}'.", userColony.Name));

            var colonyWithName = await _colonyRepository.FindByName(command.ColonyName, cancellationToken);
            if (colonyWithName != null)
                throw new YagoException(string.Format("Название колонии '{0}' уже занято.", command.ColonyName));

            var colony = Colony.CreateNew(command.UserId, command.ColonyName, command.PresetType);
            colony = await _colonyRepository.Add(colony, cancellationToken);

            var colonyCreated = await _colonyWithShipAndBuildingsRepository.Find(colony.Id, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(ColonyWithShipAndBuildings), colony.Id);

            return new CreateColonyResult(colonyCreated);
        }
    }
}
