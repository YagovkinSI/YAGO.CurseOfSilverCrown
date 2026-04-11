using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Services;
using YAGO.World.Domain.Aggregates.ColonyEpisodes;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Entities.Colonies;
using YAGO.World.Domain.Entities.GameEvents;
using YAGO.World.Domain.Exceptions;
using YAGO.World.Domain.Services;
using static YAGO.World.Application.Cycles.Commands.RunCycle.RunCycleCommandHandler;

namespace YAGO.World.Application.Cycles.Commands.RunCycle
{
    public class RunCycleCommandHandler(
        IColonyRepository colonyRepository,
        ICurrentCycleProvider currentCycleProvider,
        IGameEventGenerator gameEventGenerator,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<RunCycleCommand, RunCycleResult>
    {
        public async Task<RunCycleResult> Handle(RunCycleCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");
            var cycle = await currentCycleProvider.Get(colony.Id, cancellationToken);
            if (cycle.ActiveEventId != null)
                return GetActiveEvent(cycle.ActiveEventId, colony.Stats);

            var gameEvents = GameEventsDataset.GetAll();
            cycle.RunCycle();
            var gameEventGenerateResult = gameEventGenerator.Generate(gameEvents, cycle.StepNumber, colony);
            var episode = gameEventGenerateResult.Episode;
            var activeEvent = episode.Dilemma != null ? episode.Id : null;
            cycle.SetStepNumber(gameEventGenerateResult.StepNumber, activeEvent, gameEventGenerateResult.IsCycleEnded);
            if (episode.ChangesWithoutChoice != null)
            {
                var colonyStats = colony.Stats;
                colonyStats.SetEpisodeParameters(episode.ChangesWithoutChoice, gameEventGenerateResult.IsCycleEnded);
            }

            var list = new List<IEntity> { colony, cycle };
            await unitOfWorkRepository.UpdateInTransactionAsync(list, cancellationToken);

            var episodeForColony = new ColonyEpisode(episode, colony.Stats);
            return new RunCycleResult(episodeForColony, gameEventGenerateResult.IsCycleEnded);
        }

        private RunCycleResult GetActiveEvent(string activeEvent, ColonyStats colonyStats)
        {
            var gameEvent = GameEventsDataset.Get(activeEvent);
            var episodeForColony = new ColonyEpisode(gameEvent.Episode, colonyStats);
            return new RunCycleResult(episodeForColony, IsCycleCompleted: false);
        }

        public record RunCycleCommand(long UserId) : IRequest<RunCycleResult>;
        public record RunCycleResult(ColonyEpisode Episode, bool IsCycleCompleted);
    }
}
