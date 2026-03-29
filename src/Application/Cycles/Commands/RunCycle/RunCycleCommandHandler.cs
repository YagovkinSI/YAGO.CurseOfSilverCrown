using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Application.Services;
using YAGO.World.Domain.Common.Entities;
using YAGO.World.Domain.Entities.Episodes;
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
            cycle.RunCycle();
            var gameEvents = GameEventsDataset.Get();
            var gameEventGenerateResult = gameEventGenerator.Generate(gameEvents, cycle.StepNumber, colony);
            cycle.SetStepNumber(gameEventGenerateResult.StepNumber, gameEventGenerateResult.IsCycleEnded);
            var episode = gameEventGenerateResult.Episode;
            if (episode.ChangesWithoutChoice != null)
            {
                var colonyStats = colony.Stats;
                colonyStats.SetEpisodeParameters(episode.ChangesWithoutChoice, gameEventGenerateResult.IsCycleEnded);
            }

            var list = new List<IEntity> { colony, cycle };
            await unitOfWorkRepository.UpdateInTransactionAsync(list, cancellationToken);

            return new RunCycleResult(gameEventGenerateResult.Episode, gameEventGenerateResult.IsCycleEnded);
        }

        public record RunCycleCommand(long UserId) : IRequest<RunCycleResult>;
        public record RunCycleResult(Episode? Episode, bool IsCycleCompleted);
    }
}
