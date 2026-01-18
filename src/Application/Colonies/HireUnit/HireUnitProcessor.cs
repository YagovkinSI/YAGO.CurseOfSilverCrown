using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Units;

namespace YAGO.World.Application.Colonies.HireUnit
{
    public class HireUnitProcessor : IHireUnitProcessor
    {
        private readonly IColonyRepository _colonyRepository;
        private readonly IColonyWithShipAndContractsRepository _colonyWithShipAndContractsRepository;

        public HireUnitProcessor(
            IColonyRepository colonyRepository,
            IColonyWithShipAndContractsRepository colonyWithShipAndContractsRepository)
        {
            _colonyRepository = colonyRepository;
            _colonyWithShipAndContractsRepository = colonyWithShipAndContractsRepository;
        }

        public async Task<HireUnitResult> Execute(HireUnitCommand command, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var allUnits = ContractDataset.Get().ToList();
            var unit = allUnits.Find(x => x.Id == command.UnitId)
                ?? throw new YagoNotFoundException(nameof(Contract), command.UnitId);

            var colonyWithShipAndContractsDto = await _colonyWithShipAndContractsRepository.Find(colony.Id, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(ColonyWithShipAndContracts), colony.Id);

            colonyWithShipAndContractsDto.СoncludeСontract(unit);
            await _colonyRepository.Update(colonyWithShipAndContractsDto.Colony, cancellationToken);

            return new HireUnitResult(colonyWithShipAndContractsDto);
        }
    }
}
