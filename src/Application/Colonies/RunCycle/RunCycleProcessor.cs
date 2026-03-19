using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Cycles;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Services;

namespace YAGO.World.Application.Colonies.RunCycle
{
    public class RunCycleProcessor : IRunCycleProcessor
    {
        private readonly IColonyRepository _colonyRepository;
        private readonly ICycleProvider _cycleProvider;
        private readonly IUnitOfWorkRepository _unitOfWorkRepository;

        public RunCycleProcessor(
            IColonyRepository colonyRepository,
            ICycleProvider cycleService,
            IUnitOfWorkRepository unitOfWorkRepository)
        {
            _colonyRepository = colonyRepository;
            _cycleProvider = cycleService;
            _unitOfWorkRepository = unitOfWorkRepository;
        }

        public async Task<RunCycleResult> Execute(RunCycleCommand command, CancellationToken cancellationToken)
        {
            var colony = await _colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");

            var lastCycle = await GetLastCycle(command.UserId, cancellationToken);

            if (lastCycle.State == CycleState.Completed)
                throw new YagoException("Цикл завершен. Дождитесь следующего цикла не более двух минут.");

            var episode = RunCycleService.RunCycle(lastCycle, colony);

            var list = new List<IEntity>
            {
                colony,
                lastCycle
            };
            await _unitOfWorkRepository.UpdateInTransactionAsync(list, cancellationToken);

            var myCycle = await GetLastCycle(command.UserId, cancellationToken);

            return new RunCycleResult(episode, colony, myCycle);
        }

        private async Task<Cycle> GetLastCycle(long userId, CancellationToken cancellationToken)
        {
            var command = new GetCycleCommand(userId);
            return await _cycleProvider.Get(command, cancellationToken)
                ?? throw new YagoException("Цикл отсутствует. Вероятно нет созданной колонии.");
        }
    }
}
