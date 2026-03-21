using MediatR;
using System;
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

            var cycle = await cycleRepository.GetLast(colony.Id, cancellationToken);
            if (cycle == null || cycle.State == CycleState.Completed)
                cycle = Cycle.CreateNew(colony.Id, cycle);

            if (cycle.StartAtUtc > DateTime.UtcNow)
                throw new YagoException("Цикл не готов к запуску. Дождитесь готовности не более двух минут.");

            var episode = RunCycleService.RunCycle(cycle, colony);

            var list = new List<IEntity>
            {
                colony,
                cycle
            };
            await unitOfWorkRepository.UpdateInTransactionAsync(list, cancellationToken);

            return new RunCycleResult(episode, cycle.State == CycleState.Completed);
        }

        public record RunCycleCommand(long UserId) : IRequest<RunCycleResult>;
        public record RunCycleResult(Episode? Episode, bool IsCycleCompleted);
    }
}
