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
        private readonly IColonyWithShipAndBuildingsRepository _colonyWithShipAndBuildingsRepository;

        public HireUnitProcessor(
            IColonyRepository colonyRepository,
            IColonyWithShipAndBuildingsRepository colonyWithShipAndBuildingsRepository)
        {
            _colonyRepository = colonyRepository;
            _colonyWithShipAndBuildingsRepository = colonyWithShipAndBuildingsRepository;
        }

        public async Task<HireUnitResult> Execute(HireUnitCommand command, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var allUnits = UnitsDataset.Get().ToList();
            var unit = allUnits.Find(x => x.Id == command.UnitId)
                ?? throw new YagoNotFoundException(nameof(Unit), command.UnitId);

            var colonyWithShipAndBuildingsDto = await _colonyWithShipAndBuildingsRepository.Find(colony.Id, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(ColonyWithShipAndBuildings), colony.Id);

            colonyWithShipAndBuildingsDto.HiringUnit(unit);
            await _colonyRepository.Update(colonyWithShipAndBuildingsDto.Colony, cancellationToken);

            return new HireUnitResult(colonyWithShipAndBuildingsDto);
        }
    }
}
