using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YAGO.World.Application.Interfaces.Repository;
using YAGO.World.Domain.Colonies;
using YAGO.World.Domain.Common.Exceptions;
using YAGO.World.Domain.GameEvents;
using YAGO.World.Domain.GameEvents.Dataset.Prologue;

namespace YAGO.World.Application.Colonies.Commands.CompleteEvent
{
    public class CompleteEventCommandHandler(
        IColonyRepository colonyRepository,
        IColonyEventRepository colonyEventRepository,
        IUnitOfWorkRepository unitOfWorkRepository)
        : IRequestHandler<CompleteEventCommand, CompleteEventResult>
    {
        public async Task<CompleteEventResult> Handle(CompleteEventCommand command, CancellationToken cancellationToken)
        {
            if (command.DilemmaResolving.Contains('#'))
                throw new YagoException("Команда содержит недопустимый символ.");
            var colony = await colonyRepository.FindByUserId(command.UserId, cancellationToken)
                ?? throw new YagoException("Пользователь не имеет колонии.");
            var colonyEvent = await colonyEventRepository.Find(command.ColonyEventId, cancellationToken)
                ?? throw new YagoNotFoundException(nameof(ColonyEvent), command.ColonyEventId.ToString());
            if (colonyEvent.IsCompleted)
                throw new YagoException("Событие уже завершено.");

            var gameEvent = GameEventsDataset.Get(colonyEvent.EventCode);
            var chooseChanges = gameEvent.ChangeList.SingleOrDefault(x => x.Key == command.DilemmaResolving).Value;
            var endChanges = gameEvent.ChangeList.SingleOrDefault(x => x.Key == "#end").Value;
            var eventResult = SetChangesAndCompleteEvent(
                colony, colonyEvent, gameEvent, command.DilemmaResolving, chooseChanges, endChanges);
            var turnNumber = colony.State.Resources.TurnNumber.Value;
            var newColonyEvents = CreateNewEvents(colony, chooseChanges, endChanges, turnNumber);

            await SaveChanges(colony, colonyEvent, newColonyEvents, cancellationToken);

            return new CompleteEventResult(eventResult.Show ? eventResult : null);
        }

        private static void SetChanges(
            Colony colony,
            GameEventChangeList changeList)
        {
            var isAvailable = changeList.Requirements.All(x => x.Check(colony.State));
            if (!isAvailable)
                throw new YagoException("Не выполнены условия.", 400);
            colony.SetChanges(changeList);
        }

        private void SetUnqueChanges(Colony colony, GameEvent gameEvent, string dilemmaResolving)
        {
            if (gameEvent.Code == nameof(ColonyNameEvent) && !string.IsNullOrEmpty(dilemmaResolving))
                colony.SetName(dilemmaResolving);
        }

        private IReadOnlyList<ColonyEvent> GetNewEvents(IEnumerable<string> eventCodes, long colonyId, int turnNumber)
        {
            return eventCodes
                .Select(x => ColonyEvent.CreateNew(colonyId, x, turnNumber))
                .ToList();
        }

        private async Task SaveChanges(
            Colony colony, 
            ColonyEvent colonyEvent, 
            List<ColonyEvent> newColonyEvents, 
            CancellationToken cancellationToken)
        {
            try
            {
                await unitOfWorkRepository.BeginTransactionAsync(cancellationToken);
                await unitOfWorkRepository.Update(colony, cancellationToken);
                await unitOfWorkRepository.Update(colonyEvent, cancellationToken);
                foreach (var newColonyEvent in newColonyEvents)
                    await unitOfWorkRepository.Add(newColonyEvent, cancellationToken);
                await unitOfWorkRepository.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await unitOfWorkRepository.RollbackTransactionAsync(cancellationToken);
                throw;
            }
        }

        private EventResult SetChangesAndCompleteEvent(
            Colony colony,
            ColonyEvent colonyEvent,
            GameEvent gameEvent,
            string dilemmaResolving,
            GameEventChangeList? chooseChanges,
            GameEventChangeList? endChanges)
        {
            var eventResult = gameEvent.Results.FirstOrDefault(x => x.Key == dilemmaResolving).Value
                ?? gameEvent.Results.FirstOrDefault(x => x.Key == "#end").Value
                ?? EventResult.CreateNew();
            eventResult.SetMainParametersBefore(colony);
            if (chooseChanges != null)
                SetChanges(colony, chooseChanges);
            if (endChanges != null)
                SetChanges(colony, endChanges);
            SetUnqueChanges(colony, gameEvent, dilemmaResolving);
            eventResult.SetMainParametersAfter(colony);
            colonyEvent.SetComplited();
            return eventResult;
        }

        private List<ColonyEvent> CreateNewEvents(
            Colony colony, GameEventChangeList chooseChanges, GameEventChangeList endChanges, int turnNumber)
        {
            var newColonyEvents = new List<ColonyEvent>();
            if (chooseChanges != null)
                newColonyEvents.AddRange(GetNewEvents(chooseChanges.NewQuests, colony.Id, turnNumber));
            if (endChanges != null)
                newColonyEvents.AddRange(GetNewEvents(endChanges.NewQuests, colony.Id, turnNumber));
            return newColonyEvents;
        }
    }

    public record CompleteEventCommand(long UserId, long ColonyEventId, string DilemmaResolving) : IRequest<CompleteEventResult>;
    public record CompleteEventResult(EventResult? EventResult);
}
