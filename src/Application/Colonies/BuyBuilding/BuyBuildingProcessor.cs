using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Buildings;
using YAGO.World.Domain.Buildings;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.BuyBuilding
{
    public class BuyBuildingProcessor : IBuyBuildingProcessor
    {
        private readonly IBuildingRepository _buildingRepository;
        private readonly IColonyRepository _colonyRepository;
        private readonly IColonyWithShipAndBuildingsRepository _colonyWithShipAndBuildingsRepository;

        public BuyBuildingProcessor(
            IBuildingRepository buildingRepository,
            IColonyRepository colonyRepository,
            IColonyWithShipAndBuildingsRepository colonyWithShipAndBuildingsRepository)
        {
            _buildingRepository = buildingRepository;
            _colonyRepository = colonyRepository;
            _colonyWithShipAndBuildingsRepository = colonyWithShipAndBuildingsRepository;
        }

        public async Task<BuyBuildingResult> Execute(BuyBuildingCommand command, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var building = await _buildingRepository.Find(command.BuildingId, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(Building), command.BuildingId);

            var colonyWithShipAndBuildingsDto = await _colonyWithShipAndBuildingsRepository.Find(colony.Id, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(ColonyWithShipAndBuildings), colony.Id);

            colonyWithShipAndBuildingsDto.ByuBuilding(building);
            await _colonyRepository.Update(colonyWithShipAndBuildingsDto.Colony, cancellationToken);

            return new BuyBuildingResult(colonyWithShipAndBuildingsDto);
        }
    }
}
