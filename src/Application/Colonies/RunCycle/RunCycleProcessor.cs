using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cycles;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Cycles;
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
            var cyrcle = await RunCycle(command.UserId, cancellationToken);
            return new RunCycleResult(cyrcle);
        }

        public async Task<Cycle?> RunCycle(long userId, CancellationToken cancellationToken)
        {
            var colonyWithShipAndBuildings = await _colonyService.GetMyColonyWithShipAndBuildings(userId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var lastCycle = await _cycleService.GetMyLastCycle(userId, cancellationToken)
                ?? throw new YagoException("Цикл отсутствует. Вероятно нет созданной колонии.");

            colonyWithShipAndBuildings.AddIncome();
            lastCycle.SetCompleted();

            var list = new List<IEntity>
            {
                colonyWithShipAndBuildings.Colony,
                lastCycle
            };
            await _unitOfWorkRepository.UpdateInTransactionAsync(list, cancellationToken);

            return await _cycleService.GetMyLastCycle(userId, cancellationToken);
        }
    }
}
