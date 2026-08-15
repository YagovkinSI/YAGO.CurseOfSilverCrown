using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;

namespace YAGO.World.Application.Colonies.Commands.RunTurn
{
    public class RunTurnCommandHandler(
        IColonyRepository colonyRepository,
        IGameEventGenerator gameEventGenerator,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<RunTurnCommand, RunTurnResult>
    {
        public async Task<RunTurnResult> Handle(RunTurnCommand command, CancellationToken cancellationToken)
        {
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException($"Отсутствует колония у пользователя с UserId={command.UserId}");

            return await GenerateNextTurn(colony, cancellationToken);
        }

        private async Task<RunTurnResult> GenerateNextTurn(Colony colony, CancellationToken cancellationToken)
        {
            colony.UseTurn(DateTime.UtcNow);

            var gameEvents = GameEventsDataset.All;
            var gameEventGenerateResult = gameEventGenerator.Generate(gameEvents, colony);

            var eventResult = EventResult.CreateNew();
            eventResult.SetMainParametersBefore(colony);
            colony.SetChanges(gameEventGenerateResult.TurnEndingChangeList);
            eventResult.SetMainParametersAfter(colony);

            var events = gameEventGenerateResult.Events;
            var turnNumber = colony.State.Resources.TurnNumber.Value;
            var colonyEvents = events.Select(x => ColonyEvent.CreateNew(colony.Id, x.Code, turnNumber));

            await SaveChanges(colony, colonyEvents, cancellationToken);

            return new RunTurnResult(eventResult);
        }

        private async Task SaveChanges(
            Colony colony, IEnumerable<ColonyEvent> colonyEvents, CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWorkRepository.BeginTransactionAsync(cancellationToken);
                await unitOfWorkRepository.Update(colony, cancellationToken);
                foreach (var colonyEvent in colonyEvents)
                    await unitOfWorkRepository.Add(colonyEvent, cancellationToken);
                await unitOfWorkRepository.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }
    }

    public record RunTurnCommand(long UserId) : IRequest<RunTurnResult>;
    public record RunTurnResult(EventResult EventResult);
}
