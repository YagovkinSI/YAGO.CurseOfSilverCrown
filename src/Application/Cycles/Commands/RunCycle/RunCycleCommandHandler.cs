using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Entities;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.Cycles;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Services;
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
            var gameEvents = GameEventsDataset.GetAll();
            var gameEventGenerateResult = gameEventGenerator.Generate(gameEvents, colony);
            var colonyStats = colony.Stats;
            var events = gameEventGenerateResult.Events;
            foreach (var gameEvent in events)
            {
                if (gameEvent.ChangeList.ContainsKey("init"))
                {
                    colony.SetChanges(gameEvent.ChangeList["init"]);
                }
            }
            colonyStats.AddCurrentWeek();
            cycle.SetCompleted();

            var newCycle = Cycle.CreateNew(colony.Id, cycle);

            colony.UpdateQuests([.. events.Select(x => x.Id)]);

            var list = new List<IEntity> { colony, cycle, newCycle };
            await unitOfWorkRepository.SaveInTransactionAsync(list, cancellationToken);

            var episodesForColony = events.Select(x => new ColonyEpisode(x.Episode, colony.Stats)).ToList();
            return new RunCycleResult(newCycle, episodesForColony);
        }

        public record RunCycleCommand(long UserId) : IRequest<RunCycleResult>;
        public record RunCycleResult(Cycle Cycle, IReadOnlyList<ColonyEpisode> Episodes);
    }
}
