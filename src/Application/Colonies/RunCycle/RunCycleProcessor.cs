using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cycles;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Exceptions;

namespace YAGO.World.Application.Colonies.RunCycle
{
    public class RunCycleProcessor : IRunCycleProcessor
    {
        private readonly IColonyService _colonyService;
        private readonly ICycleService _cycleService;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public RunCycleProcessor(
            IColonyService colonyService,
            ICycleService cycleService,
            IUnitOfWorkRepository unitOfWorkRepository)
        {
            _colonyService = colonyService;
            _cycleService = cycleService;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<RunCycleResult> Execute(RunCycleCommand command, CancellationToken cancellationToken)
        {
            var userId = command.UserId;

            var colonyWithShipAndContracts = await _colonyService.GetMyColonyWithDetails(userId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var lastCycle = await _cycleService.GetMyLastCycle(userId, cancellationToken)
                ?? throw new YagoException("Цикл отсутствует. Вероятно нет созданной колонии.");

            var notification = lastCycle.RunCycle(colonyWithShipAndContracts);

            var list = new List<IEntity>
            {
                colonyWithShipAndContracts.Colony,
                lastCycle
            };
            await _unitOfWorkRepository.UpdateInTransactionAsync(list, cancellationToken);

            var myCycle = await _cycleService.GetMyLastCycle(userId, cancellationToken);

            return new RunCycleResult(notification, myCycle, colonyWithShipAndContracts);
        }
    }
}
