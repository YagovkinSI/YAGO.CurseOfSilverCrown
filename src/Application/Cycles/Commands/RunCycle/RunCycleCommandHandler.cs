using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Entities.Episodes;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Services;
using static YAGO.World.Application.Cycles.Commands.RunCycle.RunCycleCommandHandler;

namespace YAGO.World.Application.Cycles.Commands.RunCycle
{
    public class RunCycleCommandHandler(
        IColonyRepository colonyRepository,
        ICycleRepository cycleRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<RunCycleCommand, RunCycleResult>
    {
        public async Task<RunCycleResult> Handle(RunCycleCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
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
            await unitOfWorkRepository.UpdateInTransactionAsync(list, cancellationToken);

            return new RunCycleResult(episode);
        }

        private async Task<Cycle> GetLastCycle(long colonyId, CancellationToken cancellationToken)
        {
            var cycle = await cycleRepository.GetLast(colonyId, cancellationToken);

            if (cycle == null || cycle.ReadyForNewCycle())
                cycle = await cycleRepository.CreateNew(colonyId, cancellationToken);

            return cycle;
        }

        public record RunCycleCommand(long UserId) : IRequest<RunCycleResult>;
        public record RunCycleResult(Episode? Episode);
    }
}
