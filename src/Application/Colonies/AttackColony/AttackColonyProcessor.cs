using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Colonies.RunCycle;
using YAGO.World.Application.Cycles;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.AttackColony
{
    public class AttackColonyProcessor : IAttackColonyProcessor
    {
        private readonly IColonyService _colonyService;
        private readonly ICycleService _cycleService;
        private readonly IColonyWithShipAndBuildingsRepository _colonyWithShipAndBuildingsRepository;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public AttackColonyProcessor(
            IColonyService colonyService,
            ICycleService cycleService,
            IColonyWithShipAndBuildingsRepository colonyWithShipAndBuildingsRepository,
            IUnitOfWorkRepository unitOfWorkRepository)
        {
            _colonyService = colonyService;
            _cycleService = cycleService;
            _colonyWithShipAndBuildingsRepository = colonyWithShipAndBuildingsRepository;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<AttackColonyResult> Execute(AttackColonyCommand command, CancellationToken cancellationToken)
        {
            var colonyWithShipAndBuildings = await _colonyService.GetMyColonyWithShipAndBuildings(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var lastCycle = await _cycleService.GetMyLastCycle(command.UserId, cancellationToken)
                ?? throw new YagoException("Цикл отсутствует. Вероятно нет созданной колонии.");

            var targetColony = await _colonyWithShipAndBuildingsRepository.Find(command.TargetColonyId, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(Colony), command.TargetColonyId);

            colonyWithShipAndBuildings.AttackColony(targetColony, command.PrizeType);

            lastCycle.SetCompleted();

            var list = new List<IEntity>
            {
                colonyWithShipAndBuildings.Colony,
                targetColony.Colony,
                lastCycle
            };
            await _unitOfWorkRepository.UpdateInTransactionAsync(list, cancellationToken);

            var myCycle = await _cycleService.GetMyLastCycle(command.UserId, cancellationToken);

            return new AttackColonyResult(myCycle, colonyWithShipAndBuildings);
        }
    }
}
