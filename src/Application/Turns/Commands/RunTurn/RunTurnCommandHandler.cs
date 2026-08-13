using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.Turns;
using static YAGO.World.Application.Turns.Commands.RunTurn.RunTurnCommandHandler;

namespace YAGO.World.Application.Turns.Commands.RunTurn
{
    public class RunTurnCommandHandler(
        IColonyRepository colonyRepository,
        ITurnRepository turnRepository,
        IGameEventGenerator gameEventGenerator,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<RunTurnCommand, RunTurnResult>
    {
        public async Task<RunTurnResult> Handle(RunTurnCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException($"Отсутствует колония у пользователя с UserId={command.UserId}");
            var turn = await turnRepository.FindLastColonyTurn(colony.Id, cancellationToken)
                ?? Turn.CreateNew(colony.Id, prevTurn: null);
            return await GenerateNextTurn(colony, turn, cancellationToken);
        }

        private async Task<RunTurnResult> GenerateNextTurn(Colony colony, Turn turn, CancellationToken cancellationToken)
        {
            turn.RunTurn();
            var gameEvents = GameEventsDataset.All;
            var gameEventGenerateResult = gameEventGenerator.Generate(gameEvents, colony);

            var eventResult = EventResult.CreateNew();
            eventResult.SetMainParametersBefore(colony);

            colony.SetChanges(gameEventGenerateResult.TurnEndingChangeList);

            var events = gameEventGenerateResult.Events;
            colony.AddEvents([.. events.Select(x => x.Id)]);
            turn.SetCompleted();

            var newTurn = Turn.CreateNew(colony.Id, turn);

            eventResult.SetMainParametersAfter(colony);

            var list = new List<IEntity> { colony, turn, newTurn };
            await unitOfWorkRepository.SaveInTransactionAsync(list, cancellationToken);

            return new RunTurnResult(eventResult);
        }

        public record RunTurnCommand(long UserId) : IRequest<RunTurnResult>;

        public record RunTurnResult(EventResult EventResult);
    }
}
