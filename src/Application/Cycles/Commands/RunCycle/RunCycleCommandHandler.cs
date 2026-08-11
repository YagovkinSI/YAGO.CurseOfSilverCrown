using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.Cycles;
using YAGO.World.Domain.GameEvents;
using static YAGO.World.Application.Cycles.Commands.RunCycle.RunCycleCommandHandler;

namespace YAGO.World.Application.Cycles.Commands.RunCycle
{
    public class RunCycleCommandHandler(
        IColonyRepository colonyRepository,
        ICycleRepository cycleRepository,
        IGameEventGenerator gameEventGenerator,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<RunCycleCommand, RunCycleResult>
    {
        public async Task<RunCycleResult> Handle(RunCycleCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException($"Отсутствует колония у пользователя с UserId={command.UserId}");
            var cycle = await cycleRepository.FindLastColonyCycle(colony.Id, cancellationToken)
                ?? Cycle.CreateNew(colony.Id, prevCycle: null);
            return await GenerateNextCycle(colony, cycle, cancellationToken);
        }

        private async Task<RunCycleResult> GenerateNextCycle(Colony colony, Cycle cycle, CancellationToken cancellationToken)
        {
            cycle.RunCycle();
            var gameEvents = GameEventsDataset.All;
            var gameEventGenerateResult = gameEventGenerator.Generate(gameEvents, colony);

            var eventResult = EventResult.CreateNew();
            eventResult.SetMainParametersBefore(colony);

            colony.SetChanges(gameEventGenerateResult.CycleEndingChangeList);

            var events = gameEventGenerateResult.Events;
            colony.AddEvents([.. events.Select(x => x.Id)]);
            cycle.SetCompleted();

            var newCycle = Cycle.CreateNew(colony.Id, cycle);

            eventResult.SetMainParametersAfter(colony);

            var list = new List<IEntity> { colony, cycle, newCycle };
            await unitOfWorkRepository.SaveInTransactionAsync(list, cancellationToken);

            return new RunCycleResult(eventResult);
        }

        public record RunCycleCommand(long UserId) : IRequest<RunCycleResult>;

        public record RunCycleResult(EventResult EventResult);
    }
}
